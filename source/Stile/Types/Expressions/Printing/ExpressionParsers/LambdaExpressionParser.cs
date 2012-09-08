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
