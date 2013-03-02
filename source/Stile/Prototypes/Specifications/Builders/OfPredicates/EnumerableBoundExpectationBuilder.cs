#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates
{
	public interface IEnumerableBoundExpectationBuilder<out TSpecification, TSubject, TResult, TItem> :
		IEnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, IChainableSpecification, ISpecification<TSubject, TResult>
		where TResult : class, IEnumerable<TItem> {}
}
