#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Has;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Is;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders
{
    public interface IBoundSpecificationBuilder<out TSubject, out TResult, out THas, out TNegatableIs, out TIs,
        out TSpecifies, out TEvaluation> :
            ISpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies, TEvaluation>
        where THas : class, IHas<TResult, TSpecifies, TEvaluation, TSubject>
        where TNegatableIs : class, INegatableIs<TResult, TIs, TSpecifies, TEvaluation, TSubject>
        where TIs : class, IIs<TResult, TSubject, TSpecifies, TEvaluation>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult> {}

    public abstract class BoundSpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies,
        TEvaluation> : SpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies, TEvaluation>,
            IBoundSpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies, TEvaluation>,
            IBoundSpecificationBuilderState<TSubject>
        where THas : class, IHas<TResult, TSpecifies, TEvaluation, TSubject>
        where TNegatableIs : class, INegatableIs<TResult, TIs, TSpecifies, TEvaluation, TSubject>
        where TIs : class, IIs<TResult, TSubject, TSpecifies, TEvaluation>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult>
    {
        protected BoundSpecificationBuilder([NotNull] ISource<TSubject> source)
        {
            Source = source.ValidateArgumentIsNotNull();
        }

        public ISource<TSubject> Source { get; private set; }
    }
}
