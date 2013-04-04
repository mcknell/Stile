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
	public class SkipWithFixture
	{
		[Test]
		public void AllowsEmpty()
		{
			var ints = new int[0];
			Assert.That(ints.SkipWith(x => {}, 2), Is.EquivalentTo(new int[0]));
		}

		[Test]
		public void InvokesAction()
		{
			var ints = new[] {1, 2, 3};
			int calls = 0;
			IEnumerable<int> enumerable = ints.SkipWith(x => calls++);
			Assert.That(enumerable, Is.EquivalentTo(new[] {2, 3}));
		}

		[Test]
		public void RejectsNullAction()
		{
			var ints = new int[0];
			Action<int> action = null;
// ReSharper disable AssignNullToNotNullAttribute
			Assert.Throws<ArgumentNullException>(() => ints.SkipWith(action).Any());
// ReSharper restore AssignNullToNotNullAttribute
		}

		[Test]
		public void RejectsNullSequence()
		{
			int[] ints = null;
// ReSharper disable AssignNullToNotNullAttribute
			Assert.Throws<ArgumentNullException>(() => ints.SkipWith(x => {}).Any());
// ReSharper restore AssignNullToNotNullAttribute
		}

		[Test]
		public void SkipsMoreWhenAsked()
		{
			var ints = new[] {1, 2, 3, 4};
			Assert.That(ints.SkipWith(x => {}, 2), Is.EquivalentTo(new[] {3, 4}));
		}

		[Test]
		public void SkipsOnceByDefault()
		{
			var ints = new[] {1, 2, 3};
			Assert.That(ints.SkipWith(x => {}), Is.EquivalentTo(new[] {2, 3}));
		}
	}
}
