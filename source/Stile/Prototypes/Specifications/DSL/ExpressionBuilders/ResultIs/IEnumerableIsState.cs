#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs
{
    public interface IEnumerableIsState : IIsState {}

    public interface IEnumerableIsState<out TResult, out TItem> : IEnumerableIsState
        where TResult : class, IEnumerable<TItem> {}

    public interface IEnumerableIsState<TSubject, TResult, out TItem> : IEnumerableIsState<TResult, TItem>,
        IIsState<TSubject, TResult>
        where TResult : class, IEnumerable<TItem> {}
}
