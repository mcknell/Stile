#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations
{
	public interface IBoundExpectationBuilder : IExpectationBuilder {}

	public interface IBoundExpectationBuilder<out TSpecification, TSubject, TResult> : IBoundExpectationBuilder,
		IExpectationBuilder<TSpecification, TSubject, TResult>
		where TSpecification : class, IBoundSpecification<TSubject, TResult>, IChainableSpecification {}
}
