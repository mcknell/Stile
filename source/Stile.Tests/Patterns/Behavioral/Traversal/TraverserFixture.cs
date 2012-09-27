#region License statement
// Stile.Tests
// Copyright (c) 2010-2012, Mark Knell
// Published under the MIT License; all other rights reserved
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
		#region Setup/Teardown
		[SetUp]
		public void Init()
		{
			_traversed = new HashSet<int>();
		}
		#endregion

		protected HashSet<int> _traversed;

		protected Traverser.Move BalkyAccepter(int i)
		{
			if (i % 2 == 0)
			{
				return Traverser.Move.Visit;
			}
			return Traverser.Move.Halt;
		}

		protected Traverser.Move AccumulatingAccepter(int i)
		{
			_traversed.Add(i);
			return Traverser.Move.Visit;
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
	}
}
