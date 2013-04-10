#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Tests.Types.Enumerables
{
	[TestFixture]
	public class InterlaceFixture
	{
		[Test]
		public void WalksOne()
		{
			var odds = new[] {1, 3, 5, 7};

			Assert.That(odds.Interlace(2), Is.EquivalentTo(new[] {1, 2, 3, 2, 5, 2, 7}));
		}
	}
}
