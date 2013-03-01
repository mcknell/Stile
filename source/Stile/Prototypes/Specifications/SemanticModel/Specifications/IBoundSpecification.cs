#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications.Builders.OfPredicates;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface IBoundSpecification : ISpecification {}

	public interface IResultBoundSpecification<out TResult> : IBoundSpecification,
		IResultSpecification<TResult> {}

	public interface IBoundSpecification<in TSubject> : IBoundSpecification,
		ISpecification<TSubject> {}

	public interface IBoundSpecification<in TSubject, out TResult> : IBoundSpecification<TSubject>,
		IResultBoundSpecification<TResult>,
		ISpecification<TSubject, TResult>,
		IChainableSpecification
	{
		IEvaluation<TSubject, TResult> Evaluate();
	}

	public interface IBoundSpecification<TSubject, TResult, out TExpectationBuilder> :
		IBoundSpecification<TSubject, TResult>,
		ISpecification<TSubject, TResult, TExpectationBuilder>
		where TExpectationBuilder : class, IExpectationBuilder {}
}
