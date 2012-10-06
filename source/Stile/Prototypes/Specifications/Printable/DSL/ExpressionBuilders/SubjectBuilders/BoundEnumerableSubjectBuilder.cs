#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders
{
    public interface IBoundEnumerableSubjectBuilder<TSubject, TItem> : IBoundSubjectBuilder<TSubject>,
        IEnumerableSubjectBuilder<TSubject, TItem>
        where TSubject : class, IEnumerable<TItem> {}

    public class BoundEnumerableSubjectBuilder<TSubject, TItem> : BoundSubjectBuilder<TSubject>,
        IBoundEnumerableSubjectBuilder<TSubject, TItem>
        where TSubject : class, IEnumerable<TItem>
    {
        public BoundEnumerableSubjectBuilder([NotNull] ISource<TSubject> source)
            : base(source) {}
    }
}
