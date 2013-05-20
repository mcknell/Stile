#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
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
