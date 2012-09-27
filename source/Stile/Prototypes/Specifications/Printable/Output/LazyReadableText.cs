#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output
{
    public interface ILazyReadableText
    {
        Lazy<string> Retrieved { get; }
    }

    public class LazyReadableText : ILazyReadableText
    {
        private readonly Lazy<string> _lazyExplanation;

        public LazyReadableText([NotNull] Func<string> explainer)
            : this(new Lazy<string>(explainer)) {}

        public LazyReadableText([NotNull] Lazy<string> lazyExplanation)
        {
            _lazyExplanation = lazyExplanation.ValidateArgumentIsNotNull();
        }

        public Lazy<string> Retrieved
        {
            get { return _lazyExplanation; }
        }

        public override string ToString()
        {
            return _lazyExplanation.IsValueCreated ? _lazyExplanation.Value : "Not built.";
        }
    }
}
