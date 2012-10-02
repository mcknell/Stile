#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Patterns.SelfDescribingPredicates;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.Explainers.Is
{
    public class ExplainGreaterThan<TSubject, TResult> : Explainer<TSubject, TResult>
    {
        [Rule(Variable.Explainer,
            Items = new object[] {"{0}", Terminal.Be, "'>' {1}", Variable.Conjunction, Terminal.Was, Variable.ActualValue})]
        public ExplainGreaterThan([Symbol(Variable.Negated)] Negated negated, [Symbol(Variable.ExpectedValue)] TResult expected)
            : base(negated ? ExpectationVerb.NotBe : ExpectationVerb.Be, ">", result => expected.ToDebugString()) {}
    }
}
