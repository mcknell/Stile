#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.Emitting
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

	public abstract class EmittingSpecification<TSubject, TResult, TEvaluation, TEmit> :
		Specification<TSubject, TResult, TEvaluation>,
		IEmittingSpecification<TSubject, TResult, TEvaluation, TEmit>
		where TEvaluation : class, IEmittingEvaluation<TResult, TEmit>
	{
		protected EmittingSpecification([NotNull] Lazy<Func<TSubject, TResult>> instrument,
			[NotNull] Predicate<TResult> accepter,
			Func<TResult, Exception, TEvaluation> exceptionFilter = null)
			: base(instrument, accepter, exceptionFilter) {}

		protected abstract TEmit EmittingFactory(IWrappedResult<TResult> result);

		protected override sealed TEvaluation EvaluationFactory(IWrappedResult<TResult> result)
		{
			TEmit emit = EmittingFactory(result);
			TEvaluation evaluation = EvaluationFactory(result, emit);
			return evaluation;
		}

		protected abstract TEvaluation EvaluationFactory(IWrappedResult<TResult> result, TEmit emitted);
	}
}
