#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders
{
    public interface ISpecificationBuilderForEnumerableSubject : ISpecificationBuilder {}

    public interface ISpecificationBuilderForEnumerableSubject<out TSubject, out TItem, out TResult, out THas,
        out TNegatableIs, out TIs, out TSpecifies> : ISpecificationBuilderForEnumerableSubject,
            ISpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies>
        where TSubject : class, IEnumerable<TItem>
        where THas : class, IHas<TResult, TSpecifies>
        where TNegatableIs : class, INegatableIs<TResult, TIs, TSpecifies>
        where TIs : class, IIs<TResult, TSpecifies>
        where TSpecifies : class, ISpecification<TSubject, TResult> {}

    public interface ISpecificationBuilderForEnumerableSubjectState : ISpecificationBuilderState {}

    public abstract class SpecificationBuilderForEnumerableSubject<TSubject, TItem, TResult, THas, TNegatableIs, TIs,
        TSpecifies, TEvaluation> :
            SpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies>,
            ISpecificationBuilderForEnumerableSubject
                <TSubject, TItem, TResult, THas, TNegatableIs, TIs, TSpecifies>,
            ISpecificationBuilderForEnumerableSubjectState
        where TSubject : class, IEnumerable<TItem>
        where THas : class, IHas<TResult, TSpecifies>
        where TNegatableIs : class, INegatableIs<TResult, TIs, TSpecifies>
        where TIs : class, IIs<TResult, TSpecifies>
        where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult> {}
}
