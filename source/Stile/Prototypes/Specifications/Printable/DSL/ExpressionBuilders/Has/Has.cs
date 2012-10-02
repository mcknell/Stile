#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Has
{
    public interface IHas<out TSubject> {}

    public interface IHas<out TSubject, out TResult> : IHas<TResult> {}

    public class Has<TSubject> : IHas<TSubject>,
        IHasState<TSubject>
    {
        public Has([NotNull] Lazy<string> subjectDescription)
        {
            SubjectDescription = subjectDescription.ValidateArgumentIsNotNull();
        }

        public Lazy<string> SubjectDescription { get; private set; }
    }

    public class Has<TSubject, TResult> : Has<TResult>,
        IHas<TSubject, TResult>,
        IHasState<TSubject, TResult>
    {
        public Has([NotNull] Lazy<Func<TSubject, TResult>> extractor, [NotNull] Lazy<string> subjectDescription)
            : base(subjectDescription)
        {
            Extractor = extractor.ValidateArgumentIsNotNull();
        }

        public Lazy<Func<TSubject, TResult>> Extractor { get; private set; }
    }
}
