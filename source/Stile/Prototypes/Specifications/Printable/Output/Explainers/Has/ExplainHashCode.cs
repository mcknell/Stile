#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Globalization;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.Explainers.Has
{
    public class ExplainHashCode<TSubject> : Explainer<TSubject, int>
    {
        [Rule(Variable.Explainer, Items = new object[] {"{0}", Terminal.Have, "'hashCode' {1}", //
            Variable.Conjunction, Terminal.Had, "'hashCode'", Variable.ActualValue})]
        public ExplainHashCode(int hashCode)
            : base(ExpectationVerb.Have, "hashCode", result => hashCode.ToString(CultureInfo.InvariantCulture)) {}
    }
}
