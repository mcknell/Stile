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

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Is
{
    public interface IIs {}

    public interface IIs<out TResult> : IIs {}

    public interface IIs<out TResult, out TSpecifies, out TEvaluation> : IIs<TResult>
        where TSpecifies : class, ISpecification<TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult> {}

    public interface IIs<out TResult, out TSubject, out TSpecifies, out TEvaluation> :
        IIs<TResult, TSpecifies, TEvaluation>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult> {}

    public abstract class Is<TResult, TSubject, TNegated, TSpecifies, TEvaluation> :
        INegatableIs<TResult, TNegated, TSpecifies, TEvaluation, TSubject>,
        IIsState<TSubject, TResult>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult>
        where TNegated : class, IIs<TResult, TSubject, TSpecifies, TEvaluation>
    {
        protected Is(Negated negated, [NotNull] Lazy<Func<TSubject, TResult>> extractor)
        {
            Negated = negated;
            Instrument = extractor.ValidateArgumentIsNotNull();
        }

        public Lazy<Func<TSubject, TResult>> Instrument { get; private set; }
        public Negated Negated { get; private set; }
        public TNegated Not
        {
            get { return Factory(); }
        }

        [NotNull]
        protected abstract TNegated Factory();
    }
}
