#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders
{
    public interface ISpecificationBuilder {}

    public interface ISpecificationBuilder<out TSubject, out TResult, out THas, out TNegatableIs, out TIs,
        out TSpecifies, out TEvaluation> : ISpecificationBuilder
        where THas : class, IHas<TResult, TSpecifies, TEvaluation, TSubject>
        where TNegatableIs : class, INegatableIs<TSubject, TResult, TIs, TSpecifies, TEvaluation>
        where TIs : class, IIs<TSubject, TResult, TSpecifies, TEvaluation>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult>
    {
        THas Has { get; }
        TNegatableIs Is { get; }
    }

    public abstract class SpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies, TEvaluation> :
        ISpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies, TEvaluation>
        where THas : class, IHas<TResult, TSpecifies, TEvaluation, TSubject>
        where TNegatableIs : class, INegatableIs<TSubject, TResult, TIs, TSpecifies, TEvaluation>
        where TIs : class, IIs<TSubject, TResult, TSpecifies, TEvaluation>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult>
    {
        private readonly Lazy<THas> _lazyHas;
        private readonly Lazy<TNegatableIs> _lazyIs;

        protected SpecificationBuilder()
        {
            _lazyHas = new Lazy<THas>(MakeHas);
            _lazyIs = new Lazy<TNegatableIs>(MakeIs);
        }

        public THas Has
        {
            get
            {
                THas value = _lazyHas.Value;
                return value;
            }
        }
        public TNegatableIs Is
        {
            get
            {
                TNegatableIs value = _lazyIs.Value;
                return value;
            }
        }
        protected abstract THas MakeHas();
        protected abstract TNegatableIs MakeIs();
    }
}
