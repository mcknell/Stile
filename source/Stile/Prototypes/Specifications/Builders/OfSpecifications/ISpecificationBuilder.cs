#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfSpecifications
{
	public interface ISpecificationBuilder {}

	public interface ISpecificationBuilder<out TSpecification> : ISpecificationBuilder
		where TSpecification : class, IChainableSpecification
	{
		TSpecification Build();
	}
}
