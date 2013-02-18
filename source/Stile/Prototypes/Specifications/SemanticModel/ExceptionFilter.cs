#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IExceptionFilter
	{
		IEvaluation FailBeforeResult();
		bool TryFilterBeforeResult([NotNull] Exception e, out IEvaluation evaluation);
	}

	public interface IExceptionFilter<TResult> : IExceptionFilter
	{
		/// <summary>
		/// If exception was expected but none was thrown.
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		IEvaluation<TResult> Fail(TResult result);

		bool TryFilter(TResult result, [NotNull] Exception e, out IEvaluation<TResult> evaluation);
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

	public class ExceptionFilter<TResult> : ExceptionFilter,
		IExceptionFilter<TResult>
	{
		public ExceptionFilter([NotNull] Predicate<Exception> predicate)
			: base(predicate) {}

		public IEvaluation<TResult> Fail(TResult result)
		{
			throw new NotImplementedException();
		}

		public bool TryFilter(TResult result, Exception e, out IEvaluation<TResult> evaluation)
		{
			if(Predicate.Invoke(e))
			{
				evaluation = new Evaluation<TResult>(Outcome.Succeeded, result, new Error(e));
				return true;
			}
			evaluation = new Evaluation<TResult>(Outcome.Interrupted, result, new Error(e, false));
			return false;
		}
	}
}
