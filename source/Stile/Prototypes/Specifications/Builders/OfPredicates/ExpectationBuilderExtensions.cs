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
	public static class ExpectationBuilderExtensions
	{
		[Pure]
		public static IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem> OfItemsLike
			<TSpecification, TSubject, TResult, TItem>(this IExpectationBuilder<TSpecification, TSubject, TResult> builder,
				TItem throwaway)
			where TSpecification : class,
				ISpecification<TSubject, TResult, IExpectationBuilder<TSpecification, TSubject, TResult>>
			where TResult : class, IEnumerable<TItem>
		{
			IExpectationBuilderState<TSpecification, TSubject, TResult> state = builder.Xray;
			IInstrument<TSubject, TResult> instrument = state.Instrument;
			ISource<TSubject> source = builder.Xray.Source;
			return new FluentEnumerableExpectationBuilder<TSubject, TResult, TItem>(instrument,
				state.Source);
		}

		[Pure]
		public static IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem> OfItemsLike
			<TSpecification, TSubject, TResult, TItem>(
			this IBoundExpectationBuilder<TSpecification, TSubject, TResult> builder, TItem throwaway)
			where TSpecification : class,
				IBoundSpecification<TSubject, TResult, IBoundExpectationBuilder<TSpecification, TSubject, TResult>>
			where TResult : class, IEnumerable<TItem>
		{
			IExpectationBuilderState<TSpecification, TSubject, TResult> state = builder.Xray;
			IInstrument<TSubject, TResult> instrument = state.Instrument;
			return new FluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem>(instrument,
				state.Source);
		}
	}
}
