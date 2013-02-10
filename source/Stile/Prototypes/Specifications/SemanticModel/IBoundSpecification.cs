#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IBoundSpecification : ISpecification {}

	public interface IResultBoundSpecification<out TResult> : IBoundSpecification,
		IResultSpecification<TResult> {}

	public interface IBoundSpecification<in TSubject> : IBoundSpecification,
		ISpecification<TSubject> {}

	public interface IBoundSpecification<TSubject, TResult> : IBoundSpecification<TSubject>,
		IResultBoundSpecification<TResult>,
		ISpecification<TSubject, TResult>
	{
		IEvaluation<TResult> Evaluate();
	}
}
