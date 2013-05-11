#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Types.Comparison;
#endregion

namespace Stile.Tests.Types.Enumerables
{
	[TestFixture]
	public class ComparisonRelationExtensionsFixture
	{
		[Test]
		public void PassesFor()
		{
			AssertRelationTo1(ComparisonRelation.Equal, false, true, false);
			AssertRelationTo1(ComparisonRelation.GreaterThan, false, false, true);
			AssertRelationTo1(ComparisonRelation.GreaterThanOrEqual, false, true, true);
			AssertRelationTo1(ComparisonRelation.LessThan, true, false, false);
			AssertRelationTo1(ComparisonRelation.LessThanOrEqual, true, true, false);
		}

		private static void AssertRelationTo1(ComparisonRelation relation, params bool[] iterations)
		{
			for (int left = 0; left < iterations.Length; left++)
			{
				Assert.That(relation.PassesFor(left, 1),
					Is.EqualTo(iterations[left]),
					string.Format("{0} {1} with 1", relation, left));
			}
		}
	}
}
