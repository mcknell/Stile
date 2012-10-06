#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Has.Enumerable
{
    public class PrintableHasAll<TResult, TItem, TSubject> :
        PrintableQuantifiedEnumerableHas<TResult, TItem, TSubject>
        where TResult : class, IEnumerable<TItem>
    {
        public override IPrintableSpecification<TSubject, TResult> ItemsSatisfying(
            Expression<Func<TItem, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
