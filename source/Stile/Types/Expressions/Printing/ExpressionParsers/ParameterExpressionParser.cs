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
	public class ParameterExpressionParser : ExpressionParser<ParameterExpression>
	{
		public ParameterExpressionParser(ParameterExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		protected override void Parse(ParameterExpression expression)
		{
			Append(GetAlias(expression) ?? expression.Name);
		}
	}
}
