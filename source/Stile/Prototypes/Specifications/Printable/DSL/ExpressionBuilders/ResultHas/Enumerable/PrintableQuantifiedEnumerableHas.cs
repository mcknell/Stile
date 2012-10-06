#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultHas.Enumerable
{
    public interface IPrintableQuantifiedEnumerableHas<out TResult, TItem, TSubject> :
        IQuantifiedEnumerableHas
            <TResult, TItem, IPrintableSpecification<TSubject, TResult>, IPrintableEvaluation<TResult>, TSubject>
        where TResult : class, IEnumerable<TItem> {}

    public abstract class PrintableQuantifiedEnumerableHas<TResult, TItem, TSubject> :
        QuantifiedEnumerableHas
            <TResult, TItem, IPrintableSpecification<TSubject, TResult>, IPrintableEvaluation<TResult>, TSubject>,
        IPrintableQuantifiedEnumerableHas<TResult, TItem, TSubject>
        where TResult : class, IEnumerable<TItem> {}
}
