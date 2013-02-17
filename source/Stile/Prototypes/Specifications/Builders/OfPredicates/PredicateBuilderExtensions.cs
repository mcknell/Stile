#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
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
		public static IEnumerablePredicateBuilder<TSpecification, TSubject, TResult, TItem> OfItemsLike
			<TSpecification, TSubject, TResult, TItem>(this IPredicateBuilder<TSpecification, TSubject, TResult> builder,
				TItem throwaway) where TResult : class, IEnumerable<TItem>
			where TSpecification : class, ISpecification<TSubject, TResult>
		{
			throw new NotImplementedException();
		}
	}
}
