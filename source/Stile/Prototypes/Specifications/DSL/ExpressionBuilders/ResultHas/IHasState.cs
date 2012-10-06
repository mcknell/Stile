#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas
{
    public interface IHasState<TSubject> {}

    public interface IHasState<TSubject, TResult> : IHasState<TSubject>
    {
        [NotNull]
        Lazy<Func<TSubject, TResult>> Instrument { get; }
    }
}
