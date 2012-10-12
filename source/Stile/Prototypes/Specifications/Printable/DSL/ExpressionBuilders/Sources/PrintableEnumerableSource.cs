#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Sources;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Sources
{
    public interface IPrintableEnumerableSource : IPrintableSource,
        IEnumerableSource {}

    public interface IPrintableEnumerableSource<out TSubject, out TItem> : IPrintableEnumerableSource,
        IPrintableSource<TSubject>,
        IEnumerableSource<TSubject, TItem>
        where TSubject : class, IEnumerable<TItem> {}

    public class PrintableEnumerableSource<TSubject, TItem> : PrintableSource<TSubject>,
        IPrintableEnumerableSource<TSubject, TItem>
        where TSubject : class, IEnumerable<TItem>
    {
        public PrintableEnumerableSource(IPrintableSource<TSubject> source)
            : this(() => source.Get, source.Description) {}

        protected PrintableEnumerableSource(Func<Func<TSubject>> doubleFunc, [NotNull] Lazy<string> description)
            : base(doubleFunc, description) {}
    }
}
