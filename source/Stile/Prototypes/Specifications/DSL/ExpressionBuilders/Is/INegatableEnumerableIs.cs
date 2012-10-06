#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Is
{
    public interface INegatableEnumerableIs : IEnumerableIs,
        INegatableIs {}

    public interface INegatableEnumerableIs<out TResult> : INegatableEnumerableIs,
        IEnumerableIs<TResult>,
        INegatableIs<TResult> {}

    public interface INegatableEnumerableIs<out TResult, out TNegated> : INegatableEnumerableIs<TResult>,
        INegatableIs<TResult, TNegated> {}

    public interface INegatableEnumerableIs<out TResult, out TItem, out TNegated, out TSpecifies, out TEvaluation> :
        INegatableEnumerableIs<TResult, TNegated>,
        IEnumerableIs<TResult, TItem, TSpecifies, TEvaluation>,
        INegatableIs<TResult, TNegated, TSpecifies, TEvaluation>
        where TResult : class, IEnumerable<TItem>
        where TNegated : class, IIs<TResult, TSpecifies, TEvaluation>
        where TSpecifies : class, ISpecification<TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult> {}

    public interface INegatableEnumerableIs<out TResult, out TItem, out TNegated, out TSpecifies, out TEvaluation,
        out TSubject> : INegatableEnumerableIs<TResult, TItem, TNegated, TSpecifies, TEvaluation>,
            IEnumerableIs<TResult, TItem, TSpecifies, TEvaluation, TSubject>,
            INegatableIs<TResult, TNegated, TSpecifies, TEvaluation, TSubject>
        where TResult : class, IEnumerable<TItem>
        where TNegated : class, IIs<TResult, TSubject, TSpecifies, TEvaluation>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult> {}
}
