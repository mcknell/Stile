#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Linq;
using System.Linq.Expressions;
using Stile.Types.Reflection;
#endregion

namespace Stile.Types.Expressions.Printing.ExpressionParsers
{
	public class MethodCallExpressionParser : ExpressionParser<MethodCallExpression>
	{
		public MethodCallExpressionParser(MethodCallExpression expression, IPrintStrategy printStrategy)
			: base(expression, printStrategy) {}

		protected override void Parse(MethodCallExpression expression)
		{
			if (expression.Object == null)
			{
				if (expression.Method.IsExtensionMethod())
				{
					Append(expression.Arguments.First());
					Append(".");
					Append(expression.Method.Name);
					Append("(");
					AppendSequence(expression.Arguments.Skip(1));
					Append(")");
					return;
				}
				Append(expression.Method.ReflectedType.Name);
			}
			else
			{
				Append(expression.Object);
			}
			Append(".");
			Append(expression.Method.Name);
			Append("(");
			if (expression.Arguments.Any())
			{
				AppendSequence(expression.Arguments);
			}
			Append(")");
		}
	}
}
