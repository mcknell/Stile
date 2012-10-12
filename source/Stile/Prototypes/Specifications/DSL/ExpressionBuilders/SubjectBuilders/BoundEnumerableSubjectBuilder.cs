#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Sources;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SubjectBuilders
{
    public interface IBoundEnumerableSubjectBuilder : IBoundSubjectBuilder,
        IEnumerableSubjectBuilder {}

    public interface IBoundEnumerableSubjectBuilder<TSubject, TItem> : IBoundEnumerableSubjectBuilder,
        IBoundSubjectBuilder<TSubject>,
        IEnumerableSubjectBuilder<TSubject, TItem>
        where TSubject : class, IEnumerable<TItem> {}

    public abstract class BoundEnumerableSubjectBuilder<TSubject, TItem, TSource> :
        BoundSubjectBuilder<TSubject, TSource>,
        IBoundEnumerableSubjectBuilder<TSubject, TItem>
        where TSubject : class, IEnumerable<TItem>
        where TSource : class, IEnumerableSource<TSubject, TItem>
    {
        protected BoundEnumerableSubjectBuilder([NotNull] TSource source)
            : base(source) {}
    }

    public class BoundEnumerableSubjectBuilder<TSubject, TItem> :
        BoundEnumerableSubjectBuilder<TSubject, TItem, IEnumerableSource<TSubject, TItem>>
        where TSubject : class, IEnumerable<TItem>
    {
        public BoundEnumerableSubjectBuilder(ISource<TSubject> source)
            : this(new EnumerableSource<TSubject, TItem>(() => source.Get)) {}

        public BoundEnumerableSubjectBuilder([NotNull] IEnumerableSource<TSubject, TItem> source)
            : base(source) {}
    }

    public interface IBoundEnumerableSubjectBuilderState : IEnumerableSubjectBuilderState,
        IBoundSubjectBuilderState {}

    public interface IBoundEnumerableSubjectBuilderState<out TSubject, out TItem, out TSource> :
        IBoundEnumerableSubjectBuilderState,
        IEnumerableSubjectBuilderState<TSubject, TItem>,
        IBoundSubjectBuilderState<TSubject, TSource>
        where TSubject : class, IEnumerable<TItem>
        where TSource : class, ISource<TSubject> {}
}
