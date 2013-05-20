#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
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
