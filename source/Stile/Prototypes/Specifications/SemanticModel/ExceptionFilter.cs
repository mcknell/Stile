#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IExceptionFilter
	{
		IEvaluation FailBeforeResult();
		bool TryFilterBeforeResult([NotNull] Exception e, out IEvaluation evaluation);
	}

	public interface IExceptionFilter<TSubject, TResult> : IExceptionFilter
	{
		/// <summary>
		/// If exception was expected but none was thrown.
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		IEvaluation<TSubject, TResult> Fail(TResult result);

		bool TryFilter(ISpecification<TSubject, TResult> specification,
			TResult result,
			[NotNull] Exception e,
			out IEvaluation<TSubject, TResult> evaluation);
	}

	public class ExceptionFilter : IExceptionFilter
	{
		public ExceptionFilter([NotNull] Predicate<Exception> predicate)
		{
			Predicate = predicate.ValidateArgumentIsNotNull();
		}

		public IEvaluation FailBeforeResult()
		{
			throw new NotImplementedException();
		}

		public bool TryFilterBeforeResult(Exception e, out IEvaluation evaluation)
		{
			if (Predicate.Invoke(e))
			{
				evaluation = new Evaluation(Outcome.Succeeded, e);
				return true;
			}
			evaluation = new Evaluation(Outcome.Failed);
			return false;
		}

		protected Predicate<Exception> Predicate { get; private set; }
	}

	public class ExceptionFilter<TSubject, TResult> : ExceptionFilter,
		IExceptionFilter<TSubject, TResult>
	{
		public ExceptionFilter([NotNull] Predicate<Exception> predicate)
			: base(predicate) {}

		public IEvaluation<TSubject, TResult> Fail(TResult result)
		{
			throw new NotImplementedException();
		}

		public bool TryFilter(ISpecification<TSubject, TResult> specification,
			TResult result,
			Exception e,
			out IEvaluation<TSubject, TResult> evaluation)
		{
			if (Predicate.Invoke(e))
			{
				evaluation = new Evaluation<TSubject, TResult>(specification, Outcome.Succeeded, result, new Error(e));
				return true;
			}
			evaluation = new Evaluation<TSubject, TResult>(specification,
				Outcome.Interrupted,
				result,
				new Error(e, false));
			return false;
		}
	}
}
