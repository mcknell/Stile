#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Sources;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SubjectBuilders
{
    public interface IBoundSubjectBuilder : ISubjectBuilder {}

    public interface IBoundSubjectBuilder<TSubject> : IBoundSubjectBuilder,
        ISubjectBuilder<TSubject> {}

    public interface IBoundSubjectBuilderState : ISubjectBuilderState {}

    public interface IBoundSubjectBuilderState<out TSubject, out TSource> : IBoundSubjectBuilderState,
        ISubjectBuilderState<TSubject>
        where TSource : class, ISource<TSubject>
    {
        TSource Source { get; }
    }

    public abstract class BoundSubjectBuilder<TSubject, TSource> : SubjectBuilder<TSubject>,
        IBoundSubjectBuilder<TSubject>,
        IBoundSubjectBuilderState<TSubject, TSource>
        where TSource : class, ISource<TSubject>
    {
        private readonly TSource _source;

	    protected BoundSubjectBuilder([NotNull] TSource source)
        {
            _source = source.ValidateArgumentIsNotNull();
        }

        public TSource Source
        {
            get { return _source; }
        }
    }

    public class BoundSubjectBuilder<TSubject> : BoundSubjectBuilder<TSubject, ISource<TSubject>>
    {
        public BoundSubjectBuilder([NotNull] ISource<TSubject> source)
            : base(source) {}
    }
}
