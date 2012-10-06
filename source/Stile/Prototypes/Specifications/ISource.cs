#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Types.Expressions;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications
{
    public interface ISource
    {
        [NotNull]
        Lazy<string> Description { get; }
    }

    public interface ISource<out TSubject> : ISource
    {
        [CanBeNull]
        TSubject Get();
    }

    public class Source<TSubject> : ISource<TSubject>
    {
        private readonly Lazy<TSubject> _subjectGetter;

        public Source([NotNull] Expression<Func<TSubject>> expression, Lazy<string> description = null)
            : this(expression.Compile, description ?? expression.ToLazyDebugString()) {}

        public Source(TSubject subject, Lazy<string> description = null)
            : this(() => () => subject, description ?? subject.ToLazyDebugString()) {}

        private Source(Func<Func<TSubject>> doubleFunc, [NotNull] Lazy<string> description)
        {
            _subjectGetter = new Lazy<TSubject>(doubleFunc.Invoke());
            Description = description.ValidateArgumentIsNotNull();
        }

        public Lazy<string> Description { get; private set; }

        public TSubject Get()
        {
            return _subjectGetter.Value;
        }
    }
}
