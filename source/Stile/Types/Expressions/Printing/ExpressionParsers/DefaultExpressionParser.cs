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
	public class DefaultExpressionParser : ExpressionParser<DefaultExpression>
	{
		public DefaultExpressionParser(DefaultExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		protected override void Parse(DefaultExpression expression)
		{
			Append("default(");
			Append(expression.Type);
			Append(")");
		}
	}
}
