#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates.Is
{
	public static class IsExtensions
	{
		[Pure]
		public static TSpecification EqualTo<TSpecification, TSubject, TResult>(
			this IPredicateIs<TSpecification, TSubject, TResult> builder, TResult result)
			where TSpecification : class, ISpecification<TSubject, TResult> where TResult : IEquatable<TResult>
		{
			return Make(builder, x => x.Equals(result));
		}

		private static TSpecification Make<TSpecification, TSubject, TResult>(
			IPredicateIs<TSpecification, TSubject, TResult> builder, Predicate<TResult> predicate)
			where TSpecification : class, ISpecification<TSubject, TResult> where TResult : IEquatable<TResult>
		{
			IPredicateIsState<TSpecification, TSubject, TResult> state = builder.Xray;
			Predicate<TResult> accepter = x => state.Negated.AgreesWith(predicate.Invoke(x));
			var criterion = new Criterion<TResult>(x => accepter.Invoke(x) ? Outcome.Succeeded : Outcome.Failed);
			return state.Make(criterion);
		}
	}
}
