#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Patterns.SelfDescribingPredicates;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.Explainers.Is.Strings
{
    public class ExplainStringNullOrWhitespace<TSubject> : Explainer<TSubject, string>
    {
        private const string NullOrEmpty = "null or whitespace";

        [Rule(Variable.Explainer, Items = new object[] {"{0}", Terminal.Be, "'" + NullOrEmpty + "'", //
            Variable.Conjunction, Terminal.Was, Variable.ActualValue})]
        public ExplainStringNullOrWhitespace([Symbol(Variable.Negated)] Negated negated)
            : base(ExpectationVerb.Be.Negate(negated), expected : result => NullOrEmpty) {}
    }
}
