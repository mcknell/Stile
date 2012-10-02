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
    public class ExplainTrue<TSubject, TResult> : Explainer<TSubject, TResult>
    {
        public const string True = "true";

        [Rule(Variable.Explainer,
            Items = new object[] {"{0}", Terminal.Be, "'" + True + "'", Variable.Conjunction, Terminal.Was, Variable.ActualValue})
        ]
        public ExplainTrue([Symbol(Variable.Negated)] Negated negated)
            : base(ChooseVerb(negated), True, result => True) {}
    }
}