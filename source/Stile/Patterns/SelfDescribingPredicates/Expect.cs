#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using System.Linq.Expressions;
using Stile.Patterns.SelfDescribingPredicates.Evaluators;
using Stile.Readability;
using Stile.Types.Equality;
#endregion

namespace Stile.Patterns.SelfDescribingPredicates
{
    public static class Expect<TSubject>
    {
        public static NoOpEvaluator<TSubject> Whatever
        {
            get { return new NoOpEvaluator<TSubject>(); }
        }

        public static Evaluator<TSubject, int> HashCode(int hashCode)
        {
            Func<TSubject, int> extractor = x => ReferenceEquals(null, x) ? -1 : x.GetHashCode();
            var evaluator = new Evaluator<TSubject, int>(new ExpectedClause<int>(hashCode),
                new ActualClause<int>(),
                extractor,
                x => x == hashCode);
            return evaluator;
        }

        public static Evaluator<TSubject> Satisfies(Expression<Func<TSubject, bool>> lambdaPredicate)
        {
            VerbTensePair verbTensePair = VerbTensePair.WouldSatisfyButWas;
            var evaluator =
                new Evaluator<TSubject>(new ExpectedClause<TSubject>(verbTensePair, x => lambdaPredicate.ToDebugString()),
                    new ActualClause<TSubject>(),
                    lambdaPredicate.Compile());
            return evaluator;
        }

        public static Evaluator<TSubject> ToBeEqualTo(TSubject subject, Negated negated)
        {
            VerbTensePair wouldBeButWas = VerbTensePair.WouldBeButWas;

            var evaluator =
                new Evaluator<TSubject>(
                    new ExpectedClause<TSubject>(wouldBeButWas, x => string.Format("{0}{1}", negated, subject.ToDebugString())),
                    new ActualClause<TSubject>(wouldBeButWas, x => x.ToDebugString()),
                    x => (x.EqualsOrIsEquallyNull(subject)) != negated);
            return evaluator;
        }

        public static Evaluator<bool> ToBeFalse(Negated negated)
        {
            bool expected = negated; // if negated, invert the expectation, so expect true; otherwise, false
            var evaluator = new Evaluator<bool>(new ExpectedClause<bool>(false, negated),
                new ActualClause<bool>(),
                x => x == expected);
            return evaluator;
        }

        public static Evaluator<string> ToBeStringContaining(string part, Negated negated)
        {
            return BeString(VerbTensePair.WouldContainButWas, part, negated, (x, y) => x.Contains(y));
        }

        public static Evaluator<string> ToBeStringEndingWith(string end, Negated negated)
        {
            return BeString(VerbTensePair.WouldEndWithButWas, end, negated, (x, y) => x.EndsWith(y));
        }

        public static Evaluator<string> ToBeStringStartingWith(string start, Negated negated)
        {
            return BeString(VerbTensePair.WouldStartWithButWas, start, negated, (x, y) => x.StartsWith(y));
        }

        public static Evaluator<bool> ToBeTrue(Negated negated)
        {
            bool expected = !negated; // if negated, invert the expectation, so expect false; otherwise, true
            var evaluator = new Evaluator<bool>(new ExpectedClause<bool>(true, negated),
                new ActualClause<bool>(),
                x => x == expected);
            return evaluator;
        }

        private static Evaluator<string> BeString(VerbTensePair verbTensePair,
            string target,
            Negated negated,
            Func<string, string, bool> stringPredicate)
        {
            var evaluator = new Evaluator<string>(new ExpectedClause<string>(target, negated, verbTensePair),
                new ActualClause<string>(),
                x => stringPredicate.Invoke(x, target) != negated);
            return evaluator;
        }
    }
}
