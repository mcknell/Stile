#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates.Has
{
	public static class HasExtensions
	{
		[Pure]
		public static ISpecification<TSubject, TResult> HashCode<TSpecification, TSubject, TResult>(
			this IPredicateHas<TSpecification, TSubject, TResult> has, int hashCode)
			where TSpecification : class, ISpecification<TSubject, TResult>
		{
			return Specification.Make(has.Xray.Instrument,
				new Criterion<TResult>(x => x.GetHashCode() == hashCode ? Outcome.Succeeded : Outcome.Failed));
		}
	}
}
