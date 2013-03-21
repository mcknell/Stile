#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface IBoundSpecification : ISpecification {}

	public interface IBoundSpecification<in TSubject> : IBoundSpecification,
		ISpecification<TSubject> {}

	public interface IBoundSpecification<TSubject, TResult> : IBoundSpecification<TSubject>,
		ISpecification<TSubject, TResult>,
		IChainableSpecification
	{
		[Pure]
		IEvaluation<TSubject, TResult> Evaluate(IDeadline deadline = null);
	}

	public interface IBoundSpecification<TSubject, TResult, out TExpectationBuilder> :
		IBoundSpecification<TSubject, TResult>,
		ISpecification<TSubject, TResult, TExpectationBuilder>
		where TExpectationBuilder : class, IExpectationBuilder
	{
		new IBoundSpecification<TSubject, TResult, TExpectationBuilder> Because(string reason);
	}
}
