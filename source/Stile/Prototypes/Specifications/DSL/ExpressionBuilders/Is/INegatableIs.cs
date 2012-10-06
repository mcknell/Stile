#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Is
{
    public interface INegatableIs : IIs {}

    public interface INegatableIs<out TResult> : INegatableIs {}

    public interface INegatableIs<out TResult, out TNegated> : INegatableIs<TResult>,
        INegatable<TNegated> {}

    public interface INegatableIs<out TResult, out TNegated, out TSpecifies, out TEvaluation> :
        INegatableIs<TResult, TNegated>
        where TSpecifies : class, ISpecification<TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult> {}

    public interface INegatableIs<out TResult, out TNegated, out TSpecifies, out TEvaluation, out TSubject> :
        INegatableIs<TResult, TNegated>,
        IIs<TResult, TSpecifies, TEvaluation>
        where TNegated : class, IIs<TResult, TSubject, TSpecifies, TEvaluation>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult> {}
}
