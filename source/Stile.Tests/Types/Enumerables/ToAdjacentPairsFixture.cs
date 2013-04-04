#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Tests.Types.Enumerables
{
	[TestFixture]
	public class ToAdjacentPairsFixture
	{
		[Test]
		public void AllowsEmpty()
		{
			var ints = new int[0];
			Assert.That(ints.ToAdjacentPairs(x => {}), Is.EquivalentTo(new int[0]));
		}

		[Test]
		public void RejectsNullSequence()
		{
			int[] ints = null;
// ReSharper disable AssignNullToNotNullAttribute
			Assert.Throws<ArgumentNullException>(() => ints.ToAdjacentPairs(x => {}).Any());
// ReSharper restore AssignNullToNotNullAttribute
		}

		[Test]
		public void SelectsPairs()
		{
			var ints = new[] {1, 2, 3};
			int first = 0;
			List<Tuple<int, int>> list = ints.ToAdjacentPairs(x => first = x).ToList();
			Assert.That(first, Is.EqualTo(1));
			Assert.That(list, Is.EquivalentTo(new[] {Tuple.Create(1, 2), Tuple.Create(2, 3)}));
		}
	}
}
