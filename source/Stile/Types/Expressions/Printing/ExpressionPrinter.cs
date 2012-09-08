#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
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

		public Lazy<string> Print(Expression expression)
		{
			return new Lazy<string>(() => LazyPrint(expression));
		}

		private string LazyPrint(Expression expression)
		{
			var session = new Session(_factory, _format);
			session.Append(expression);
			return session.Print();
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

		public class Session : IPrintStrategy
		{
			private readonly ParserFactory _factory;
			private readonly StringBuilder _stringBuilder;
			private bool _first;

			public Session(ParserFactory factory = null, TokenFormat format = null)
			{
				_factory = factory ?? new ParserFactory();
				Format = format ?? TokenFormat.Shared;
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

			public string Print()
			{
				return _stringBuilder.ToString();
			}
		}
	}
}
