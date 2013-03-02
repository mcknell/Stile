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

	public interface IExceptionFilter<out TSubject, TResult> : IExceptionFilter
	{
		/// <summary>
		/// If exception was expected but none was thrown.
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		TEvaluation Fail<TEvaluation>(TResult result) where TEvaluation : class, IEvaluation<TSubject, TResult>;

		bool TryFilter<TEvaluation>(TResult result,
			[NotNull] Exception e,
			[NotNull] Func<Outcome, TResult, IError, TEvaluation> factory,
			out TEvaluation evaluation) where TEvaluation : class, IEvaluation<TSubject, TResult>;
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

		public TEvaluation Fail<TEvaluation>(TResult result)
			where TEvaluation : class, IEvaluation<TSubject, TResult>
		{
			throw new NotImplementedException();
		}

		public bool TryFilter<TEvaluation>(TResult result,
			Exception e,
			Func<Outcome, TResult, IError, TEvaluation> factory,
			out TEvaluation evaluation) where TEvaluation : class, IEvaluation<TSubject, TResult>
		{
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			e.ValidateArgumentIsNotNull();
			factory.ValidateArgumentIsNotNull();
// ReSharper restore ReturnValueOfPureMethodIsNotUsed
			if (Predicate.Invoke(e))
			{
				evaluation = factory.Invoke(Outcome.Succeeded, result, new Error(e));
				return true;
			}
			evaluation = factory.Invoke(Outcome.Interrupted, result, new Error(e, false));
			return false;
		}
	}
}
