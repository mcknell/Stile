#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs
{
    public interface IPrintableNegatableEnumerableIs : INegatableEnumerableIs {}

    public interface IPrintableNegatableEnumerableIs<out TResult> : IPrintableNegatableEnumerableIs,
        INegatableEnumerableIs<TResult> {}

    public interface IPrintableNegatableEnumerableIs<TSubject, out TResult, out TItem> :
        IPrintableNegatableEnumerableIs<TResult>,
        INegatableEnumerableIs
            <TSubject, TResult, TItem, IPrintableEnumerableIs<TSubject, TResult, TItem>,
                IPrintableSpecification<TSubject, TResult>, IPrintableEvaluation<TResult>>
        where TResult : class, IEnumerable<TItem> {}
}
