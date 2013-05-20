#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Linq.Expressions;
#endregion

namespace Stile.Types.Expressions.Printing.ExpressionParsers
{
	public class LambdaExpressionParser : ExpressionParser<LambdaExpression>
	{
		public LambdaExpressionParser(LambdaExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		private void AppendLambdaBody(LambdaExpression expression)
		{
			Append(" => ");
			Expression body = expression.Body;
			bool isTopLevel = body.NodeType.IsBinaryExpressionAndNotAnAssignment();
			Append(body, isTopLevel);
		}

		protected override void Parse(LambdaExpression expression)
		{
			int parameterCount = expression.Parameters.Count;
			if (parameterCount == 0)
			{
				Append("()");
			}
			else
			{
				bool addParentheses = parameterCount != 1;
				if (addParentheses)
				{
					Append("(");
				}
				AppendSequence(expression.Parameters);
				if (addParentheses)
				{
					Append(")");
				}
			}
			AppendLambdaBody(expression);
		}
	}
}
