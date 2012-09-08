#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
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
