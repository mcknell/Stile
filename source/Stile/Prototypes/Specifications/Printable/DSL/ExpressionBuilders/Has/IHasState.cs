#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Has
{
    public interface IHasState<TSubject>
    {
        [NotNull]
        Lazy<string> SubjectDescription { get; }
    }

    public interface IHasState<TSubject, TResult> : IHasState<TSubject>
    {
        Lazy<Func<TSubject, TResult>> Extractor { get; }
    }
}
