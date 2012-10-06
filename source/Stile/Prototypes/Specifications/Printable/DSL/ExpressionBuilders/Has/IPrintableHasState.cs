#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Has;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Has
{
    public interface IPrintableHasState<TSubject> : IHasState<TSubject>
    {
        [NotNull]
        Lazy<string> SubjectDescription { get; }
    }

    public interface IPrintableHasState<TSubject, TResult> : IPrintableHasState<TSubject>,
        IHasState<TSubject, TResult> {}
}
