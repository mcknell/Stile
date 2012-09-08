#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System.Collections.Generic;
using System.Collections.ObjectModel;
#endregion

namespace Stile.Types.Enumerables
{
    public static class ListExtensions
    {
        public static ReadOnlyCollection<TItem> ToReadOnly<TItem>(this IList<TItem> list)
        {
            return new ReadOnlyCollection<TItem>(list);
        }
    }
}
