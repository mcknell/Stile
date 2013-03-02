#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates.Is
{
    public interface IEnumerableIs<out TSpecification, TSubject, out TResult, TItem> :
        IIs<TSpecification, TSubject, TResult>
        where TResult : class, IEnumerable<TItem>
        where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
    {
        [System.Diagnostics.Contracts.Pure]
        TSpecification Empty { get; }
    }

    public interface INegatableEnumerableIs<out TSpecification, TSubject, out TResult, out TNegated, TItem> :
        IEnumerableIs<TSpecification, TSubject, TResult, TItem>,
        INegatable<TNegated>
        where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
        where TNegated : class, IEnumerableIs<TSpecification, TSubject, TResult, TItem>
        where TResult : class, IEnumerable<TItem> {}

    public class EnumerableIs<TSpecification, TSubject, TResult, TItem> : Is<TSpecification, TSubject, TResult>,
        INegatableEnumerableIs
            <TSpecification, TSubject, TResult, IEnumerableIs<TSpecification, TSubject, TResult, TItem>, TItem>
        where TSpecification : class, IChainableSpecification, ISpecification<TSubject, TResult>
        where TResult : class, IEnumerable<TItem>
    {
        private readonly Lazy<TSpecification> _lazyEmpty;
        public EnumerableIs([NotNull] IInstrument<TSubject, TResult> instrument,
            Negated negated,
            [NotNull] Func<ICriterion<TResult>, TSpecification> specificationFactory,
            ISource<TSubject> source = null)
            : base(instrument, negated, specificationFactory, source)
        {
            _lazyEmpty =
                new Lazy<TSpecification>(
                    () =>
                        Make(
                            new Criterion<TResult>(
                                x => Negated.AgreesWith(x.None()) ? Outcome.Succeeded : Outcome.Failed)));
        }

        public new IEnumerableIs<TSpecification, TSubject, TResult, TItem> Not
        {
            get
            {
                return new EnumerableIs<TSpecification, TSubject, TResult, TItem>(Instrument,
                    Negated.True,
                    SpecificationFactory,
                    Source);
            }
        }
        public TSpecification Empty
        {
            get
            {
                TSpecification specification = _lazyEmpty.Value;
                return specification;
            }
        }
    }
}
