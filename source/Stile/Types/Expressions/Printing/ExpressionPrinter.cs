#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Stile.Types.Enums;
using Stile.Types.Expressions.Printing.Tokens;
#endregion

namespace Stile.Types.Expressions.Printing
{
	public class ExpressionPrinter
	{
		private readonly ParserFactory _factory;
		private readonly TokenFormat _format;

		public ExpressionPrinter(ParserFactory factory = null, TokenFormat format = null)
		{
			_factory = factory;
			_format = format;
		}

		public static ExpressionPrinter Make(VersionedLanguage language = VersionedLanguage.CSharp4)
		{
			switch (language)
			{
				case VersionedLanguage.CSharp4:
					return new ExpressionPrinter();
			}
			throw Enumeration.FailedToRecognize(() => language);
		}

		public Lazy<string> Print(Expression expression, Dictionary<string, string> parameterAliases = null)
		{
			return new Lazy<string>(() => LazyPrint(expression, parameterAliases));
		}

		private string LazyPrint(Expression expression, Dictionary<string, string> parameterAliases = null)
		{
			var session = new Session(_factory, _format, parameterAliases);
			session.Append(expression);
			return session.Print();
		}

		public class Session : IPrintStrategy
		{
			private readonly ParserFactory _factory;
			private readonly Dictionary<string, string> _parameterAliases;
			private readonly StringBuilder _stringBuilder;
			private bool _first;

			public Session(ParserFactory factory = null,
				TokenFormat format = null,
				Dictionary<string, string> parameterAliases = null)
			{
				_factory = factory ?? new ParserFactory();
				Format = format ?? TokenFormat.Shared;
				_parameterAliases = parameterAliases;
				_stringBuilder = new StringBuilder();
				_first = true;
			}

			public TokenFormat Format { get; private set; }

			public void Append(Expression expression, bool isTopLevel = false)
			{
				ExpressionParser parser = _factory.Make(expression, this);
				isTopLevel = isTopLevel || _first;
				_first = false; // only first once
				parser.Parse(isTopLevel);
			}

			public void Append(string value)
			{
				_stringBuilder.Append(value);
			}

			public string ParameterAlias(ParameterExpression expression)
			{
				string alias = null;
				if (_parameterAliases != null)
				{
					_parameterAliases.TryGetValue(expression.Name, out alias);
				}
				return alias ?? expression.Name;
			}

			public string Print()
			{
				return _stringBuilder.ToString();
			}
		}
	}
}
