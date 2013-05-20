#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
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
