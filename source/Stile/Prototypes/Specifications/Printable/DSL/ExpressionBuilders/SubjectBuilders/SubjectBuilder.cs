#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders
{
    public interface ISubjectBuilder<TSubject>
    {
        ISubjectBuilder<TSubject> DescribedBy([NotNull] string description);
        ISubjectBuilder<TSubject> DescribedBy(Lazy<string> description);
        ISubjectBuilder<TSubject> DescribedBy([NotNull] Func<string> description);
    }

    public class SubjectBuilder<TSubject> : ISubjectBuilder<TSubject>,
        ISubjectBuilderState<TSubject>
    {
// ReSharper disable StaticFieldInGenericType
// ReSharper disable InconsistentNaming
        private static readonly Lazy<string> sSubjectDescription = typeof(TSubject).ToLazyDebugString();
// ReSharper restore InconsistentNaming
// ReSharper restore StaticFieldInGenericType

        public SubjectBuilder()
            : this(sSubjectDescription) {}

        public SubjectBuilder([NotNull] Lazy<string> subjectDescription)
        {
            SubjectDescription = subjectDescription.ValidateArgumentIsNotNull();
        }

        public Lazy<string> SubjectDescription { get; private set; }

        public ISubjectBuilder<TSubject> DescribedBy(string description)
        {
            description.ValidateArgumentIsNotNull();
            return DescribedBy(new Lazy<string>(() => description));
        }

        public ISubjectBuilder<TSubject> DescribedBy(Lazy<string> description)
        {
            return new SubjectBuilder<TSubject>(description);
        }

        public ISubjectBuilder<TSubject> DescribedBy(Func<string> description)
        {
            description.ValidateArgumentIsNotNull();
            return DescribedBy(new Lazy<string>(description));
        }
    }
}
