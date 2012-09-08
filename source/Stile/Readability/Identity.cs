#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
#endregion

namespace Stile.Readability
{
    public static class Identity
    {
        public const string Format = "{0}";

        public static Func<TItem, TItem> Map<TItem>()
        {
            return x => x;
        }
    }
}
