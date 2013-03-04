#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IExceptionFilter
	{
		IEvaluation FailBeforeResult(bool timedOut);

		bool TryFilterBeforeResult([NotNull] Exception e, out IEvaluation evaluation);
	}

	public interface IExceptionFilter<out TSubject, TResult> : IExceptionFilter
	{
		/// <summary>
		/// If exception was expected but none was thrown.
		/// </summary>
		/// <param name="result"></param>
		/// <param name="factory"></param>
		/// <param name="timedOut"></param>
		/// <returns></returns>
		TEvaluation Fail<TEvaluation>(TResult result,
			Evaluation.Factory<TSubject, TResult, TEvaluation> factory,
			bool timedOut) where TEvaluation : class, IEvaluation<TSubject, TResult>;

		IMeasurement<TResult> Filter(IMeasurement<TResult> measurement);

		bool TryFilter<TEvaluation>(TResult result,
			[NotNull] Exception e,
			[NotNull] Evaluation.Factory<TSubject, TResult, TEvaluation> factory,
			out TEvaluation evaluation) where TEvaluation : class, IEvaluation<TSubject, TResult>;
	}

	public class ExceptionFilter : IExceptionFilter
	{
		public ExceptionFilter([NotNull] Predicate<Exception> predicate)
		{
			Predicate = predicate.ValidateArgumentIsNotNull();
		}

		public IEvaluation FailBeforeResult(bool timedOut)
		{
			return new Evaluation(Outcome.Failed, timedOut);
		}

		public bool TryFilterBeforeResult(Exception e, out IEvaluation evaluation)
		{
			if (Predicate.Invoke(e))
			{
				evaluation = new Evaluation(Outcome.Succeeded, e);
				return true;
			}
			evaluation = new Evaluation(Outcome.Failed, false);
			return false;
		}

		protected Predicate<Exception> Predicate { get; private set; }
	}

	public class ExceptionFilter<TSubject, TResult> : ExceptionFilter,
		IExceptionFilter<TSubject, TResult>
	{
		public ExceptionFilter([NotNull] Predicate<Exception> predicate)
			: base(predicate) {}

		public TEvaluation Fail<TEvaluation>(TResult result,
			Evaluation.Factory<TSubject, TResult, TEvaluation> factory,
			bool timedOut) where TEvaluation : class, IEvaluation<TSubject, TResult>
		{
			return factory.Invoke(Outcome.Failed, result, timedOut);
		}

		public IMeasurement<TResult> Filter(IMeasurement<TResult> measurement)
		{
			var errors = new List<IError>();
			foreach (IError error in measurement.Errors)
			{
				if (Predicate.Invoke(error.Exception))
				{
					errors.Add(new Error(error.Exception, true));
				}
			}
			
			var filtered = new Measurement<TResult>(measurement.Value, measurement.TaskStatus, measurement.TimedOut, errors.ToArray());
			return filtered;
		}

		public bool TryFilter<TEvaluation>(TResult result,
			Exception e,
			Evaluation.Factory<TSubject, TResult, TEvaluation> factory,
			out TEvaluation evaluation) where TEvaluation : class, IEvaluation<TSubject, TResult>
		{
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			e.ValidateArgumentIsNotNull();
			factory.ValidateArgumentIsNotNull();
// ReSharper restore ReturnValueOfPureMethodIsNotUsed
			if (Predicate.Invoke(e))
			{
				evaluation = factory.Invoke(Outcome.Succeeded, result, false, new Error(e, true));
				return true;
			}
			evaluation = factory.Invoke(Outcome.Interrupted, result, false, new Error(e, false));
			return false;
		}
	}
}
