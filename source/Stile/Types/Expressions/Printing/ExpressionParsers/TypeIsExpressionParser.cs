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
	public class TypeIsExpressionParser : TypeBinaryExpressionParser
	{
		public TypeIsExpressionParser(TypeBinaryExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		protected override string Relation
		{
			get { return " is "; }
		}
	}
}
