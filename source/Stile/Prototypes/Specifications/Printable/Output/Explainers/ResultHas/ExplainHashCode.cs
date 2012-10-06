#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Globalization;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.Explainers.ResultHas
{
    public class ExplainHashCode<TSubject, TResult> : Explainer<TSubject, TResult>
    {
        public const string Hashcode = "hashCode";

        [Rule(Variable.Explainer, Items = new object[] {Terminal.Have, "'" + Hashcode + "' {0}", //
            Variable.Conjunction, Terminal.Had, "'" + Hashcode + "'", Variable.ActualValue})]
        public ExplainHashCode([Symbol(Variable.ExpectedValue)] int hashCode)
            : base(ExpectationVerb.Have, Hashcode, result => hashCode.ToString(CultureInfo.InvariantCulture)) {}
    }
}
