#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
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
