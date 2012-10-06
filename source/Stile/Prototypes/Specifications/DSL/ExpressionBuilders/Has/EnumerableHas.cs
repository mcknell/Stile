#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Has
{
    public interface IEnumerableHas : IHas {}

    public interface IEnumerableHas<out TResult, out TItem> : IEnumerableHas,
        IHas<TResult>
        where TResult : class, IEnumerable<TItem> {}

    public interface IEnumerableHas<out TResult, out TItem, out TSpecifies, out TEvaluation, out TQuantified> :
        IEnumerableHas<TResult, TItem>,
        IHas<TResult, TSpecifies, TEvaluation>
        where TResult : class, IEnumerable<TItem>
        where TSpecifies : class, ISpecification<TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult>
        where TQuantified : class, IQuantifiedEnumerableHas<TResult, TItem, TSpecifies, TEvaluation>
    {
        TQuantified All { get; }
    }

    public interface IEnumerableHas<out TResult, out TItem, out TSpecifies, out TEvaluation, out TQuantified,
        out TSubject> : IEnumerableHas<TResult, TItem, TSpecifies, TEvaluation, TQuantified>,
            IHas<TResult, TSpecifies, TEvaluation, TSubject>
        where TResult : class, IEnumerable<TItem>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult>
        where TQuantified : class, IQuantifiedEnumerableHas<TResult, TItem, TSpecifies, TEvaluation, TSubject> {}

    public abstract class EnumerableHas<TResult, TItem, TSpecifies, TEvaluation, TQuantified, TSubject> :
        Has<TResult, TSubject, TSpecifies, TEvaluation>,
        IEnumerableHas<TResult, TItem, TSpecifies, TEvaluation, TQuantified, TSubject>
        where TResult : class, IEnumerable<TItem>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult>
        where TQuantified : class, IQuantifiedEnumerableHas<TResult, TItem, TSpecifies, TEvaluation, TSubject>
    {
        private readonly Lazy<TQuantified> _lazy;

        protected EnumerableHas([NotNull] Lazy<Func<TSubject, TResult>> instrument)
            : base(instrument)
        {
            _lazy = new Lazy<TQuantified>(MakeAll);
        }

        public TQuantified All
        {
            get
            {
                TQuantified quantified = _lazy.Value;
                return quantified;
            }
        }

        protected abstract TQuantified MakeAll();
    }
}
