#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Patterns.SelfDescribingPredicates;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.Explainers
{
    public class ExplainBe<TSubject, TResult> : Explainer<TSubject, TResult>
    {
        [Rule(Variable.Explainer)]
        // explainer ::=  'not'? 'be' expected conjunction 'was' actual
        public ExplainBe([Symbol(Variable.Negated, Suffix = Terminal.Be)] Negated negated,
            [Symbol(Variable.ExpectedValue)] TResult expected)
            : base(negated ? ExpectationVerb.NotBe : ExpectationVerb.Be, result => expected.ToDebugString()) {}
    }
}
