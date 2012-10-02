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
    public static class IsComparableExtensions
    {
        [Pure]
        public static IPrintableSpecification<TSubject> ComparablyEquivalentTo<TSubject>(this IIs<TSubject> builder, TSubject result)
            where TSubject : IComparable<TSubject>
        {
            return Make(builder, x => x == 0, result, Explain.ComparablyEquivalentTo);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, TResult> ComparablyEquivalentTo<TSubject, TResult>(
            this IIs<TSubject, TResult> builder, TResult result) where TResult : IComparable<TResult>
        {
            return Make(builder, x => x == 0, result, Explain.ComparablyEquivalentTo);
        }

        [Pure]
        public static IPrintableSpecification<TSubject> GreaterThan<TSubject>(this IIs<TSubject> builder, TSubject result)
            where TSubject : IComparable<TSubject>
        {
            return Make(builder, x => x > 0, result, Explain.GreaterThan);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, TResult> GreaterThan<TSubject, TResult>(
            this IIs<TSubject, TResult> builder, TResult result) where TResult : IComparable<TResult>
        {
            return Make(builder, x => x > 0, result, Explain.GreaterThan);
        }

        [Pure]
        public static IPrintableSpecification<TSubject> GreaterThanOrEqualTo<TSubject>(this IIs<TSubject> builder, TSubject result)
            where TSubject : IComparable<TSubject>
        {
            return Make(builder, x => x >= 0, result, Explain.GreaterThanOrEqualTo);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, TResult> GreaterThanOrEqualTo<TSubject, TResult>(
            this IIs<TSubject, TResult> builder, TResult result) where TResult : IComparable<TResult>
        {
            return Make(builder, x => x >= 0, result, Explain.GreaterThanOrEqualTo);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, TResult> LessThan<TSubject, TResult>(this IIs<TSubject, TResult> builder,
            TResult result) where TResult : IComparable<TResult>
        {
            return Make(builder, x => x < 0, result, Explain.LessThan);
        }

        [Pure]
        public static IPrintableSpecification<TSubject> LessThan<TSubject>(this IIs<TSubject> builder, TSubject result)
            where TSubject : IComparable<TSubject>
        {
            return Make(builder, x => x < 0, result, Explain.LessThan);
        }

        [Pure]
        public static IPrintableSpecification<TSubject> LessThanOrEqualTo<TSubject>(this IIs<TSubject> builder, TSubject result)
            where TSubject : IComparable<TSubject>
        {
            return Make(builder, x => x <= 0, result, Explain.LessThanOrEqualTo);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, TResult> LessThanOrEqualTo<TSubject, TResult>(
            this IIs<TSubject, TResult> builder, TResult result) where TResult : IComparable<TResult>
        {
            return Make(builder, x => x <= 0, result, Explain.LessThanOrEqualTo);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, TResult> Make<TSubject, TResult>(IIs<TSubject, TResult> builder,
            Predicate<int> predicate,
            TResult result,
            Func<Explain.FluentSubject<TSubject>, TResult, Negated, Explainer<TSubject, TResult>> explainerFactory)
            where TResult : IComparable<TResult>
        {
            var state = (IIsState<TSubject, TResult>) builder;
            Predicate<TResult> accepter = x => state.Negated.AgreesWith(predicate.Invoke(x.CompareTo(result)));
            Explainer<TSubject, TResult> explainer = explainerFactory.Invoke(Explain.Subject<TSubject>(), result, state.Negated);
            var specification = new PrintableSpecification<TSubject, TResult>(state.Extractor, accepter, explainer);
            return specification;
        }

        [Pure]
        public static IPrintableSpecification<TSubject> Make<TSubject>(IIs<TSubject> builder,
            Predicate<int> predicate,
            TSubject result,
            Func<Explain.FluentSubject<TSubject>, TSubject, Negated, Explainer<TSubject, TSubject>> explainerFactory)
            where TSubject : IComparable<TSubject>
        {
            var state = (IIsState<TSubject>) builder;
            Predicate<TSubject> accepter = x => state.Negated.AgreesWith(predicate.Invoke(x.CompareTo(result)));
            Explainer<TSubject, TSubject> explainer = explainerFactory.Invoke(Explain.Subject<TSubject>(), result, state.Negated);
            var specification = new PrintableSpecification<TSubject>(accepter, explainer);
            return specification;
        }
    }
}