#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Linq.Expressions;
#endregion

namespace Stile.Types.Expressions.Printing.ExpressionParsers
{
	public class NewArrayBoundsExpressionParser : ExpressionParser<NewArrayExpression>
	{
		public NewArrayBoundsExpressionParser(NewArrayExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		protected override void Parse(NewArrayExpression expression)
		{
			Append(Format.PrefixNew);
			Append(expression.Type.GetElementType());
			Append(Format.OpenArrayIndex);
			AppendSequence(expression.Expressions);
			Append(Format.CloseArrayIndex);
		}
	}
}
