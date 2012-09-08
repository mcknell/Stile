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
	public class ConditionalExpressionParser : ExpressionParser<ConditionalExpression>
	{
		public ConditionalExpressionParser(ConditionalExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		protected override void Parse(ConditionalExpression expression)
		{
			Append(expression.Test);
			Append(Format.ConditionalFirst);
			Append(expression.IfTrue);
			Append(Format.ConditionalSecond);
			Append(expression.IfFalse);
		}
	}
}
