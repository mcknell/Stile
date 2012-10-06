#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.Explainers.Is.Strings
{
    public class ExplainStringNullOrEmpty<TSubject> : Explainer<TSubject, string>
    {
        private const string NullOrEmpty = "null or empty";

        [Rule(Variable.Explainer, Items = new object[] {"{0}", Terminal.Be, "'" + NullOrEmpty + "'", //
            Variable.Conjunction, Terminal.Was, Variable.ActualValue})]
        public ExplainStringNullOrEmpty([Symbol(Variable.Negated)] Negated negated)
            : base(ExpectationVerb.Be.Negate(negated), expected : result => NullOrEmpty) {}
    }
}
