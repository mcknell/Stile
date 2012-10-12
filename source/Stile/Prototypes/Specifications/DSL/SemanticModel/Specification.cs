#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.SemanticModel
{
	public interface ISpecification {}

	public interface ISpecification<in TSubject, out TResult> : ISpecification {}

	public interface ISpecification<in TSubject, out TResult, out TEvaluation> : ISpecification<TSubject, TResult>,
		IEvaluable<TSubject, TResult, TEvaluation>
		where TEvaluation : class, IEvaluation<TResult> {}

	public interface ISpecificationState<TSubject, TResult, out TEvaluation>
		where TEvaluation : class, IEvaluation<TResult>
	{
		Predicate<TResult> Accepter { get; }
		Func<TResult, Exception, TEvaluation> ExceptionFilter { get; }
		Lazy<Func<TSubject, TResult>> LazyInstrument { get; }
	}

	public abstract class Specification<TSubject, TResult, TEvaluation> : ISpecification<TSubject, TResult, TEvaluation>,
		ISpecificationState<TSubject, TResult, TEvaluation>
		where TEvaluation : class, IEvaluation<TResult>
	{
		private readonly Predicate<TResult> _accepter;
		private readonly Func<TResult, Exception, TEvaluation> _exceptionFilter;
		private readonly bool _expectsException;
		private readonly Lazy<Func<TSubject, TResult>> _lazyInstrument;

		protected Specification([NotNull] Lazy<Func<TSubject, TResult>> lazyInstrument,
			[NotNull] Predicate<TResult> accepter,
			Func<TResult, Exception, TEvaluation> exceptionFilter = null)
		{
			_lazyInstrument = lazyInstrument.ValidateArgumentIsNotNull();
			_accepter = accepter.ValidateArgumentIsNotNull();
			_exceptionFilter = exceptionFilter;
			_expectsException = ExceptionFilter != null;
		}

		public Predicate<TResult> Accepter
		{
			get { return _accepter; }
		}
		public Func<TResult, Exception, TEvaluation> ExceptionFilter
		{
			get { return _exceptionFilter; }
		}
		public Lazy<Func<TSubject, TResult>> LazyInstrument
		{
			get { return _lazyInstrument; }
		}

		public virtual TEvaluation Evaluate(TSubject subject)
		{
			Outcome outcome;
			TResult result = default(TResult);
			TEvaluation evaluation;
			try
			{
				result = LazyInstrument.Value.Invoke(subject);
				bool accepted = Accepter.Invoke(result);
				outcome = accepted ? Outcome.Succeeded : Outcome.Failed;
			} catch (Exception e)
			{
				if (_expectsException)
				{
					evaluation = ExceptionFilter.Invoke(result, e);
					if (evaluation != null)
					{
						// only trap exception if exception filter handled it
						return evaluation;
					}
				}
				throw;
			}

			if (_expectsException)
			{
				// exception was expected but none was thrown
				return ExceptionFilter.Invoke(result, null);
			}

			var wrappedResult = new WrappedResult<TResult>(outcome, result);
			evaluation = EvaluationFactory(wrappedResult);
			return evaluation;
		}

		protected abstract TEvaluation EvaluationFactory(IWrappedResult<TResult> result);
	}
}
