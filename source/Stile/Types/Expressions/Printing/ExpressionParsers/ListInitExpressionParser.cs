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
	public class ListInitExpressionParser : ExpressionParser<ListInitExpression>
	{
		public ListInitExpressionParser(ListInitExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		private void AppendElementInit(ElementInit expression)
		{
			Append(expression.AddMethod.Name);
			Append("(");
			AppendSequence(expression.Arguments);
			Append(")");
		}

		protected override void Parse(ListInitExpression expression)
		{
			Append(expression.NewExpression);
			Append(" {");
			if (expression.Initializers.Any())
			{
				AppendElementInit(expression.Initializers.First());
				foreach (ElementInit elementInit in expression.Initializers.Skip(1))
				{
					Append(", ");
					AppendElementInit(elementInit);
				}
			}
			Append("}");
		}
	}
}
