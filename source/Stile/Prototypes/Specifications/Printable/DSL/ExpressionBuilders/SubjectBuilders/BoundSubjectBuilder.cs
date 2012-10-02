#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders
{
    public interface IBoundSubjectBuilder<TSubject> {}

    public class BoundSubjectBuilder<TSubject> : SubjectBuilder<TSubject>,
        IBoundSubjectBuilder<TSubject>,
        IBoundSubjectBuilderState<TSubject>
    {
        private readonly ISource<TSubject> _source;

        public BoundSubjectBuilder([NotNull] ISource<TSubject> source)
        {
            _source = source.ValidateArgumentIsNotNull();
        }

        public TSubject Get()
        {
            return _source.Get();
        }
    }
}
