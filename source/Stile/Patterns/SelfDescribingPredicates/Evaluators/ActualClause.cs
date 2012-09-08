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
    public class ActualClause<TValue> : EvaluatorClause<TValue>
    {
        public ActualClause(IVerbTensePair verbTensePair = null, Func<TValue, string> explainer = null)
            : base((verbTensePair ?? VerbTensePair.WouldBeButWas).FollowingBut, explainer) {}
    }
}
