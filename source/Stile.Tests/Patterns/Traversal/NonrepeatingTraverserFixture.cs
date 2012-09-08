#region License statement
// Stile.Tests
// Copyright (c) 2010-2012, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using NUnit.Framework;
using Stile.Patterns.Traversal;
#endregion

namespace Stile.Tests.Patterns.Traversal
{
	[TestFixture]
	public class NonrepeatingTraverserFixture : TraverserFixture
	{
		[Test]
		public void DoesNotRepeatNodes()
		{
			// arrange
			var ints = new[] {2, 3, 5, 2};
			var testSubject = new NonrepeatingTraverser<int>(ints, AccumulatingAccepter);

			// act
			bool result = testSubject.Traverse();

			// assert
			Assert.That(result, Is.False);
		}
	}
}
