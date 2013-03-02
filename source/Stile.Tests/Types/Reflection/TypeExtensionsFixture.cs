#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;
using Stile.Types.Reflection;
#endregion

namespace Stile.Tests.Types.Reflection
{
	[TestFixture]
	public class TypeExtensionsFixture
	{
		private int _field = -5;

		[Test]
		public void Implements()
		{
			Assert.That(typeof(int).Implements<IComparable>(), Is.True);
			Assert.That(typeof(int).Implements<IList>(), Is.False);
			Assert.That(typeof(int[]).Implements<IEnumerable>(), Is.True);
			Assert.That(typeof(int[]).Implements<IEnumerable<int>>(), Is.True);
			Assert.That(typeof(int[]).Implements<IEnumerable<string>>(), Is.False);
			Assert.That(typeof(int[]).Implements(typeof(IEnumerable<>)), Is.True);

			Assert.Throws<ArgumentOutOfRangeException>(() => typeof(int).Implements(typeof(int)));
		}

		[Test]
		public void IsCapturingClosure()
		{
			/* We only really have to worry about local variables and value parameters. From the C#4 spec:
             * 7.15.5 Outer variables
             * Any local variable, value parameter, or parameter array whose scope includes the lambda-expression 
             * or anonymous-method-expression is called an outer variable of the anonymous function. In an instance 
             * function member of a class, the this value is considered a value parameter and is an outer variable 
             * of any anonymous function contained within the function member.
             * 7.15.5.1 Captured outer variables
             * When an outer variable is referenced by an anonymous function, the outer variable is said to have been 
             * captured by the anonymous function. Ordinarily, the lifetime of a local variable is limited to 
             * execution of the block or statement with which it is associated (§5.1.7). However, the lifetime of a 
             * captured outer variable is extended at least until the delegate or expression tree created from the 
             * anonymous function becomes eligible for garbage collection.
             */
			Assert.That(typeof(int).IsCapturingClosure(), Is.False);
			Assert.That(typeof(List<>).IsCapturingClosure(), Is.False);

			int i = 2;
			Expression<Func<int>> closure = () => i;
			Assert.That(closure.GetType().IsCapturingClosure(), Is.False);

			closure = () => 3;
			Assert.That(closure.GetType().IsCapturingClosure(), Is.False);

			_field++;
				// so nobody will change to (and nothing will automatically refactor to or even suggest) a constant
			closure = () => _field;
			Assert.That(closure.GetType().IsCapturingClosure(), Is.False);

			AssertTypeAndBodyTypeAreNotClosures(GetClosureOverConstant());
			AssertTypeAndBodyTypeAreNotClosures(GetClosureOverLiteral());
			AssertTypeAndBodyTypeAreNotClosures(LocallyNestedHelper.GetClosureOverStaticField());
			AssertTypeAndBodyTypeAreNotClosures(TypeExtensionsFixtureHelper.GetClosureOverStaticField());
			AssertTypeAndBodyTypeAreNotClosuresButBodyExpressionTypeIs(GetClosureOverFieldFromSameClass(), false);
			AssertTypeAndBodyTypeAreNotClosuresButBodyExpressionTypeIs(
				new LocallyNestedHelper().GetClosureOverMemberField(), false);
			AssertTypeAndBodyTypeAreNotClosuresButBodyExpressionTypeIs(
				new TypeExtensionsFixtureHelper().GetClosureOverMemberField(), false);

			AssertTypeAndBodyTypeAreNotClosuresButBodyExpressionTypeIs(GetClosureOverVariable(), true);
			AssertTypeAndBodyTypeAreNotClosuresButBodyExpressionTypeIs(GetClosureOverParameter(4), true);
		}

		[Test]
		public void IsNullable()
		{
			Assert.That(typeof(int).IsNullable(), Is.False);
			Assert.That(typeof(string).IsNullable(), Is.False);
			Assert.That(GetType().IsNullable(), Is.False);
			Assert.That(typeof(Nullable<>).IsNullable(), Is.False);
			Assert.That(typeof(int?).IsNullable(), Is.True);
		}

		private static void AssertTypeAndBodyTypeAreNotClosures(Expression<Func<int>> expression)
		{
			Assert.That(expression.GetType().IsCapturingClosure(), Is.False);
			Assert.That(expression.Body.GetType().IsCapturingClosure(), Is.False);
		}

		private static void AssertTypeAndBodyTypeAreNotClosuresButBodyExpressionTypeIs(
			Expression<Func<int>> expression, bool expected)
		{
			AssertTypeAndBodyTypeAreNotClosures(expression);
			Assert.That(((MemberExpression) expression.Body).Expression.Type.IsCapturingClosure(), Is.EqualTo(expected));
		}

		private static Expression<Func<int>> GetClosureOverConstant()
		{
			const int i = 3;
			return () => i;
		}

		private Expression<Func<int>> GetClosureOverFieldFromSameClass()
		{
			return () => _field;
		}

		private static Expression<Func<int>> GetClosureOverLiteral()
		{
			return () => 3;
		}

		private static Expression<Func<int>> GetClosureOverParameter(int i)
		{
			return () => i;
		}

		private static Expression<Func<int>> GetClosureOverVariable()
		{
			int j = 4;
			j++; // so nobody will change to (and nothing will automatically refactor to or even suggest) a constant
			return () => j;
		}

		public class Base : IBase {}

		public class Derived : Base {}

		public interface IBase {}

		public class ListGeneric<T> : List<T> {}

		public class ListInt : List<int> {}

		public class LocallyNestedHelper
		{
			private static int _sStaticFieldFromOtherClass = 4;
			private int _memberFieldFromOtherClass = 3;

			public Expression<Func<int>> GetClosureOverMemberField()
			{
				_memberFieldFromOtherClass++;
				// so nobody will change to (and nothing will automatically refactor to or even suggest) a constant
				return () => _memberFieldFromOtherClass;
			}

			public static Expression<Func<int>> GetClosureOverStaticField()
			{
				_sStaticFieldFromOtherClass++;
				// so nobody will change to (and nothing will automatically refactor to or even suggest) a constant
				return () => _sStaticFieldFromOtherClass;
			}
		}
	}

	public class TypeExtensionsFixtureHelper
	{
		private static int _sStaticFieldFromOtherClass = 4;
		private int _memberFieldFromOtherClass = 3;

		public Expression<Func<int>> GetClosureOverMemberField()
		{
			_memberFieldFromOtherClass++;
			// so nobody will change to (and nothing will automatically refactor to or even suggest) a constant
			return () => _memberFieldFromOtherClass;
		}

		public static Expression<Func<int>> GetClosureOverStaticField()
		{
			_sStaticFieldFromOtherClass++;
			// so nobody will change to (and nothing will automatically refactor to or even suggest) a constant
			return () => _sStaticFieldFromOtherClass;
		}
	}
}
