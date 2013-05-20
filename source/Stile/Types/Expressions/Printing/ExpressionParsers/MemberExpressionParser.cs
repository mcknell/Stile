#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
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
