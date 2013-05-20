#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Linq.Expressions;
using Stile.Types.Expressions.Printing.Tokens;
#endregion

namespace Stile.Types.Expressions.Printing.ExpressionParsers
{
	public class BinaryExpressionParser : ExpressionParser<BinaryExpression>
	{
		private readonly BinaryOperatorToken _binaryOperatorToken;

		public BinaryExpressionParser(BinaryExpression expression,
			IPrintStrategy printStrategy,
			BinaryOperatorToken binaryOperatorToken)
			: base(expression, printStrategy)
		{
			_binaryOperatorToken = binaryOperatorToken;
		}

		protected override void Parse(BinaryExpression expression)
		{
			bool useParenthesis = ShouldUseParenthesis(expression);
			if (useParenthesis)
			{
				AppendOpenParenthesis();
			}
			Append(expression.Left);
			Append(_binaryOperatorToken.Value);
			Append(expression.Right);
			if (useParenthesis)
			{
				AppendCloseParenthesis();
			}
		}

		private bool ShouldUseParenthesis(BinaryExpression expression)
		{
			if (IsTopLevel)
			{
				return false;
			}
			return expression.NodeType.IsBinaryExpressionAndNotAnAssignment();
		}
	}
}
