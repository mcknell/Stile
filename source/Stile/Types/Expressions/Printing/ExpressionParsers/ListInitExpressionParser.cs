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
