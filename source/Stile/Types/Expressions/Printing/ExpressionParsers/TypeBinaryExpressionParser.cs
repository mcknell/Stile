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
	public abstract class TypeBinaryExpressionParser : ExpressionParser<TypeBinaryExpression>
	{
		protected TypeBinaryExpressionParser(TypeBinaryExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		protected abstract string Relation { get; }

		protected override sealed void Parse(TypeBinaryExpression expression)
		{
			Append(expression.Expression);
			Append(Relation);
			Append(expression.TypeOperand);
		}
	}
}
