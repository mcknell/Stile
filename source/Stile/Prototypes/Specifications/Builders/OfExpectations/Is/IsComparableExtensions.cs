#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Is
{
	public static class IsComparableExtensions
	{
		[Pure]
		public static TSpecification ComparablyEquivalentTo<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder, TResult expected)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : IComparable<TResult>
		{
			var term = new ComparablyEquivalentTo<TSpecification, TSubject, TResult>(builder.Xray, expected);
			return Make(builder.Xray, expected, term);
		}

		[Pure]
		public static TSpecification GreaterThan<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder, TResult result)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : IComparable<TResult>
		{
			return Make(x => x.CompareTo(result) > 0, builder.Xray);
		}

		private static TSpecification Make<TSpecification, TSubject, TResult>(
			IIsState<TSpecification, TSubject, TResult> state,
			TResult expected,
			ComparablyEquivalentTo<TSpecification, TSubject, TResult> term)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : IComparable<TResult>
		{
			Expectation<TSubject, TResult> expectation = Expectation<TSubject>.From(x => x.CompareTo(expected) == 0,
				state.Negated,
				state.BuilderState.Instrument,
				term);
			return state.BuilderState.Make(expectation);
		}

		private static TSpecification Make<TSpecification, TSubject, TResult>(Expression<Predicate<TResult>> lambda,
			IIsState<TSpecification, TSubject, TResult> state)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : IComparable<TResult>
		{
			Expectation<TSubject, TResult> expectation = Expectation<TSubject>.From(lambda,
				state.Negated,
				state.BuilderState.Instrument,
				state);
			return state.BuilderState.Make(expectation);
		}
	}
}
