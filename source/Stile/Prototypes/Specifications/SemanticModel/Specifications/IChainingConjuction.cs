#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface IChainingConjuction {}

	public interface IChainingConjuctionState<out TSpecification>
		where TSpecification : class, IChainableSpecification
	{
		[CanBeNull]
		TSpecification Prior { get; }
	}
}
