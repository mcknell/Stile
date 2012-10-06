#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.Explainers.ResultIs.Strings
{
    public class ExplainStringEndingWith<TSubject> : Explainer<TSubject, string>
    {
        [Rule(Variable.Explainer, Items = new object[]
        {
            "{0}", Terminal.EndWith, "{1}", //
            Variable.Conjunction, Terminal.EndedWith, Variable.ActualValue
        })]
        public ExplainStringEndingWith([Symbol(Variable.Negated)] Negated negated,
            [Symbol(Variable.ExpectedValue)] string expected)
            : base(ExpectationVerb.EndWith.Negate(negated), expected : result => expected.ToDebugString()) {}
    }
}
