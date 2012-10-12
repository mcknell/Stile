#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
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
	public interface IBoundSpecificationBuilderForEnumerableSubject : ISpecificationBuilderForEnumerableSubject {}

	public interface IBoundSpecificationBuilderForEnumerableSubject<out TSubject, out TItem, out TResult, out THas,
		out TNegatableIs, out TIs, out TSpecifies, out TEvaluation> : IBoundSpecificationBuilderForEnumerableSubject,
			ISpecificationBuilderForEnumerableSubject<TSubject, TItem, TResult, THas, TNegatableIs, TIs, TSpecifies>
		where TSubject : class, IEnumerable<TItem>
		where THas : class, IHas<TResult, TSpecifies>
		where TNegatableIs : class, INegatableIs<TResult, TIs, TSpecifies>
		where TIs : class, IIs<TResult, TSpecifies>
		where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
		where TEvaluation : class, IEvaluation<TResult> {}

	public interface IBoundSpecificationBuilderForEnumerableSubjectState : IBoundSpecificationBuilderState {}

	public interface IBoundSpecificationBuilderForEnumerableSubjectState<out TSubject, out TItem> :
		IBoundSpecificationBuilderForEnumerableSubjectState,
		IBoundSpecificationBuilderState<TSubject, ISource<TSubject>>
		where TSubject : class, IEnumerable<TItem> {}

	public abstract class BoundSpecificationBuilderForEnumerableSubject<TSubject, TItem, TResult, THas, TNegatableIs,
		TIs, TSpecifies, TEvaluation> :
			SpecificationBuilderForEnumerableSubject
				<TSubject, TItem, TResult, THas, TNegatableIs, TIs, TSpecifies, TEvaluation>,
			IBoundSpecificationBuilderForEnumerableSubject
				<TSubject, TItem, TResult, THas, TNegatableIs, TIs, TSpecifies, TEvaluation>,
			IBoundSpecificationBuilderForEnumerableSubjectState<TSubject, TItem>
		where TSubject : class, IEnumerable<TItem>
		where THas : class, IHas<TResult, TSpecifies>
		where TNegatableIs : class, INegatableIs<TResult, TIs, TSpecifies>
		where TIs : class, IIs<TResult, TSpecifies>
		where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
		where TEvaluation : class, IEvaluation<TResult>
	{
		protected BoundSpecificationBuilderForEnumerableSubject([NotNull] ISource<TSubject> source)
		{
			Source = source.ValidateArgumentIsNotNull();
		}

		public ISource<TSubject> Source { get; private set; }
	}
}
