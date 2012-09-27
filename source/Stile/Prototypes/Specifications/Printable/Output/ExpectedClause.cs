#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output
{
    public class ExpectedClause<TResult> : DescriptionClause<TResult>
    {
        public ExpectedClause(string verb, Func<TResult, string> explainer = null)
            : base(verb, explainer) {}
    }
}
