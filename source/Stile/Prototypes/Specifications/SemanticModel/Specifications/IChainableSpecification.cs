#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface IChainableSpecification : ISpecification {}

	public interface IChainableSpecification<out TExpectationBuilder> : IChainableSpecification
		where TExpectationBuilder : class, IChainingConjuction
	{
		TExpectationBuilder AndThen { get; }
	}
}
