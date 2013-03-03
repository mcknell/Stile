#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has
{
	public static class HasExtensions
	{
		[Pure]
		public static TSpecification HashCode<TSpecification, TSubject, TResult>(
			this IHas<TSpecification, TSubject, TResult> has, int hashCode)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		{
			var expectation =
				new Expectation<TResult>(x => x.GetHashCode() == hashCode ? Outcome.Succeeded : Outcome.Failed);
			TSpecification specification = has.Xray.Make(expectation);
			return specification;
		}
	}
}
