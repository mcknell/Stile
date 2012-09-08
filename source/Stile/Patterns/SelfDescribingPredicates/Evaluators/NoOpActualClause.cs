#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

namespace Stile.Patterns.SelfDescribingPredicates.Evaluators
{
    public class NoOpActualClause<TValue> : ActualClause<TValue>
    {
        public NoOpActualClause()
            : base(VerbTensePair.NoOp, x => string.Empty) {}

        protected override string Separator
        {
            get { return string.Empty; }
        }
    }
}
