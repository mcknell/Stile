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
    public abstract class EvaluatorClause<TValue>
    {
        private readonly Func<TValue, string> _explainer;
        private readonly string _verb;

        protected EvaluatorClause(string verb, Func<TValue, string> explainer = null)
        {
            _verb = verb;
            _explainer = explainer ?? (x => x.ToDebugString());
        }

        public string Explain(TValue actualValue)
        {
            return string.Format("{0}{1}{2}", _verb, Separator, _explainer.Invoke(actualValue));
        }

        protected virtual string Separator
        {
            get { return " "; }
        }
    }
}
