#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Patterns.SelfDescribingPredicates;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.Explainers.Is
{
    public class ExplainFalse<TSubject, TResult> : Explainer<TSubject, TResult>
    {
        public const string False = "false";

        [Rule(Variable.Explainer,
            Items = new object[] {"{0}", Terminal.Be, "'" + False + "'", Variable.Conjunction, Terminal.Was, Variable.ActualValue}
            )]
        public ExplainFalse([Symbol(Variable.Negated)] Negated negated)
            : base(ChooseVerb(negated), False, result => False) {}
    }
}
