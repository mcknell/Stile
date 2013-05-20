#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
#endregion

namespace Stile.Types.Equality
{
	public static class ComparableExtensions
	{
		public static bool IsComparablyEqualTo<TComparable>(this TComparable comparable, TComparable other)
			where TComparable : IComparable<TComparable>
		{
			return comparable.CompareTo(other) == 0;
		}

		public static bool IsGreaterThan<TComparable>(this TComparable comparable, TComparable other)
			where TComparable : IComparable<TComparable>
		{
			return comparable.CompareTo(other) > 0;
		}

		public static bool IsGreaterThanOrEqualTo<TComparable>(this TComparable comparable, TComparable other)
			where TComparable : IComparable<TComparable>
		{
			return comparable.CompareTo(other) >= 0;
		}

		public static bool IsLessThan<TComparable>(this TComparable comparable, TComparable other)
			where TComparable : IComparable<TComparable>
		{
			return comparable.CompareTo(other) < 0;
		}

		public static bool IsLessThanOrEqualTo<TComparable>(this TComparable comparable, TComparable other)
			where TComparable : IComparable<TComparable>
		{
			return comparable.CompareTo(other) <= 0;
		}
	}
}
