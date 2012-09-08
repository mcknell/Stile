#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using Stile.Types.Expressions.Printing;
using Stile.Types.Expressions.Printing.ExpressionParsers;
#endregion

namespace Stile.Tests.Types.Expressions.Printing.ExpressionParsers
{
	[TestFixture]
	public class MethodCallExpressionParserFixture
	{
		[Test]
		public void Parse()
		{
			// arrange
			MethodCallExpressionParserFixture fixture = this;
			Expression<Func<MethodCallExpressionParserFixture>> f = () => fixture;
			Expression expression = f.Body;
			var currentMethod = (MethodInfo) MethodBase.GetCurrentMethod();
			MethodCallExpression methodCallExpression = Expression.Call(expression, currentMethod);

			var printStrategy = new ExpressionPrinter.Session();
			var expressionParser = new MethodCallExpressionParser(methodCallExpression, printStrategy);

			// act
			expressionParser.Parse();

			// assert
			Assert.That(printStrategy.Print(), Is.EqualTo("fixture.Parse()"));
		}
	}
}
