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

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Is
{
    public interface IEnumerableIs : IIs {}

    public interface IEnumerableIs<out TResult> : IEnumerableIs,
        IIs<TResult> {}

    public interface IEnumerableIs<out TResult, out TItem, out TSpecifies, out TEvaluation> : IEnumerableIs<TResult>,
        IIs<TResult, TSpecifies, TEvaluation>
        where TResult : class, IEnumerable<TItem>
        where TSpecifies : class, ISpecification<TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult> {}

    public interface IEnumerableIs<out TResult, out TItem, out TSpecifies, out TEvaluation, out TSubject> :
        IEnumerableIs<TResult, TItem, TSpecifies, TEvaluation>,
        IIs<TResult, TSubject, TSpecifies, TEvaluation>
        where TResult : class, IEnumerable<TItem>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult> {}

    public abstract class EnumerableIs<TResult, TItem, TNegated, TSpecifies, TEvaluation, TSubject> :
        Is<TResult, TSubject, TNegated, TSpecifies, TEvaluation>,
        IEnumerableIs<TResult, TItem, TSpecifies, TEvaluation, TSubject>,
        IEnumerableIsState<TResult, TItem, TSubject>
        where TResult : class, IEnumerable<TItem>
        where TNegated : class, IIs<TResult, TSubject, TSpecifies, TEvaluation>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult>
    {
        protected EnumerableIs(Negated negated, [NotNull] Lazy<Func<TSubject, TResult>> extractor)
            : base(negated, extractor) {}
    }
}
