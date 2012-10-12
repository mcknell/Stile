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
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas
{
    public interface IQuantifiedEnumerableHas {}

    public interface IQuantifiedEnumerableHas<out TResult, TItem, out TSpecifies> : IQuantifiedEnumerableHas
        where TResult : class, IEnumerable<TItem>
        where TSpecifies : class, ISpecification
    {
        [Pure]
        TSpecifies ItemsSatisfying(Expression<Func<TItem, bool>> expression);
    }

    public abstract class QuantifiedEnumerableHas<TResult, TItem, TSpecifies> :
        IQuantifiedEnumerableHas<TResult, TItem, TSpecifies>
        where TResult : class, IEnumerable<TItem>
        where TSpecifies : class, ISpecification
    {
        public abstract TSpecifies ItemsSatisfying(Expression<Func<TItem, bool>> expression);
    }
}
