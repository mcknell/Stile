#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output
{
    public abstract class ExplainerClause<TSubject, TResult>
    {
        private readonly string _adjective;
        private readonly Func<TResult, string> _explainer;
        private readonly string _verb;

        protected ExplainerClause(string verb, string adjective = null, Func<TResult, string> explainer = null)
        {
            _verb = verb;
            _adjective = adjective;
            _explainer = explainer ?? PrintExtensions.ToDebugString;
        }

	    protected virtual string Separator
        {
            get { return " "; }
        }
    }
}
