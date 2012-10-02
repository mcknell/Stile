#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.Contracts;
using Stile.Patterns.SelfDescribingPredicates;
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
        public static IPrintableSpecification<TSubject> GreaterThan<TSubject>(this IIs<TSubject> builder, TSubject result)
            where TSubject : IComparable<TSubject>
        {
            return MakeComparable(builder, x => x > 0, result, Explain.GreaterThan);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, TResult> GreaterThan<TSubject, TResult>(
            this IIs<TSubject, TResult> builder, TResult result) where TResult : IComparable<TResult>
        {
            return MakeComparable(builder, x => x > 0, result, Explain.GreaterThan);
        }

        [Pure]
        public static IPrintableSpecification<TSubject> GreaterThanOrEqualTo<TSubject>(this IIs<TSubject> builder, TSubject result)
            where TSubject : IComparable<TSubject>
        {
            return MakeComparable(builder, x => x >= 0, result, Explain.GreaterThanOrEqualTo);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, TResult> GreaterThanOrEqualTo<TSubject, TResult>(
            this IIs<TSubject, TResult> builder, TResult result) where TResult : IComparable<TResult>
        {
            return MakeComparable(builder, x => x >= 0, result, Explain.GreaterThanOrEqualTo);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, TResult> LessThan<TSubject, TResult>(this IIs<TSubject, TResult> builder,
            TResult result) where TResult : IComparable<TResult>
        {
            return MakeComparable(builder, x => x < 0, result, Explain.LessThan);
        }

        [Pure]
        public static IPrintableSpecification<TSubject> LessThan<TSubject>(this IIs<TSubject> builder, TSubject result)
            where TSubject : IComparable<TSubject>
        {
            return MakeComparable(builder, x => x < 0, result, Explain.LessThan);
        }

        [Pure]
        public static IPrintableSpecification<TSubject> LessThanOrEqualTo<TSubject>(this IIs<TSubject> builder, TSubject result)
            where TSubject : IComparable<TSubject>
        {
            return MakeComparable(builder, x => x <= 0, result, Explain.LessThanOrEqualTo);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, TResult> LessThanOrEqualTo<TSubject, TResult>(
            this IIs<TSubject, TResult> builder, TResult result) where TResult : IComparable<TResult>
        {
            return MakeComparable(builder, x => x <= 0, result, Explain.LessThanOrEqualTo);
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

        [Pure]
        public static IPrintableSpecification<TSubject, TResult> MakeComparable<TSubject, TResult>(IIs<TSubject, TResult> builder,
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
        public static IPrintableSpecification<TSubject> MakeComparable<TSubject>(IIs<TSubject> builder,
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
