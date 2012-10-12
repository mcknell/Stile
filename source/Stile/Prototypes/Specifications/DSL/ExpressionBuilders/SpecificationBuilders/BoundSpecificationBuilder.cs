#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Sources;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders
{
    public interface IBoundSpecificationBuilder : ISpecificationBuilder {}

    public interface IBoundSpecificationBuilder<out TSubject, out TResult, out THas, out TNegatableIs, out TIs,
        out TSpecifies, out TEvaluation> : IBoundSpecificationBuilder,
            ISpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies>
        where THas : class, IHas<TResult, TSpecifies>
        where TNegatableIs : class, INegatableIs<TResult, TIs, TSpecifies>
        where TIs : class, IIs<TResult, TSpecifies>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult> {}

    public interface IBoundSpecificationBuilderState : ISpecificationBuilderState {}

    public interface IBoundSpecificationBuilderState<out TSubject, out TSource> : IBoundSpecificationBuilderState
        where TSource : class, ISource<TSubject>
    {
        [NotNull]
        TSource Source { get; }
    }

    public abstract class BoundSpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies,
        TEvaluation, TSource> :
            SpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies>,
            IBoundSpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies, TEvaluation>,
            IBoundSpecificationBuilderState<TSubject, TSource>
        where THas : class, IHas<TResult, TSpecifies>
        where TNegatableIs : class, INegatableIs<TResult, TIs, TSpecifies>
        where TIs : class, IIs<TResult, TSpecifies>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult>
        where TSource : class, ISource<TSubject>
    {
        protected BoundSpecificationBuilder([NotNull] TSource source)
        {
            Source = source.ValidateArgumentIsNotNull();
        }

        public TSource Source { get; private set; }
    }
}
