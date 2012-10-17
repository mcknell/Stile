#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.SemanticModel
{
	public interface IBoundSpecification : ISpecification {}

	public interface IBoundSpecification<in TSubject, out TResult> : IBoundSpecification,
		ISpecification<TSubject, TResult> {}

	public interface IBoundSpecification<in TSubject, out TResult, out TEvaluation> :
		IBoundSpecification<TSubject, TResult>,
		ISpecification<TSubject, TResult, TEvaluation>
		where TEvaluation : class, IEvaluation<TResult>
	{
		TEvaluation Evaluate();
	}
}
