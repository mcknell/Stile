#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
