#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Stile.Prototypes.Specifications.Builders.OfPredicates;
using Stile.Prototypes.Specifications.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfInstruments
{
	public static class EnumerableInstrumentBuilderExtensions
	{
		[Pure]
		public static IEnumerablePredicateBuilder<ISpecification<TSubject, TResult>, TSubject, TResult, TItem>
			ThatTheSequence<TSubject, TResult, TItem>(this IInstrumentBuilder<TSubject> builder,
				Expression<Func<TSubject, TResult>> expression) where TResult : class, IEnumerable<TItem>
		{
			throw new NotImplementedException();
		}
	}
}
