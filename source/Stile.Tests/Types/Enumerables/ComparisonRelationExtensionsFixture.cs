#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Types.Comparison;
#endregion

namespace Stile.Tests.Types.Enumerables
{
	[TestFixture]
	public class ComparisonRelationExtensionsFixture
	{
		[Test]
		public void IsGreaterThan()
		{
			AssertDirectMethod(ComparisonRelationExtensions.IsGreaterThanOrEqualTo,
				ComparisonRelation.GreaterThanOrEqual);
		}

		[Test]
		public void IsGreaterThanOrEqualTo()
		{
			AssertDirectMethod(ComparisonRelationExtensions.IsGreaterThan, ComparisonRelation.GreaterThan);
		}

		[Test]
		public void IsLessThan()
		{
			AssertDirectMethod(ComparisonRelationExtensions.IsLessThan, ComparisonRelation.LessThan);
		}

		[Test]
		public void IsLessThanOrEqualTo()
		{
			AssertDirectMethod(ComparisonRelationExtensions.IsLessThanOrEqualTo, ComparisonRelation.LessThanOrEqual);
		}

		[Test]
		public void PassesFor()
		{
			AssertRelationTo1(ComparisonRelation.Equal, false, true, false);
			AssertRelationTo1(ComparisonRelation.GreaterThan, false, false, true);
			AssertRelationTo1(ComparisonRelation.GreaterThanOrEqual, false, true, true);
			AssertRelationTo1(ComparisonRelation.LessThan, true, false, false);
			AssertRelationTo1(ComparisonRelation.LessThanOrEqual, true, true, false);
		}

		private static void AssertDirectMethod(Func<int, int, bool> method, ComparisonRelation relation)
		{
			for (int i = 0; i < 3; i++)
			{
				Assert.That(method.Invoke(i, 1),
					Is.EqualTo(relation.PassesFor(i, 1)),
					string.Format("{0} {1}", relation, i));
			}
		}

		private static void AssertRelationTo1(Func<int, int, bool> func,
			ComparisonRelation relation,
			params bool[] iterations)
		{
			for (int left = 0; left < iterations.Length; left++)
			{
				Assert.That(func.Invoke(left, 1),
					Is.EqualTo(iterations[left]),
					string.Format("{0} {1} with 1", relation, left));
			}
		}

		private static void AssertRelationTo1(ComparisonRelation relation, params bool[] iterations)
		{
			AssertRelationTo1((i, i1) => relation.PassesFor(i, i1), relation, iterations);
		}
	}
}
