#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.Printable.Output.Explainers;
using Stile.Prototypes.Specifications.Printable.Output.Explainers.Has;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Has
{
    public static class HasExtensions
    {
        [Pure]
        public static IPrintableSpecification<TSubject, TResult> HashCode<TResult, TSubject>(
            this IPrintableHas<TResult, TSubject> has, int hashCode)
        {
            var state = (IPrintableHasState<TSubject, TResult>) has;

            Predicate<TResult> predicate = x => x.GetHashCode() == hashCode;
            IExplainer<TSubject, TResult> explainer = new ExplainHashCode<TSubject, TResult>(hashCode);
            return new PrintableSpecification<TSubject, TResult>(state.Instrument, predicate, explainer);
        }

        //[Pure]
        //public static IPrintableSpecification<TSubject, int> HashCode<TSubject, TResult>(this IPrintableHas<TSubject, TResult> has,
        //    int hashCode)
        //{
        //    var state = (IPrintableHasState<TSubject, TResult>) has;
        //    var instrument = new Lazy<Func<TSubject, int>>(() => x => state.Instrument.Value.Invoke(x).GetHashCode());
        //    Predicate<int> predicate = x => x == hashCode;
        //    ExplainHashCode<TSubject> explainer = Explain.Subject<TSubject>().HashCode<TSubject>(hashCode);
        //    return new PrintableSpecification<TSubject, int>(instrument, predicate, explainer, state.SubjectDescription);
        //}
    }
}
