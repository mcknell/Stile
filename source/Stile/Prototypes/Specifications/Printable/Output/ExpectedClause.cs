#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output
{
    public class ExpectedClause<TSubject, TResult> : ExplainerClause<TSubject, TResult>
    {
        public ExpectedClause(string verb, string adjective = null, Func<TResult, string> explainer = null)
            : base(verb, adjective, explainer) {}
    }
}
