#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Is;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Is
{
    public interface IPrintableEnumerableIs : IEnumerableIs {}

    public interface IPrintableEnumerableIs<out TResult> : IPrintableEnumerableIs,
        IEnumerableIs<TResult> {}

    public interface IPrintableEnumerableIs<out TResult, out TItem> : IPrintableEnumerableIs<TResult>,
        IEnumerableIs<TResult, TItem, IPrintableResultSpecification<TResult>, IPrintableEvaluation<TResult>>
        where TResult : class, IEnumerable<TItem> {}

    public interface IPrintableEnumerableIs<out TResult, out TItem, TSubject> :
        IPrintableEnumerableIs<TResult, TItem>,
        IEnumerableIs
            <TResult, TItem, IPrintableSpecification<TSubject, TResult>, IPrintableEvaluation<TResult>, TSubject>
        where TResult : class, IEnumerable<TItem> {}

    public class PrintableEnumerableIs<TResult, TItem, TSubject> :
        EnumerableIs
            <TResult, TItem, IPrintableEnumerableIs<TResult, TItem, TSubject>,
                IPrintableSpecification<TSubject, TResult>, IPrintableEvaluation<TResult>, TSubject>,
        IPrintableEnumerableIs<TResult, TItem, TSubject>,
        IPrintableNegatableEnumerableIs<TResult, TItem, TSubject>
        where TResult : class, IEnumerable<TItem>
    {
        public PrintableEnumerableIs(Negated negated, [NotNull] Lazy<Func<TSubject, TResult>> instrument)
            : base(negated, instrument) {}

        protected override IPrintableEnumerableIs<TResult, TItem, TSubject> Factory()
        {
            return new PrintableEnumerableIs<TResult, TItem, TSubject>(Negated.False, Instrument);
        }
    }
}
