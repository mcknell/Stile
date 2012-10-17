#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.SemanticModel
{
	public interface IEmittingSpecification : ISpecification {}

	public interface IEmittingSpecification<in TSubject, out TResult> : IEmittingSpecification,
		ISpecification<TSubject, TResult> {}

	public interface IEmittingSpecification<in TSubject, out TResult, out TEvaluation, out TEmit> :
		IEmittingSpecification<TSubject, TResult>,
		ISpecification<TSubject, TResult, TEvaluation>
		where TEvaluation : class, IEmittingEvaluation<TResult, TEmit> {}

	public interface IEmittingSpecificationState<TSubject, TResult, out TEvaluation, TEmit> :
		ISpecificationState<TSubject, TResult, TEvaluation>
		where TEvaluation : class, IEmittingEvaluation<TResult, TEmit> {}
}
