#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.Printable.Output.Explainers;
using Stile.Prototypes.Specifications.Printable.Output.Explainers.Has;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Has
{
    public static class HasExtensions
    {
        [Pure]
        public static IPrintableSpecification<TSubject, int> HashCode<TSubject>(this IHas<TSubject> has, int hashCode)
        {
            var extractor = new Lazy<Func<TSubject, int>>(() => x => x.GetHashCode());
            Predicate<int> predicate = x => x == hashCode;
            ExplainHashCode<TSubject> explainer = Explain.Subject<TSubject>().HashCode<TSubject, int>(hashCode);
            return new PrintableSpecification<TSubject, int>(extractor, predicate, explainer);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, int> HashCode<TSubject, TResult>(this IHas<TSubject, TResult> has,
            int hashCode)
        {
            var state = (IHasState<TSubject, TResult>) has;
            var extractor = new Lazy<Func<TSubject, int>>(() => x => state.Extractor.Value.Invoke(x).GetHashCode());
            Predicate<int> predicate = x => x == hashCode;
            ExplainHashCode<TSubject> explainer = Explain.Subject<TSubject>().HashCode<TSubject, int>(hashCode);
            return new PrintableSpecification<TSubject, int>(extractor, predicate, explainer, state.SubjectDescription);
        }
    }
}
