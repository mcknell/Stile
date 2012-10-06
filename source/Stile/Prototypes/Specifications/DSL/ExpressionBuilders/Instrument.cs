#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders
{
    public static class Instrument
    {
        public static class Trivial<TSubject>
        {
            public static readonly Lazy<Func<TSubject, TSubject>> Map =
                new Lazy<Func<TSubject, TSubject>>(Identity.Map<TSubject>);
        }
    }
}
