#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates
{
	public static class PredicateBuilderExtensions
	{
		[Pure]
		public static IEnumerablePredicateBuilder<ISpecification<TSubject, TResult>, TSubject, TResult, TItem>
			OfItemsLike<TSpecification, TSubject, TResult, TItem>(
			this IPredicateBuilder<TSpecification, TSubject, TResult> builder, TItem throwaway)
			where TSpecification : class, ISpecification<TSubject, TResult> where TResult : class, IEnumerable<TItem>
		{
			IPredicateBuilderState<TSubject, TResult> state = builder.Xray;
			IInstrument<TSubject, TResult> instrument = state.Instrument;
			return
				new EnumerablePredicateBuilder<ISpecification<TSubject, TResult>, TSubject, TResult, TItem>(instrument,
					Specification<TSubject, TResult>.Make,
					state.Source);
		}
		[Pure]
		public static IEnumerablePredicateBuilder<IBoundSpecification<TSubject, TResult>, TSubject, TResult, TItem>
			OfItemsLike<TSpecification, TSubject, TResult, TItem>(
			this IBoundPredicateBuilder<TSpecification, TSubject, TResult> builder, TItem throwaway)
			where TSpecification : class, IBoundSpecification<TSubject, TResult> where TResult : class, IEnumerable<TItem>
		{
			IPredicateBuilderState<TSubject, TResult> state = builder.Xray;
			IInstrument<TSubject, TResult> instrument = state.Instrument;
			return
				new EnumerablePredicateBuilder<IBoundSpecification<TSubject, TResult>, TSubject, TResult, TItem>(instrument,
					Specification<TSubject, TResult>.MakeBound,
					state.Source);
		}
	}
}
