#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Patterns.SelfDescribingPredicates;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Is
{
    public interface IIsState<TSubject>
    {
        Negated Negated { get; }
    }

    public interface IIsState<TSubject, TResult> : IIsState<TSubject>
    {
        Lazy<Func<TSubject, TResult>> Extractor { get; }
    }
}
