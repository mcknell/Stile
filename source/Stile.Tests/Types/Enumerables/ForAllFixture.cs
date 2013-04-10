#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Tests.Types.Enumerables
{
	[TestFixture]
	public class ForAllFixture
	{
		[Test]
		public void WalksList()
		{
			var list = new List<List<int>>
			{
				new List<int> {1, 2, 3, 4, 5},
				new List<int> {5, 3, 1},
				new List<int> {2, 4, 6}
			};
			Assert.That(list.ForAll(), Is.EquivalentTo(new[] {new[] {1, 5, 2}, new[] {2, 3, 4}, new[] {3, 1, 6}}));
		}

		[Test]
		public void WalksThree()
		{
			var first = new[] {1, 2, 3, 4, 5, 6};
			var odds = new[] {1, 3, 5};
			var evens = new[] {2, 4, 6};
			var arrays = new[] {first, odds, evens};

			// act
			List<IList<int>> forAll = first.ForAll(odds, evens).ToList();

			Assert.That(forAll, Has.Count.EqualTo(3));
			for (int i = 0; i < forAll.Count; i++)
			{
				Assert.That(forAll[i], Is.EquivalentTo(arrays.Select(x => x[i])), "iteration " + i);
			}
		}

		[Test]
		public void WalksTwo()
		{
			var odds = new[] {1, 3, 5};
			var evens = new[] {2, 4, 6};
			var arrays = new[] {odds, evens};

			// act
			List<IList<int>> forAll = odds.ForAll(evens).ToList();

			Assert.That(forAll, Has.Count.EqualTo(3));
			for (int i = 0; i < forAll.Count; i++)
			{
				Assert.That(forAll[i], Is.EquivalentTo(arrays.Select(x => x[i])), "iteration " + i);
			}
		}
	}
}
