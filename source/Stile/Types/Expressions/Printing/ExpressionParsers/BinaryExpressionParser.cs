#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
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

		public BinaryExpressionParser(BinaryExpression expression, IPrintStrategy printStrategy, BinaryOperatorToken binaryOperatorToken)
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
			if (_isTopLevel)
			{
				return false;
			}
			return expression.NodeType.IsBinaryExpressionAndNotAnAssignment();
		}
	}
}
