#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.Explainers.ResultIs.Comparables
{
    public class ExplainComparablyEquivalentTo<TSubject, TResult> : Explainer<TSubject, TResult>
    {
        public const string ComparablyEquivalentTo = "comparably equivalent to";

        [Rule(Variable.Explainer, Items = new object[]
        {
            "{0}", Terminal.Be, "'" + ComparablyEquivalentTo + "' {1}", //
            Variable.Conjunction, Terminal.Was, Variable.ActualValue
        })]
        public ExplainComparablyEquivalentTo([Symbol(Variable.Negated)] Negated negated,
            [Symbol(Variable.ExpectedValue)] TResult expected)
            : base(ExpectationVerb.Be.Negate(negated), ComparablyEquivalentTo, result => expected.ToDebugString()) {}
    }
}
