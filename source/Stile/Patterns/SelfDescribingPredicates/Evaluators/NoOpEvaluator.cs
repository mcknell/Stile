#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

namespace Stile.Patterns.SelfDescribingPredicates.Evaluators
{
    public class NoOpEvaluator<TSubject> : Evaluator<TSubject>
    {
        public NoOpEvaluator()
            : base(new ExpectedClause<TSubject>(), new ActualClause<TSubject>(), x => true) {}
    }
}
