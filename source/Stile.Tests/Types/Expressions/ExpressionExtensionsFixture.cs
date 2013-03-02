#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using Stile.Testing.SampleTypes;
using Stile.Types.Expressions;
#endregion

namespace Stile.Tests.Types.Expressions
{
	[TestFixture]
	public class ExpressionExtensionsFixture
	{
		[Test]
		public void GetPropertyInfo()
		{
			// arrange
			Expression<Func<Sample, int>> expression = x => x.Int;

			// act
			PropertyInfo propertyInfo = expression.GetPropertyInfo();

			// assert
			Assert.IsNotNull(propertyInfo);
			Assert.That(propertyInfo.Name, Is.EqualTo("Int"));
		}

		[Test]
		public void ToDebugString()
		{
			Expression<Func<int>> expression = () => 1;
			Assert.That(expression.ToDebugString(), Is.EqualTo("() => 1"));

			expression = () => Helper.S.GetHashCode();
			Assert.That(expression.ToDebugString(), Is.EqualTo("() => Helper.S.GetHashCode()"));

			expression = () => string.Empty.GetHashCode();
			Assert.That(expression.ToDebugString(), Is.EqualTo("() => String.Empty.GetHashCode()"));

			expression = () => Helper.GetSum(2, 3);
			Assert.That(expression.ToDebugString(), Is.EqualTo("() => Helper.GetSum(2, 3)"));
		}

		[Test]
		public void ToDebugString_Closures()
		{
			Assert.That(ClosureOnLiteral().ToDebugString(), Is.EqualTo("() => 1"));
			Assert.That(ClosureOnVariable().ToDebugString(), Is.EqualTo("() => i"));
			Assert.That(Capture(1).ToDebugString(), Is.EqualTo("() => item"));
			Assert.That(ClosureOnVariableDottingProperty().ToDebugString(), Is.EqualTo("() => s.Length"));
		}

		[Test]
		[Description("I wish this weren't the expected behavior, but it is.")]
		public void ToDebugString_Constants()
		{
			Expression<Func<int>> expression = () => Helper.Two;
			Assert.That(expression.ToDebugString(), Is.EqualTo("() => 2"));
		}

		[Test]
		public void ToDebugString_ExtensionMethods()
		{
			// arrange
			Expression<Func<Expression, string>> f = x => x.ToDebugString();

			// assert
			string debugString = f.ToDebugString();
			Assert.That(debugString, Is.EqualTo("x => x.ToDebugString()"));
		}

		[Test]
		public void ToDebugString_Lambdas()
		{
			Expression<Func<int, string>> expression = x => x.ToString();
			Assert.That(expression.ToDebugString(), Is.EqualTo("x => x.ToString()"));

			Expression<Func<int, int, string>> lambda = (x, y) => x + y.ToString();
			Assert.That(lambda.ToDebugString(), Is.EqualTo("(x, y) => x + y.ToString()"));

			Expression<Func<int, int, int>> sum = (x, y) => x + y;
			Assert.That(sum.ToDebugString(), Is.EqualTo("(x, y) => x + y"));

			Expression<Func<int, int, int, int>> runningSum = (x, y, z) => x + y + z;
			Assert.That(runningSum.ToDebugString(), Is.EqualTo("(x, y, z) => (x + y) + z"));

			Expression<Func<int, int, string>> lambdaParenthesized = (x, y) => (x + y).ToString();
			Assert.That(lambdaParenthesized.ToDebugString(), Is.EqualTo("(x, y) => (x + y).ToString()"));
		}

		private static Expression<Func<TItem>> Capture<TItem>(TItem item)
		{
			Expression<Func<TItem>> expression = () => item;
			return expression;
		}

		private static Expression<Func<int>> ClosureOnLiteral()
		{
			return () => 1;
		}

		private static Expression<Func<int>> ClosureOnVariable()
		{
			int i = 1;
			i++; // so nobody will change to (and nothing will automatically refactor to or even suggest) a constant
			return () => i;
		}

		private static Expression<Func<int>> ClosureOnVariableDottingProperty()
		{
			string s = string.Empty;
			return () => s.Length;
		}

		private static class Helper
		{
			public const int Two = 2;
			public static readonly string S = string.Empty;

			public static int GetSum(int a, int b)
			{
				return a + b;
			}
		}
	}
}
