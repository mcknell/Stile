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
	public class NewArrayInitExpressionParser : ExpressionParser<NewArrayExpression>
	{
		public NewArrayInitExpressionParser(NewArrayExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		protected override void Parse(NewArrayExpression expression)
		{
			Append(Format.PrefixNew);
			Append(expression.Type.GetElementType());
			Append(Format.OpenArrayIndex);
			Append(Format.CloseArrayIndex);
			if (expression.Expressions.Any())
			{
				Append(Format.OpenBlock);
				AppendSequence(expression.Expressions);
				Append(Format.CloseBlock);
			}
		}
	}
}
