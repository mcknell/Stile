#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.Contracts;
using Stile.Patterns.SelfDescribingPredicates;
using Stile.Prototypes.Specifications.Printable.Output.Explainers;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Is
{
    public static class IsExtensions
    {
        [Pure]
        public static IPrintableSpecification<TSubject> EqualTo<TSubject>(this IIs<TSubject> builder, TSubject subject)
            where TSubject : IEquatable<TSubject>
        {
            var state = (IIsState<TSubject>) builder;
            Negated negated = state.Negated;
            Predicate<TSubject> accepter = x => negated.AgreesWith(x.Equals(subject));
            var explainer = new ExplainBe<TSubject, TSubject>(negated, subject);
            var specification = new PrintableSpecification<TSubject>(accepter, explainer);
            return specification;
        }

        [Pure]
        public static IPrintableSpecification<TSubject, TResult> EqualTo<TSubject, TResult>(this IIs<TSubject, TResult> builder,
            TResult result) where TSubject : IEquatable<TSubject>
        {
            var state = (IIsState<TSubject, TResult>) builder;
            Negated negated = state.Negated;
            Lazy<Func<TSubject, TResult>> extractor = state.Extractor;
            Predicate<TResult> accepter = x => negated.AgreesWith(x.Equals(result));
            var explainer = new ExplainBe<TSubject, TResult>(negated, result);
            var specification = new PrintableSpecification<TSubject, TResult>(extractor, accepter, explainer);
            return specification;
        }
    }
}
