#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;
using Stile.Readability;
using Stile.Types.Reflection;
#endregion

namespace Stile.Tests.Readability
{
	[TestFixture]
	public class PrintExtensionsFixture
	{
		[Test]
		public void ArrayToDebugString()
		{
			// arrange
			var ints = new[] {5, 3, 4};
			// act
			string s = ints.ToDebugString();

			// assert
			Assert.That(s, Is.EqualTo("int[3] {5, 3, 4}"));
			Assert.That(new int[0].ToDebugString(), Is.EqualTo("int[0]"));
		}

		[Test]
		public void ExpressionToDebugString()
		{
			// arrange
			Expression<Func<int>> f = () => 5;

			// act & assert
			Assert.That(f.ToDebugString(), Is.EqualTo("() => 5"));
		}

		[Test]
		public void ListToDebugString()
		{
			Assert.That(new List<int?> {1, null, -4}.ToDebugString(), Is.EqualTo("List<int?> {1, <null>, -4}"));
		}

		[Test]
		public void NullToDebugString()
		{
			// arrange
			object obj = null;

			// assert
			Assert.That(obj.ToDebugString(), Is.EqualTo(PrintExtensions.ReadableNull));
		}

		[Test]
		public void Pluralize()
		{
			Assert.That(0.Pluralize("shoe"), Is.EqualTo("shoes"));
			Assert.That(1.Pluralize("shoe"), Is.EqualTo("shoe"));
			Assert.That(2.Pluralize("shoe"), Is.EqualTo("shoes"));
			Assert.That(0.Pluralize("child", "children"), Is.EqualTo("children"));
			Assert.That(1.Pluralize("child"), Is.EqualTo("child"));
			Assert.That(2.Pluralize("child", "children"), Is.EqualTo("children"));
		}

		[Test]
		public void StringToDebugString()
		{
			Assert.That(Null.String.ToDebugString(), Is.EqualTo(PrintExtensions.ReadableNullString));
			Assert.That(string.Empty.ToDebugString(), Is.EqualTo("\"\""));
			Assert.That("hello world".ToDebugString(), Is.EqualTo("\"hello world\""));
			Assert.That("hello\tworld".ToDebugString(), Is.EqualTo("\"hello\\tworld\""));
			Assert.That("hello\nworld".ToDebugString(), Is.EqualTo("\"hello\\nworld\""));
			Assert.That("hello\rworld".ToDebugString(), Is.EqualTo("\"hello\\rworld\""));
		}

		[Test]
		public void ToLazyDebugString()
		{
			Assert.That(6.ToLazyDebugString().IsValueCreated, Is.False);

			var lazy = new Lazy<int>(() => 7);
			Assert.That(lazy.ToLazyDebugString().IsValueCreated, Is.False);
			Assert.That(lazy.IsValueCreated, Is.False);
		}

		[Test]
		public void ToLazyDebugStringWhenNull()
		{
			Lazy<int> lazy = null;
			Lazy<string> lazyDebugString = lazy.ToLazyDebugString();
			Assert.That(lazyDebugString.IsValueCreated, Is.False);
			Assert.That(lazyDebugString.Value, Is.EqualTo(PrintExtensions.ReadableNullString));
		}

		[Test]
		public void TypeToDebugString()
		{
			// arrange
			Type type = typeof(Dictionary<int?, IList<Stack<int>>>);

			// act
			string expected = type.ToDebugString();

			// assert
			Assert.That(PrintExtensions.ToDebugString(type), Is.EqualTo(expected));
		}
	}
}
