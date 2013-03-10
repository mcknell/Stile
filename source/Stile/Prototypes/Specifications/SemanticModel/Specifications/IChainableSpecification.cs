#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications.Builders.OfExpectations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface IChainableSpecification : ISpecification {}

	public interface IChainableSpecification<out TPredicateBuilder> : IChainableSpecification
		where TPredicateBuilder : class, IExpectationBuilder
	{
		TPredicateBuilder AndThen { get; }
	}
}
