#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs
{
    public interface IPrintableEnumerableIs : IEnumerableIs {}

    public interface IPrintableEnumerableIs<out TResult> : IPrintableEnumerableIs,
        IEnumerableIs<TResult> {}

    public interface IPrintableEnumerableIs<out TResult, out TItem> : IPrintableEnumerableIs<TResult>,
        IEnumerableIs<TResult, TItem, IPrintableResultSpecification<TResult>, IPrintableEvaluation<TResult>>
        where TResult : class, IEnumerable<TItem> {}

    public interface IPrintableEnumerableIs<TSubject, out TResult, out TItem> :
        IPrintableEnumerableIs<TResult, TItem>,
        IEnumerableIs
            <TSubject, TResult, TItem, IPrintableSpecification<TSubject, TResult>, IPrintableEvaluation<TResult>>
        where TResult : class, IEnumerable<TItem> {}

    public class PrintableEnumerableIs<TSubject, TResult, TItem> :
        EnumerableIs
            <TSubject, TResult, TItem,
                IPrintableEnumerableIs<TSubject, TResult, TItem>, IPrintableSpecification<TSubject, TResult>, IPrintableEvaluation<TResult>>,
        IPrintableEnumerableIs<TSubject, TResult, TItem>,
        IPrintableNegatableEnumerableIs<TSubject, TResult, TItem>
        where TResult : class, IEnumerable<TItem>
    {
        public PrintableEnumerableIs(Negated negated, [NotNull] Lazy<Func<TSubject, TResult>> instrument)
            : base(negated, instrument) {}

        protected override IPrintableEnumerableIs<TSubject, TResult, TItem> Factory()
        {
            return new PrintableEnumerableIs<TSubject, TResult, TItem>(Negated.False, Instrument);
        }
    }
}
