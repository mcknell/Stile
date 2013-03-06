#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Is
{
	public static class IsComparableExtensions
	{
		[Pure]
		public static TSpecification ComparablyEquivalentTo<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder, TResult result)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : IComparable<TResult>
		{
			return Make(x => x.CompareTo(result) == 0, builder.Xray, Clause.IsComparablyEquivalentTo);
		}

		[Pure]
		public static TSpecification GreaterThan<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder, TResult result)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : IComparable<TResult>
		{
			return Make(x => x.CompareTo(result) > 0, builder.Xray, Clause.IsGreaterThan);
		}

		private static TSpecification Make<TSpecification, TSubject, TResult>(Expression<Predicate<TResult>> lambda,
			IIsState<TSpecification, TSubject, TResult> state,
			IClause clause) where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : IComparable<TResult>
		{
			Expectation<TSubject, TResult> expectation = Expectation<TSubject>.From(lambda, state.Negated, clause);
			return state.Make(expectation);
		}
	}
}
