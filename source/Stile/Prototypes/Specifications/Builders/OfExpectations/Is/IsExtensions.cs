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
	public static class IsExtensions
	{
		[Pure]
		public static TSpecification EqualTo<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder, TResult result)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		{
			return Make(x => x.Equals(result), builder.Xray, Clause.IsEqualTo);
		}

		private static TSpecification Make<TSpecification, TSubject, TResult>(
			Expression<Predicate<TResult>> expression,
			IIsState<TSpecification, TSubject, TResult> state,
			IClause clause) where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		{
			Expectation<TSubject, TResult> expectation = Expectation<TSubject>.From(expression, state.Negated, clause);
			return state.Make(expectation);
		}
	}
}
