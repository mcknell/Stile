#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Patterns.SelfDescribingPredicates;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.Explainers.Is.Strings
{
    public class ExplainStringContaining<TSubject> : Explainer<TSubject, string>
    {
        [Rule(Variable.Explainer, Items = new object[]
        {
            "{0}", Terminal.Contain, "{1}", //
            Variable.Conjunction, Terminal.Contained, Variable.ActualValue
        })]
        public ExplainStringContaining([Symbol(Variable.Negated)] Negated negated,
            [Symbol(Variable.ExpectedValue)] string expected)
            : base(ExpectationVerb.Contain.Negate(negated), expected : result => expected.ToDebugString()) {}
    }
}
