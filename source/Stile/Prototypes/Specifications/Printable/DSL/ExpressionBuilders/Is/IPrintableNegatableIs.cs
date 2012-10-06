#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Is;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Is
{
    public interface IPrintableNegatableIs : INegatableIs,
        IPrintableIs {}

    public interface IPrintableNegatableIs<out TResult> : IPrintableNegatableIs,
        INegatableIs<TResult>, IPrintableIs<TResult> {}

    public interface IPrintableNegatableIs<out TResult, out TNegated> : IPrintableNegatableIs<TResult>,
        INegatableIs<TResult, TNegated>
        where TNegated : class, IPrintableIs<TResult> {}

    public interface IPrintableNegatableIs<out TResult, out TNegated, TSubject> :
        IPrintableNegatableIs<TResult, TNegated>,
        INegatableIs
            <TResult, TNegated, IPrintableSpecification<TSubject, TResult>, IPrintableEvaluation<TResult>, TSubject>,
        IPrintableIs<TResult, TSubject>
        where TNegated : class, IPrintableIs<TResult, TSubject> {}
}
