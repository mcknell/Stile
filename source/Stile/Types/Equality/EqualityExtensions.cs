#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
#endregion

namespace Stile.Types.Equality
{
    public static class EqualityExtensions
    {
        public static bool EqualsOrIsEquallyNull<TItem>(this TItem item, TItem other)
        {
            if (ReferenceEquals(null, item))
            {
                return ReferenceEquals(null, other);
            }
            return item.Equals(other);
        }
    }
}
