#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Stile.Patterns.Behavioral.Traversal;
#endregion

namespace Stile.Tests.Patterns.Behavioral.Traversal
{
	[TestFixture]
	public class TraverserFixture
	{
		protected HashSet<int> _traversed;

		[SetUp]
		public void Init()
		{
			_traversed = new HashSet<int>();
		}

		[Test]
		public void TraverseFailsForBalkyAccepter()
		{
			// arrange
			var ints = new[] {2, 3, 5};
			var testSubject = new Traverser<int>(ints, BalkyAccepter);

			// act
			bool result = testSubject.Traverse();

			// assert
			Assert.That(result, Is.False);
		}

		[Test]
		public void TraverseSucceedsForAccumulatingAccepter()
		{
			// arrange
			var ints = new[] {2, 3, 5};
			var testSubject = new Traverser<int>(ints, AccumulatingAccepter);
			Assert.That(_traversed, Is.Empty, "precondition");

			// act
			bool result = testSubject.Traverse();

			// assert
			Assert.That(result, Is.True);
			foreach (int i in _traversed)
			{
				Assert.That(_traversed.Contains(i), Is.True);
			}
			Assert.That(_traversed.Count, Is.EqualTo(ints.Length));
		}

		[Test]
		public void TraverseUsesNavigator()
		{
			// arrange
			var ints = new[] {2, 3, 5};
			var testSubject = new Traverser<int>(ints, AccumulatingAccepter, Enumerable.Reverse);

			// act
			bool result = testSubject.Traverse();

			// assert
			Assert.That(result, Is.True);
			Assert.That(_traversed.SequenceEqual(ints.Reverse()));
		}

		protected Traverser.Move AccumulatingAccepter(int i)
		{
			_traversed.Add(i);
			return Traverser.Move.Visit;
		}

		protected Traverser.Move BalkyAccepter(int i)
		{
			if (i % 2 == 0)
			{
				return Traverser.Move.Visit;
			}
			return Traverser.Move.Halt;
		}
	}
}
