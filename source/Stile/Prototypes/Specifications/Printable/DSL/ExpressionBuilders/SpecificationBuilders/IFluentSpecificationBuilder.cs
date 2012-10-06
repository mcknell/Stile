#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultHas;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders
{
    public interface IFluentSpecificationBuilder<TSubject> :
        IPrintableSpecificationBuilder
            <TSubject, TSubject, IPrintableHas<TSubject, TSubject>,
                IPrintableNegatableIs<TSubject, TSubject, IPrintableIs<TSubject, TSubject>>,
                IPrintableIs<TSubject, TSubject>> {}

    public interface IFluentSpecificationBuilder<TSubject, out TResult> :
        IPrintableSpecificationBuilder
            <TSubject, TResult, IPrintableHas<TResult, TSubject>,
                IPrintableNegatableIs<TSubject, TResult, IPrintableIs<TSubject, TResult>>,
                IPrintableIs<TSubject, TResult>> {}
}
