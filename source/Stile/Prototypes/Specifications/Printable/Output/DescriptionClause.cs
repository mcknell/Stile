#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Propter project on GitHub
#endregion

#region using...
using System;
using Stile.Prototypes.Specifications.Evaluations;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output
{
    public abstract class DescriptionClause<TResult>
    {
        private readonly Func<TResult, string> _explainer;
        private readonly string _verb;

        protected DescriptionClause(string verb, Func<TResult, string> explainer = null)
        {
            _verb = verb;
            _explainer = explainer ?? PrintExtensions.ToDebugString;
        }

        public string Explain(IWrappedResult<TResult> wrappedResult)
        {
            string explanation = _explainer.Invoke(wrappedResult.Value);
            return string.Join(Separator, _verb, explanation);
        }

        protected virtual string Separator
        {
            get { return " "; }
        }
    }
}
