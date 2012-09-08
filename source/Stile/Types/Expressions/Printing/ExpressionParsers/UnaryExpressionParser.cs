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
	public class UnaryExpressionParser : ExpressionParser<UnaryExpression>
	{
		private readonly UnaryToken _token;

		public UnaryExpressionParser(UnaryExpression expression, IPrintStrategy printStrategy, UnaryToken token)
			: base(expression, printStrategy)
		{
			_token = token;
		}

		protected override void Parse(UnaryExpression expression)
		{
			Append(_token.Prefix);
			Append(expression.Operand);
			Append(_token.Suffix);
		}
	}
}
