#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas
{
    public interface IQuantifiedEnumerableHas {}

    public interface IQuantifiedEnumerableHas<out TResult, TItem, out TSpecifies, out TEvaluation> :
        IQuantifiedEnumerableHas {}

    public interface IQuantifiedEnumerableHas<out TResult, TItem, out TSpecifies, out TEvaluation, TSubject> :
        IQuantifiedEnumerableHas<TResult, TItem, TSpecifies, TEvaluation>
        where TResult : class, IEnumerable<TItem>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult>
    {
        [Pure]
        TSpecifies ItemsSatisfying(Expression<Func<TItem, bool>> expression);
    }

    public abstract class QuantifiedEnumerableHas<TResult, TItem, TSpecifies, TEvaluation, TSubject> :
        IQuantifiedEnumerableHas<TResult, TItem, TSpecifies, TEvaluation, TSubject>
        where TResult : class, IEnumerable<TItem>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult>
    {
        public abstract TSpecifies ItemsSatisfying(Expression<Func<TItem, bool>> expression);
    }
}
