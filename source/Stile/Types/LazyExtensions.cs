#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
#endregion

namespace Stile.Types
{
    public static class LazyExtensions
    {
        public static Lazy<TValue> ToLazy<TValue>(this TValue value)
        {
            return new Lazy<TValue>(() => value);
        }

        public static Lazy<TValue> ToLazy<TValue>(this Func<TValue> func)
        {
            return new Lazy<TValue>(func);
        }
    }
}
