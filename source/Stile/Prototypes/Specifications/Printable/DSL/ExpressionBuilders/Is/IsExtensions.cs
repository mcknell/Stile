﻿#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.Printable.Output.Explainers;
using Stile.Prototypes.Specifications.Printable.Output.Explainers.Is;
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
            Predicate<TSubject> accepter = x => state.Negated.AgreesWith(x.Equals(subject));
            ExplainIs<TSubject, TSubject> explainer = Explain.Subject<TSubject>().Is(subject, state.Negated);
            return Make(accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, TResult> EqualTo<TSubject, TResult>(this IIs<TSubject, TResult> builder,
            TResult result) where TResult : IEquatable<TResult>
        {
            var state = (IIsState<TSubject, TResult>) builder;
            Predicate<TResult> accepter = x => state.Negated.AgreesWith(x.Equals(result));
            ExplainIs<TSubject, TResult> explainer = Explain.Subject<TSubject>().Is(result, state.Negated);
            return Make(builder, accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, TResult> Make<TSubject, TResult>(IIs<TSubject, TResult> builder,
            Predicate<TResult> accepter,
            Explainer<TSubject, TResult> explainer)
        {
            var state = (IIsState<TSubject, TResult>) builder;
            var specification = new PrintableSpecification<TSubject, TResult>(state.Extractor, accepter, explainer);
            return specification;
        }

        [Pure]
        public static IPrintableSpecification<TSubject> Make<TSubject>(Predicate<TSubject> accepter,
            Explainer<TSubject, TSubject> explainer)
        {
            var specification = new PrintableSpecification<TSubject>(accepter, explainer);
            return specification;
        }
    }
}