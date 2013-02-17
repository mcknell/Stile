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
		private readonly Predicate<Exception> _predicate;

		public ExceptionFilter([NotNull] Predicate<Exception> predicate)
		{
			_predicate = predicate.ValidateArgumentIsNotNull();
		}

		public IEvaluation FailBeforeResult()
		{
			throw new NotImplementedException();
		}

		public bool TryFilterBeforeResult(Exception e, out IEvaluation evaluation)
		{
			if (_predicate.Invoke(e))
			{
				evaluation = new Evaluation(Outcome.Succeeded, e);
				return true;
			}
			evaluation = null;
			return false;
		}
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
			throw new NotImplementedException();
		}
	}
}
