#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Has;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Has.Enumerable
{
    public interface IPrintableEnumerableHas : IEnumerableHas {}

    public interface IPrintableEnumerableHas<out TResult, TItem> : IPrintableEnumerableHas
        where TResult : class, IEnumerable<TItem> {}

    public interface IPrintableEnumerableHas<out TResult, TItem, TSubject> : IPrintableEnumerableHas<TResult, TItem>,
        IEnumerableHas
            <TResult, TItem, IPrintableSpecification<TSubject, TResult>, IPrintableEvaluation<TResult>,
                IPrintableQuantifiedEnumerableHas<TResult, TItem, TSubject>, TSubject>,
        IPrintableHas<TResult, TSubject>
        where TResult : class, IEnumerable<TItem> {}

    public class PrintableEnumerableHas<TResult, TItem, TSubject> :
        EnumerableHas
            <TResult, TItem, IPrintableSpecification<TSubject, TResult>, IPrintableEvaluation<TResult>,
                IPrintableQuantifiedEnumerableHas<TResult, TItem, TSubject>, TSubject>,
        IPrintableEnumerableHas<TResult, TItem, TSubject>
        where TResult : class, IEnumerable<TItem>
    {
        public PrintableEnumerableHas([NotNull] Lazy<Func<TSubject, TResult>> instrument)
            : base(instrument) {}

        protected override IPrintableQuantifiedEnumerableHas<TResult, TItem, TSubject> MakeAll()
        {
            return new PrintableHasAll<TResult, TItem, TSubject>();
        }
    }
}
