#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Patterns.SelfDescribingPredicates;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.Explainers
{
    public static class Explain
    {
        public static ExplainGreaterThan<TSubject, TResult> GreaterThan<TSubject, TResult>(this FluentSubject<TSubject> subject,
            TResult expected,
            Negated negated)
        {
            return new ExplainGreaterThan<TSubject, TResult>(negated, expected);
        }

        public static ExplainGreaterThanOrEqualTo<TSubject, TResult> GreaterThanOrEqualTo<TSubject, TResult>(
            this FluentSubject<TSubject> subject, TResult expected, Negated negated)
        {
            return new ExplainGreaterThanOrEqualTo<TSubject, TResult>(negated, expected);
        }

        public static ExplainLessThan<TSubject, TResult> LessThan<TSubject, TResult>(this FluentSubject<TSubject> subject,
            TResult expected,
            Negated negated)
        {
            return new ExplainLessThan<TSubject, TResult>(negated, expected);
        }

        public static ExplainLessThanOrEqualTo<TSubject, TResult> LessThanOrEqualTo<TSubject, TResult>(
            this FluentSubject<TSubject> subject, TResult expected, Negated negated)
        {
            return new ExplainLessThanOrEqualTo<TSubject, TResult>(negated, expected);
        }

        public static FluentSubject<TSubject> Subject<TSubject>()
        {
            return new FluentSubject<TSubject>();
        }

        public class FluentSubject<TSubject>
        {
            public ExplainIs<TSubject, TResult> Is<TResult>(TResult expected, Negated negated)
            {
                return new ExplainIs<TSubject, TResult>(negated, expected);
            }
        }
    }
}
