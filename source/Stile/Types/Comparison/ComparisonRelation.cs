#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Types.Enums;
#endregion

namespace Stile.Types.Comparison
{
	public enum ComparisonRelation
	{
		LessThan,
		LessThanOrEqual,
		Equal,
		GreaterThanOrEqual,
		GreaterThan
	}

	public static class ComparisonRelationExtensions
	{
		public static bool IsGreaterThan<TItem>(this TItem left, TItem right) where TItem : IComparable<TItem>
		{
			return left.CompareTo(right) > 0;
		}

		public static bool IsGreaterThanOrEqualTo<TItem>(this TItem left, TItem right)
			where TItem : IComparable<TItem>
		{
			return left.CompareTo(right) >= 0;
		}

		public static bool IsLessThan<TItem>(this TItem left, TItem right) where TItem : IComparable<TItem>
		{
			return left.CompareTo(right) < 0;
		}

		public static bool IsLessThanOrEqualTo<TItem>(this TItem left, TItem right) where TItem : IComparable<TItem>
		{
			return left.CompareTo(right) <= 0;
		}

		public static bool PassesFor<TItem>(this ComparisonRelation relation, TItem left, TItem right)
			where TItem : IComparable<TItem>
		{
			switch (relation)
			{
				case ComparisonRelation.LessThan:
					return left.CompareTo(right) < 0;
				case ComparisonRelation.LessThanOrEqual:
					return left.CompareTo(right) <= 0;
				case ComparisonRelation.Equal:
					return left.CompareTo(right) == 0;
				case ComparisonRelation.GreaterThanOrEqual:
					return left.CompareTo(right) >= 0;
				case ComparisonRelation.GreaterThan:
					return left.CompareTo(right) > 0;
			}
			throw Enumeration.FailedToRecognize(() => relation);
		}
	}
}
