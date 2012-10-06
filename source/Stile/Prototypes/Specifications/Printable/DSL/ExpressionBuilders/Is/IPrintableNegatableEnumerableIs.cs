#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Is;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Is
{
    public interface IPrintableNegatableEnumerableIs : INegatableEnumerableIs {}

    public interface IPrintableNegatableEnumerableIs<out TResult> : IPrintableNegatableEnumerableIs,
        INegatableEnumerableIs<TResult> {}

    public interface IPrintableNegatableEnumerableIs<out TResult, out TItem, TSubject> :
        IPrintableNegatableEnumerableIs<TResult>,
        INegatableEnumerableIs
            <TResult, TItem, IPrintableEnumerableIs<TResult, TItem, TSubject>,
                IPrintableSpecification<TSubject, TResult>, IPrintableEvaluation<TResult>, TSubject>
        where TResult : class, IEnumerable<TItem> {}
}
