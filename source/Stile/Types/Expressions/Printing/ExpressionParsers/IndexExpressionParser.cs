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
	public abstract class IndexExpressionParser<TExpression> : ExpressionParser<TExpression>
		where TExpression : Expression
	{
		protected IndexExpressionParser(TExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		protected void AppendIndex(Expression left, params Expression[] right)
		{
			Append(left);
			Append(Format.OpenArrayIndex);
			AppendSequence(right);
			Append(Format.CloseArrayIndex);
		}
	}

	public class IndexExpressionParser : IndexExpressionParser<IndexExpression>
	{
		public IndexExpressionParser(IndexExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		protected override void Parse(IndexExpression expression)
		{
			AppendIndex(expression.Object, expression.Arguments.ToArray());
		}
	}
}
