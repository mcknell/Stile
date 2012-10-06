#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Is;
using Stile.Prototypes.Specifications.Printable.Output.Explainers;
using Stile.Prototypes.Specifications.Printable.Output.Explainers.Is.Strings;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Is
{
    public static class IsStringExtensions
    {
        [Pure]
        public static IPrintableSpecification<string> NullOrEmpty(this IPrintableIs<string> builder)
        {
            var state = (IIsState) builder;
            Predicate<string> accepter = x => state.Negated.AgreesWith(string.IsNullOrEmpty(x));
            ExplainStringNullOrEmpty<string> explainer = Explain.Subject<string>().NullOrEmpty<string>(state.Negated);
            return IsExtensions.Make(accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, string> NullOrEmpty<TSubject>(
            this IPrintableIs<string, TSubject> builder)
        {
            var state = (IIsState<TSubject, string>) builder;
            Predicate<string> accepter = x => state.Negated.AgreesWith(string.IsNullOrEmpty(x));
            ExplainStringNullOrEmpty<TSubject> explainer = Explain.Subject<TSubject>().NullOrEmpty(state.Negated);
            return IsExtensions.Make(builder, accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, string> NullOrWhitespace<TSubject>(
            this IPrintableIs<string, TSubject> builder)
        {
            var state = (IIsState<TSubject, string>) builder;
            Predicate<string> accepter = x => state.Negated.AgreesWith(string.IsNullOrWhiteSpace(x));
            ExplainStringNullOrWhitespace<TSubject> explainer =
                Explain.Subject<TSubject>().NullOrWhitespace(state.Negated);
            return IsExtensions.Make(builder, accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<string> NullOrWhitespace(this IPrintableIs<string> builder)
        {
            var state = (IIsState) builder;
            Predicate<string> accepter = x => state.Negated.AgreesWith(string.IsNullOrWhiteSpace(x));
            ExplainStringNullOrWhitespace<string> explainer =
                Explain.Subject<string>().NullOrWhitespace<string>(state.Negated);
            return IsExtensions.Make(accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<string> StringContaining(this IPrintableIs<string> builder,
            string expected)
        {
            var state = (IIsState) builder;
            Predicate<string> accepter = x => state.Negated.AgreesWith(x.Contains(expected));
            ExplainStringContaining<string> explainer = Explain.Subject<string>().Containing<string>(expected,
                state.Negated);
            return IsExtensions.Make(accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, string> StringContaining<TSubject>(
            this IPrintableIs<string, TSubject> builder, string expected)
        {
            var state = (IIsState<TSubject, string>) builder;
            Predicate<string> accepter = x => state.Negated.AgreesWith(x.Contains(expected));
            ExplainStringContaining<TSubject> explainer = Explain.Subject<TSubject>().Containing(expected,
                state.Negated);
            return IsExtensions.Make(builder, accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<string> StringEndingWith(this IPrintableIs<string> builder,
            string expected)
        {
            var state = (IIsState) builder;
            Predicate<string> accepter = x => state.Negated.AgreesWith(x.EndsWith(expected));
            ExplainStringEndingWith<string> explainer = Explain.Subject<string>().EndingWith<string>(expected,
                state.Negated);
            return IsExtensions.Make(accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, string> StringEndingWith<TSubject>(
            this IPrintableIs<string, TSubject> builder, string expected)
        {
            var state = (IIsState<TSubject, string>) builder;
            Predicate<string> accepter = x => state.Negated.AgreesWith(x.EndsWith(expected));
            ExplainStringEndingWith<TSubject> explainer = Explain.Subject<TSubject>().EndingWith(expected,
                state.Negated);
            return IsExtensions.Make(builder, accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<string> StringStartingWith(this IPrintableIs<string> builder,
            string expected)
        {
            var state = (IIsState) builder;
            Predicate<string> accepter = x => state.Negated.AgreesWith(x.StartsWith(expected));
            ExplainStringStartingWith<string> explainer = Explain.Subject<string>().StartingWith<string>(expected,
                state.Negated);
            return IsExtensions.Make(accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, string> StringStartingWith<TSubject>(
            this IPrintableIs<string, TSubject> builder, string expected)
        {
            var state = (IIsState<TSubject, string>) builder;
            Predicate<string> accepter = x => state.Negated.AgreesWith(x.StartsWith(expected));
            ExplainStringStartingWith<TSubject> explainer = Explain.Subject<TSubject>().StartingWith(expected,
                state.Negated);
            return IsExtensions.Make(builder, accepter, explainer);
        }
    }
}
