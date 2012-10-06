#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Is
{
    public interface IIsState
    {
        Negated Negated { get; }
    }

    public interface IIsState<TSubject, TResult> : IIsState
    {
        Lazy<Func<TSubject, TResult>> Instrument { get; }
    }
}
