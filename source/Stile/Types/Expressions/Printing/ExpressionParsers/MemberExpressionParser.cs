#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System.Linq.Expressions;
using Stile.Types.Reflection;
#endregion

namespace Stile.Types.Expressions.Printing.ExpressionParsers
{
	public class MemberExpressionParser : ExpressionParser<MemberExpression>
	{
		public MemberExpressionParser(MemberExpression memberExpression, IPrintStrategy stringBuilder)
			: base(memberExpression, stringBuilder) {}

		protected override void Parse(MemberExpression expression)
		{
			bool suppressName;
			var constantExpression = expression.Expression as ConstantExpression;
			if (constantExpression != null)
			{
				if (constantExpression.Type.IsCapturingClosure())
				{
					suppressName = true;
				}
				else
				{
					suppressName = constantExpression.Type.IsOrDerivesFrom(expression.Member.ReflectedType);
				}
			}
			else
			{
				suppressName = false;
			}
			if (!suppressName)
			{
				if (expression.MemberIsStatic())
				{
					Append(expression.Member.ReflectedType.Name);
				}
				else
				{
					Append(expression.Expression);
				}
				Append(".");
			}
			Append(expression.Member.Name);
		}
	}
}
