#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System.Linq.Expressions;
#endregion

namespace Stile.Types.Expressions.Printing.ExpressionParsers
{
	public class PowerExpressionParser : ExpressionParser<BinaryExpression>
	{
		public PowerExpressionParser(BinaryExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		protected override void Parse(BinaryExpression expression)
		{
			Append("Math.Pow(");
			Append(expression.Left);
			Append(", ");
			Append(expression.Right);
			Append(")");
		}
	}
}
