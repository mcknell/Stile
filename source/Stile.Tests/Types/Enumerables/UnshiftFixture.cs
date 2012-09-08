#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using NUnit.Framework;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Tests.Types.Enumerables
{
	[TestFixture]
	public class UnshiftFixture
	{
		[Test]
		public void AddManyToEmpty()
		{
			var ints = new int[0];
			Assert.That(ints.Unshift(4, 5), Is.EquivalentTo(new[] {4, 5}));
		}

		[Test]
		public void AddManyToNonempty()
		{
			var ints = new[] {3, 4};
			Assert.That(ints.Unshift(1, 2), Is.EquivalentTo(new[] {1, 2, 3, 4}));
		}

		[Test]
		public void AddOneToEmpty()
		{
			var ints = new int[0];
			Assert.That(ints.Unshift(4), Is.EquivalentTo(new[] {4}));
		}
	}
}
