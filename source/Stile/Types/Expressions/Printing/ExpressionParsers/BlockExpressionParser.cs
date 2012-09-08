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
	public class BlockExpressionParser : ExpressionParser<BlockExpression>
	{
		public BlockExpressionParser(BlockExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		protected override void Parse(BlockExpression expression)
		{
			Append(Format.OpenBlock);
			AppendSequence(Format.StatementSeparator, expression.Expressions.ToArray());
			Append(Format.CloseBlock);
		}
	}
}
