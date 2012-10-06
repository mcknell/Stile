#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders
{
    public interface IEnumerableSpecificationBuilder : ISpecificationBuilder {}

/*
    public interface IEnumerableSpecificationBuilder<out TResult, out TItem, out THas, out TNegatableIs, out TIs,
        out TSpecifies, out TEvaluation, out TQuantified> : IEnumerableSpecificationBuilder
        where TResult : class, IEnumerable<TItem>
        where THas : class, IEnumerableHas<TResult, TItem, TSpecifies, TEvaluation, TQuantified>
        where TNegatableIs : class, INegatableEnumerableIs<TResult, TItem, TIs, TSpecifies, TEvaluation>
        where TIs : class, IEnumerableIs<TResult, TItem, TSpecifies, TEvaluation>
        where TSpecifies : class, ISpecification<TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult>
        where TQuantified : class, IQuantifiedEnumerableHas<TResult, TItem, TSpecifies, TEvaluation> {}
*/

    public interface IEnumerableSpecificationBuilder<out TSubject, out TResult, out TItem, out THas, out TNegatableIs,
        out TIs, out TSpecifies, out TEvaluation, out TQuantified> : IEnumerableSpecificationBuilder,
            ISpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies, TEvaluation>
        where TResult : class, IEnumerable<TItem>
        where THas : class, IEnumerableHas<TResult, TItem, TSpecifies, TEvaluation, TQuantified, TSubject>
        where TNegatableIs : class, INegatableEnumerableIs<TSubject, TResult, TItem, TIs, TSpecifies, TEvaluation>
        where TIs : class, IEnumerableIs<TSubject, TResult, TItem, TSpecifies, TEvaluation>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult>
        where TQuantified : class, IQuantifiedEnumerableHas<TResult, TItem, TSpecifies, TEvaluation, TSubject> {}

    public abstract class EnumerableSpecificationBuilder<TSubject, TResult, TItem, THas, TNegatableIs, TIs,
        TSpecifies, TEvaluation, TQuantified> :
            SpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies, TEvaluation>,
            IEnumerableSpecificationBuilder
                <TSubject, TResult, TItem, THas, TNegatableIs, TIs, TSpecifies, TEvaluation, TQuantified>
        where TResult : class, IEnumerable<TItem>
        where THas : class, IEnumerableHas<TResult, TItem, TSpecifies, TEvaluation, TQuantified, TSubject>
        where TNegatableIs : class, INegatableEnumerableIs<TSubject, TResult, TItem, TIs, TSpecifies, TEvaluation>
        where TIs : class, IEnumerableIs<TSubject, TResult, TItem, TSpecifies, TEvaluation>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult>
        where TQuantified : class, IQuantifiedEnumerableHas<TResult, TItem, TSpecifies, TEvaluation, TSubject> {}
}
