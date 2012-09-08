#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System.Linq.Expressions;
using Stile.Readability;
#endregion

namespace Stile.Types.Expressions.Printing.ExpressionParsers
{
	public class ConstantExpressionParser : ExpressionParser<ConstantExpression>
	{
		public ConstantExpressionParser(ConstantExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		protected override void Parse(ConstantExpression expression)
		{
			if (expression.Type != typeof(string))
			{
				Append(expression.Value.ToDebugString());
			}
			else
			{
				Append(expression.ToString());
			}
		}
	}
}
