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
	public class TypeAsExpressionParser : ExpressionParser<UnaryExpression>
	{
		public TypeAsExpressionParser(UnaryExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		protected override void Parse(UnaryExpression expression)
		{
			Append(expression.Operand);
			Append(" as ");
			Append(expression.Type);
		}
	}
}
