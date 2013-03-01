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
		public static IEnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem> OfItemsLike
			<TSpecification, TSubject, TResult, TItem>(
			this IExpectationBuilder<TSpecification, TSubject, TResult> builder, TItem throwaway)
			where TSpecification : class,
				ISpecification<TSubject, TResult, IExpectationBuilder<TSpecification, TSubject, TResult>>
			where TResult : class, IEnumerable<TItem>
		{
			IExpectationBuilderState<TSpecification, TSubject, TResult> state = builder.Xray;
			IInstrument<TSubject, TResult> instrument = state.Instrument;

			// Specification<TSubject, TResult, EnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem>>. Make
			return new EnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem>(instrument,
				(source, instrument1, criterion, expectationBuilder, filter) => builder.Xray.Make(criterion),
				state.Source);
		}

		[Pure]
		public static IEnumerableBoundExpectationBuilder<TSpecification, TSubject, TResult, TItem> OfItemsLike
			<TSpecification, TSubject, TResult, TPredicateBuilder, TItem>(
			this IBoundExpectationBuilder<TSpecification, TSubject, TResult> builder, TItem throwaway)
			where TSpecification : class,
				IBoundSpecification
					<TSubject, TResult, IEnumerableBoundExpectationBuilder<TSpecification, TSubject, TResult, TItem>>,
				IBoundSpecification<TSubject, TResult, TPredicateBuilder> where TResult : class, IEnumerable<TItem>
			where TPredicateBuilder : class, IBoundExpectationBuilder<TSpecification, TSubject, TResult>
		{
			IExpectationBuilderState<TSpecification, TSubject, TResult> state = builder.Xray;
			IInstrument<TSubject, TResult> instrument = state.Instrument;
			return
				new EnumerableBoundExpectationBuilder<TSpecification, TSubject, TResult, TPredicateBuilder, TItem>(
					instrument,
					(source, instrument1, criterion, expectationBuilder, filter) => builder.Xray.Make(criterion),
					state.Source);
		}
	}

	public interface IFluentEnumerableBoundSpecification<TSubject, TResult, TItem> :
		IBoundSpecification<TSubject, TResult, IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem>>
		where TResult : class, IEnumerable<TItem> {}

	public interface IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem> :
		IEnumerableBoundExpectationBuilder
			<IFluentEnumerableBoundSpecification<TSubject, TResult, TItem>, TSubject, TResult, TItem>
		where TResult : class, IEnumerable<TItem> {}
}
