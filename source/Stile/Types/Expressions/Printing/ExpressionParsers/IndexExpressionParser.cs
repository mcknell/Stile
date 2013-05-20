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
