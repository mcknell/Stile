#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using Stile.Readability;
#endregion

namespace Stile.Patterns.SelfDescribingPredicates.Evaluators
{
    public class ExpectedClause<TValue> : EvaluatorClause<TValue>
    {
        public ExpectedClause(IVerbTensePair verbTensePair = null, Func<TValue, string> explainer = null)
            : base((verbTensePair ?? VerbTensePair.WouldBeButWas).FollowingWould, explainer) {}

        public ExpectedClause(TValue expected)
            : base(VerbTensePair.WouldBeButWas.FollowingWould, x => expected.ToDebugString()) {}

        public ExpectedClause(TValue expected, Negated negated)
            : this(expected, negated, VerbTensePair.WouldBeButWas) {}

        public ExpectedClause(TValue expected, Negated negated, IVerbTensePair verbTensePair)
            : base(negated + verbTensePair.FollowingWould, x => expected.ToDebugString()) {}
    }
}
