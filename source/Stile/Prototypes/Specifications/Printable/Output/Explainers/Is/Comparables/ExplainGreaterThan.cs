#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.Explainers.Is.Comparables
{
    public class ExplainGreaterThan<TSubject, TResult> : Explainer<TSubject, TResult>
    {
        public const string Operator = ">";

        [Rule(Variable.Explainer,
            Items =
                new object[]
                {"{0}", Terminal.Be, "'" + Operator + "' {1}", Variable.Conjunction, Terminal.Was, Variable.ActualValue})]
        public ExplainGreaterThan([Symbol(Variable.Negated)] Negated negated, [Symbol(Variable.ExpectedValue)] TResult expected)
            : base(ExpectationVerb.Be.Negate(negated), Operator, result => expected.ToDebugString()) {}
    }
}
