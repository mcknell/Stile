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
			const int firstBucket = 1;
			var hashBucket = new HashBucket<int, int>();

			hashBucket.Add(firstBucket, 2);
			Assert.That(hashBucket[firstBucket], Contains.Item(2));
			Assert.That(hashBucket[firstBucket].Count, Is.EqualTo(1));

			hashBucket.Add(firstBucket, 3);
			Assert.That(hashBucket[firstBucket], Contains.Item(3));
			Assert.That(hashBucket[firstBucket].Count, Is.EqualTo(2));

			Assert.That(hashBucket.ContainsKey(firstBucket), "precondition");
			hashBucket.Add(firstBucket, 2);
			Assert.That(hashBucket[firstBucket].Count,
				Is.EqualTo(2),
				"Shouldn't add an item to the bucket more than once");

			const int secondBucket = 5;
			hashBucket.Add(secondBucket, 7);
			Assert.That(hashBucket[secondBucket], Contains.Item(7));
			Assert.That(hashBucket[secondBucket].Count, Is.EqualTo(1));

			Assert.That(hashBucket[firstBucket].Count,
				Is.EqualTo(2),
				string.Format("Adding to bucket {0} shouldn't affect bucket {1}", secondBucket, firstBucket));
		}
	}
}
