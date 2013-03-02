#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
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
