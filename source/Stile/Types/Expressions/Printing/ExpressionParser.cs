#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Types.Expressions.Printing.Tokens;
using Stile.Types.Reflection;
#endregion

namespace Stile.Types.Expressions.Printing
{
	public abstract class ExpressionParser
	{
		private readonly IPrintStrategy _printStrategy;

		protected ExpressionParser(IPrintStrategy printStrategy)
		{
			_printStrategy = printStrategy;
		}

		public abstract void Parse(bool isTopLevel = false);

		protected TokenFormat Format
		{
			get { return _printStrategy.Format; }
		}

		protected void Append(string value)
		{
			_printStrategy.Append(value);
		}

		protected void Append(Type type)
		{
			_printStrategy.Append(type.ToDebugString());
		}

		protected void Append(Expression expression, bool isTopLevel = false)
		{
			_printStrategy.Append(expression, isTopLevel);
		}

		protected void AppendCloseParenthesis()
		{
			Append(_printStrategy.Format.CloseParenthesis);
		}

		protected void AppendOpenParenthesis()
		{
			Append(_printStrategy.Format.OpenParenthesis);
		}

		protected void AppendSequence(IEnumerable<Expression> sequence)
		{
			AppendSequence(_printStrategy.Format.ItemSeparator, sequence);
		}

		protected void AppendSequence(string separator, IEnumerable<Expression> sequence)
		{
			if (!sequence.Any())
			{
				return;
			}
			Append(sequence.First());
			foreach (Expression expression in sequence.Skip(1))
			{
				Append(separator);
				Append(expression);
			}
		}

		[CanBeNull]
		protected string GetAlias(ParameterExpression expression)
		{
			return _printStrategy.ParameterAlias(expression);
		}
	}

	public abstract class ExpressionParser<TExpression> : ExpressionParser
		where TExpression : Expression
	{
		private readonly TExpression _expression;

		protected ExpressionParser(TExpression expression, IPrintStrategy printStrategy)
			: base(printStrategy)
		{
			_expression = expression;
		}

		public override void Parse(bool isTopLevel = false)
		{
			IsTopLevel = isTopLevel;
			Parse(_expression);
		}

		protected bool IsTopLevel { get; set; }

		protected abstract void Parse(TExpression expression);
	}
}
