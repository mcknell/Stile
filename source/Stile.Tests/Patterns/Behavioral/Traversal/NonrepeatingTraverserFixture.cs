#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Patterns.Behavioral.Traversal;
#endregion

namespace Stile.Tests.Patterns.Behavioral.Traversal
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
