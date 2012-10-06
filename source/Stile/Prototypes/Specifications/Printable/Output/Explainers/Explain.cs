#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.Printable.Output.Explainers.ResultHas;
using Stile.Prototypes.Specifications.Printable.Output.Explainers.ResultIs;
using Stile.Prototypes.Specifications.Printable.Output.Explainers.ResultIs.Comparables;
using Stile.Prototypes.Specifications.Printable.Output.Explainers.ResultIs.Strings;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.Explainers
{
    public static class Explain
    {
        public static ExplainComparablyEquivalentTo<TSubject, TResult> ComparablyEquivalentTo<TSubject, TResult>(
            this FluentSubject<TSubject> subject, TResult expected, Negated negated)
        {
            return new ExplainComparablyEquivalentTo<TSubject, TResult>(negated, expected);
        }

        public static ExplainStringContaining<TSubject> Containing<TSubject>(this FluentSubject<TSubject> subject,
            string expected,
            Negated negated)
        {
            return new ExplainStringContaining<TSubject>(negated, expected);
        }

        public static ExplainStringEndingWith<TSubject> EndingWith<TSubject>(this FluentSubject<TSubject> subject,
            string expected,
            Negated negated)
        {
            return new ExplainStringEndingWith<TSubject>(negated, expected);
        }

        public static ExplainFalse<TSubject, bool> False<TSubject>(this FluentSubject<TSubject> subject, Negated negated)
        {
            return new ExplainFalse<TSubject, bool>(negated);
        }

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

        public static ExplainHashCode<TSubject, TResult> HashCode<TSubject, TResult>(this FluentSubject<TSubject> subject, int hashCode)
        {
            return new ExplainHashCode<TSubject, TResult>(hashCode);
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

        public static ExplainNull<TSubject, TResult> Null<TSubject, TResult>(this FluentSubject<TSubject> subject, Negated negated)
        {
            return new ExplainNull<TSubject, TResult>(negated);
        }

        public static ExplainStringNullOrEmpty<TSubject> NullOrEmpty<TSubject>(this FluentSubject<TSubject> subject,
            Negated negated)
        {
            return new ExplainStringNullOrEmpty<TSubject>(negated);
        }

        public static ExplainStringNullOrWhitespace<TSubject> NullOrWhitespace<TSubject>(this FluentSubject<TSubject> subject,
            Negated negated)
        {
            return new ExplainStringNullOrWhitespace<TSubject>(negated);
        }

        public static ExplainStringStartingWith<TSubject> StartingWith<TSubject>(this FluentSubject<TSubject> subject,
            string expected,
            Negated negated)
        {
            return new ExplainStringStartingWith<TSubject>(negated, expected);
        }

        public static FluentSubject<TSubject> Subject<TSubject>()
        {
            return new FluentSubject<TSubject>();
        }

        public static ExplainTrue<TSubject, bool> True<TSubject>(this FluentSubject<TSubject> subject, Negated negated)
        {
            return new ExplainTrue<TSubject, bool>(negated);
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
