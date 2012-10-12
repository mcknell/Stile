#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Sources
{
    public interface IEnumerableSource : ISource {}

    public interface IEnumerableSource<out TSubject, out TItem> : IEnumerableSource,
        ISource<TSubject>
        where TSubject : class, IEnumerable<TItem> {}

    public class EnumerableSource<TSubject, TItem> : Source<TSubject>,
        IEnumerableSource<TSubject, TItem>
        where TSubject : class, IEnumerable<TItem>
    {
        public EnumerableSource([NotNull] Expression<Func<TSubject>> expression)
            : this(expression.Compile) {}

        public EnumerableSource(TSubject subject)
            : this(() => () => subject) {}

        public EnumerableSource(Func<Func<TSubject>> doubleFunc)
            : base(doubleFunc) {}
    }
}
