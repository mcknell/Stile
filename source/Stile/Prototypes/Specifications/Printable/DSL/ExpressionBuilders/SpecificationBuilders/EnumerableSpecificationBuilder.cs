#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultHas.Enumerable;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders
{
    public interface IPrintableEnumerableSpecificationBuilder : IPrintableSpecificationBuilder,
        IEnumerableSpecificationBuilder {}

    public interface IPrintableEnumerableSpecificationBuilder<TSubject, out TResult, TItem> :
        IPrintableEnumerableSpecificationBuilder,
        IEnumerableSpecificationBuilder<TSubject, //
            TResult, //
            TItem, //
            IPrintableEnumerableHas<TResult, TItem, TSubject>, //
            IPrintableNegatableEnumerableIs<TSubject, TResult, TItem>, //
            IPrintableEnumerableIs<TSubject, TResult, TItem>, //
            IPrintableSpecification<TSubject, TResult>, //
            IPrintableEvaluation<TResult>, //
            IPrintableQuantifiedEnumerableHas<TResult, TItem, TSubject>>
        where TResult : class, IEnumerable<TItem> {}

    public class PrintableEnumerableSpecificationBuilder<TSubject, TResult, TItem> :
        EnumerableSpecificationBuilder
            <TSubject, TResult, TItem, IPrintableEnumerableHas<TResult, TItem, TSubject>, //
                IPrintableNegatableEnumerableIs<TSubject, TResult, TItem>, //
                IPrintableEnumerableIs<TSubject, TResult, TItem>, //
                IPrintableSpecification<TSubject, TResult>, //
                IPrintableEvaluation<TResult>, //
                IPrintableQuantifiedEnumerableHas<TResult, TItem, TSubject>>,
        IPrintableEnumerableSpecificationBuilder<TSubject, TResult, TItem>
        where TResult : class, IEnumerable<TItem>
    {
        private readonly Lazy<Func<TSubject, TResult>> _instrument;

        public PrintableEnumerableSpecificationBuilder([NotNull] Lazy<Func<TSubject, TResult>> instrument)
        {
            _instrument = instrument.ValidateArgumentIsNotNull();
        }

        protected override IPrintableEnumerableHas<TResult, TItem, TSubject> MakeHas()
        {
            return new PrintableEnumerableHas<TResult, TItem, TSubject>(_instrument);
        }

        protected override IPrintableNegatableEnumerableIs<TSubject, TResult, TItem> MakeIs()
        {
            return new PrintableEnumerableIs<TSubject, TResult, TItem>(Negated.False, _instrument);
        }
    }
}
