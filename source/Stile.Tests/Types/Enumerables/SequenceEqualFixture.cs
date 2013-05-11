#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Linq;
using NUnit.Framework;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Tests.Types.Enumerables
{
	[TestFixture]
	public class SequenceEqualFixture
	{
		[Test]
		public void Three()
		{
			var first = new[] {1, 2, 3};
			var second = new[] {1, 2};
			var third = new[] {1, 2, 4};
			var altThird = new[] {2, 1};

			int firstDifference = first.SequenceEquals(second, third);

			Assert.That(firstDifference, Is.EqualTo(2));
			Assert.That(first.SequenceEquals(second, altThird), Is.EqualTo(0));
		}

		[Test]
		public void Two()
		{
			var first = new[] {1, 2, 3};
			var second = new[] {1, 2};

			int firstDifference = first.SequenceEquals(second);

			Assert.That(firstDifference, Is.EqualTo(2));
			Assert.That(first.SequenceEquals(first), Is.EqualTo(-1));
			Assert.That(first.SequenceEquals(second.Reverse()), Is.EqualTo(0));
		}

		[Test]
		public void Zero()
		{
			var first = new int[0];
			var second = new[] {1};

			int firstDifference = first.SequenceEquals(second);

			Assert.That(firstDifference, Is.EqualTo(0));
			Assert.That(first.SequenceEquals(first), Is.EqualTo(-1));
		}
	}
}
