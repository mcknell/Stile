#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
#endregion

namespace Stile.Patterns.SelfDescribingPredicates.Evaluators
{
    public class SimpleEvaluator<TSubject> : Evaluator<TSubject>
    {
        public SimpleEvaluator(TSubject expected, Func<TSubject, bool> valuePredicate)
            : this(expected, valuePredicate, Negated.False, VerbTensePair.WouldBeButWas) {}

        public SimpleEvaluator(TSubject expected,
            Func<TSubject, bool> valuePredicate,
            Negated negated,
            IVerbTensePair verbTensePair)
            : base(
                new ExpectedClause<TSubject>(expected, negated, verbTensePair),
                new ActualClause<TSubject>(verbTensePair),
                subject => valuePredicate.Invoke(subject) != negated) {}
    }
}
