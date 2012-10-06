#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Is;
using Stile.Prototypes.Specifications.Printable.Output.Explainers;
using Stile.Prototypes.Specifications.Printable.Output.Explainers.Is;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Is
{
    public static class IsExtensions
    {
        [Pure]
        public static IPrintableSpecification<TSubject> EqualTo<TSubject>(this IPrintableIs<TSubject> builder, TSubject subject)
            where TSubject : IEquatable<TSubject>
        {
            var state = (IIsState) builder;
            Predicate<TSubject> accepter = x => state.Negated.AgreesWith(x.Equals(subject));
            ExplainIs<TSubject, TSubject> explainer = Explain.Subject<TSubject>().Is(subject, state.Negated);
            return Make(accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, TResult> EqualTo<TSubject, TResult>(this IPrintableIs<TResult, TSubject> builder,
            TResult result) where TResult : IEquatable<TResult>
        {
            var state = (IIsState<TSubject, TResult>) builder;
            Predicate<TResult> accepter = x => state.Negated.AgreesWith(x.Equals(result));
            ExplainIs<TSubject, TResult> explainer = Explain.Subject<TSubject>().Is(result, state.Negated);
            return Make(builder, accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, bool> False<TSubject>(this IPrintableIs<bool, TSubject> builder)
        {
            var state = (IIsState<TSubject, bool>) builder;
            Predicate<bool> accepter = x => state.Negated.AgreesWith(!x);
            ExplainFalse<TSubject, bool> explainer = Explain.Subject<TSubject>().False(state.Negated);
            return Make(builder, accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<bool> False(this IPrintableIs<bool> builder)
        {
            var state = (IIsState) builder;
            Predicate<bool> accepter = x => state.Negated.AgreesWith(!x);
            ExplainFalse<bool, bool> explainer = Explain.Subject<bool>().False(state.Negated);
            return Make(accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, TResult> Make<TSubject, TResult>(IPrintableIs<TResult, TSubject> builder,
            Predicate<TResult> accepter,
            Explainer<TSubject, TResult> explainer)
        {
            var state = (IIsState<TSubject, TResult>) builder;
            var specification = new PrintableSpecification<TSubject, TResult>(state.Instrument, accepter, explainer);
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
        public static IPrintableSpecification<TSubject, TResult> Null<TSubject, TResult>(this IPrintableIs<TResult, TSubject> builder)
            where TResult : class
        {
            var state = (IIsState<TSubject, TResult>) builder;
            Predicate<TResult> accepter = x => state.Negated.AgreesWith(x == null);
            ExplainNull<TSubject, TResult> explainer = Explain.Subject<TSubject>().Null<TSubject, TResult>(state.Negated);
            return Make(builder, accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<TSubject> Null<TSubject>(this IPrintableIs<TSubject> builder) where TSubject : class
        {
            var state = (IIsState) builder;
            Predicate<TSubject> accepter = x => state.Negated.AgreesWith(x == null);
            ExplainNull<TSubject, TSubject> explainer = Explain.Subject<TSubject>().Null<TSubject, TSubject>(state.Negated);
            return Make(accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, TResult?> Null<TSubject, TResult>(this IPrintableIs<TResult?, TSubject> builder)
            where TResult : struct
        {
            var state = (IIsState<TSubject, TResult?>) builder;
            Predicate<TResult?> accepter = x => state.Negated.AgreesWith(x == null);
            ExplainNull<TSubject, TResult?> explainer = Explain.Subject<TSubject>().Null<TSubject, TResult?>(state.Negated);
            return Make(builder, accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<TSubject?> Null<TSubject>(this IPrintableIs<TSubject?> builder) where TSubject : struct
        {
            var state = (IIsState) builder;
            Predicate<TSubject?> accepter = x => state.Negated.AgreesWith(x == null);
            ExplainNull<TSubject?, TSubject?> explainer = Explain.Subject<TSubject?>().Null<TSubject?, TSubject?>(state.Negated);
            return Make(accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<TSubject, bool> True<TSubject>(this IPrintableIs<bool, TSubject> builder)
        {
            var state = (IIsState<TSubject, bool>) builder;
            Predicate<bool> accepter = x => state.Negated.AgreesWith(x);
            ExplainTrue<TSubject, bool> explainer = Explain.Subject<TSubject>().True(state.Negated);
            return Make(builder, accepter, explainer);
        }

        [Pure]
        public static IPrintableSpecification<bool> True(this IPrintableIs<bool> builder)
        {
            var state = (IIsState) builder;
            Predicate<bool> accepter = x => state.Negated.AgreesWith(x);
            ExplainTrue<bool, bool> explainer = Explain.Subject<bool>().True(state.Negated);
            return Make(accepter, explainer);
        }
    }
}
