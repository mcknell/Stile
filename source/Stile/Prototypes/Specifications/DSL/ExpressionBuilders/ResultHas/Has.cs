#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas
{
    public interface IHas {}

    public interface IHas<out TResult> : IHas {}

    public interface IHas<out TResult, out TSpecifies, out TEvaluation> : IHas<TResult>
        where TSpecifies : class, ISpecification<TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult> {}

    public interface IHas<out TResult, out TSpecifies, out TEvaluation, out TSubject> :
        IHas<TResult, TSpecifies, TEvaluation>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult> {}

    public class Has<TResult, TSubject, TSpecifies, TEvaluation> : IHas<TResult, TSpecifies, TEvaluation, TSubject>,
        IHasState<TSubject, TResult>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult>
    {
        public Has([NotNull] Lazy<Func<TSubject, TResult>> instrument)
        {
            Instrument = instrument.ValidateArgumentIsNotNull();
        }

        public Lazy<Func<TSubject, TResult>> Instrument { get; private set; }
    }
}
