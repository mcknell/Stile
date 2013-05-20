#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Linq;
using System.Linq.Expressions;
#endregion

namespace Stile.Types.Expressions.Printing.ExpressionParsers
{
	public class InvocationExpressionParser : ExpressionParser<InvocationExpression>
	{
		public InvocationExpressionParser(InvocationExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		protected override void Parse(InvocationExpression expression)
		{
			Append(expression.Expression);
			Append(Format.OpenInvocation);
			AppendSequence(Format.ItemSeparator, expression.Arguments.ToArray());
			Append(Format.CloseInvocation);
		}
	}
}
