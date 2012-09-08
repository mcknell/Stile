#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using System.Linq.Expressions;
#endregion

namespace Stile.Types.Expressions.Printing.ExpressionParsers
{
	public abstract class NewExpressionParser<TExpression> : ExpressionParser<TExpression>
		where TExpression : Expression
	{
		protected NewExpressionParser(TExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		protected void ParseNew(NewExpression expression)
		{
			Type type = expression.Type;
// ReSharper disable ConditionIsAlwaysTrueOrFalse
			if (expression.Constructor != null)
// ReSharper restore ConditionIsAlwaysTrueOrFalse
			{
				type = expression.Constructor.ReflectedType;
			}
			Append(Format.PrefixNew);
			Append(type);
			Append(Format.OpenNew);
			AppendSequence(expression.Arguments);
			Append(Format.CloseNew);
		}
	}

	public class NewExpressionParser : NewExpressionParser<NewExpression>
	{
		public NewExpressionParser(NewExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		protected override void Parse(NewExpression expression)
		{
			ParseNew(expression);
		}
	}
}
