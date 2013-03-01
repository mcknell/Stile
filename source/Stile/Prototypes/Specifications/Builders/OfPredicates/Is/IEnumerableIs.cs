#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates.Is
{
	public interface IEnumerableIs<out TSpecification, TSubject, out TResult, TItem> :
		IIs<TSpecification, TSubject, TResult>
		where TResult : class, IEnumerable<TItem>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification {}
}
