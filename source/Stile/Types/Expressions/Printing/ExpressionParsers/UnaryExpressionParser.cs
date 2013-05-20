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
