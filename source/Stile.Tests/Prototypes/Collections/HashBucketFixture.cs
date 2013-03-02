#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Collections;
#endregion

namespace Stile.Tests.Prototypes.Collections
{
	[TestFixture]
	public class HashBucketFixture
	{
		[Test]
		public void Add()
		{
			var hashBucket = new HashBucket<int, int>();

			hashBucket.Add(1, 2);
			Assert.That(hashBucket[1], Contains.Item(2));
			Assert.That(hashBucket[1].Count, Is.EqualTo(1));

			hashBucket.Add(1, 3);
			Assert.That(hashBucket[1], Contains.Item(3));
			Assert.That(hashBucket[1].Count, Is.EqualTo(2));

			hashBucket.Add(1, 2);
			Assert.That(hashBucket[1].Count, Is.EqualTo(3));

			hashBucket.Add(5, 7);
			Assert.That(hashBucket[5], Contains.Item(7));
			Assert.That(hashBucket[5].Count, Is.EqualTo(1));

			Assert.That(hashBucket[1].Count, Is.EqualTo(3));
		}
	}
}
