#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
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
			return Make(builder, x => x == 0, result);
		}

		[Pure]
		public static TSpecification GreaterThan<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder, TResult result)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : IComparable<TResult>
		{
			return Make(builder, x => x > 0, result);
		}

		[Pure]
		public static TSpecification Make<TSpecification, TSubject, TResult>(
			IIs<TSpecification, TSubject, TResult> builder, Predicate<int> predicate, TResult result)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : IComparable<TResult>
		{
			IIsState<TSpecification, TSubject, TResult> state = builder.Xray;
			Predicate<TResult> accepter = x => state.Negated.AgreesWith(predicate.Invoke(x.CompareTo(result)));
			var criterion = new Criterion<TResult>(x => accepter.Invoke(x) ? Outcome.Succeeded : Outcome.Failed);
			return state.Make(criterion);
		}
	}
}
