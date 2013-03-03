#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface ISpecification {}

// ReSharper disable UnusedTypeParameter
	/// <summary>
	/// Generalization so that Is and Has can have generic type constraints that don't specify a Subject.
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public interface IResultSpecification<out TResult> : ISpecification {}

// ReSharper restore UnusedTypeParameter

	public interface ISpecification<in TSubject> : ISpecification {}

	public interface ISpecification<in TSubject, out TResult> : ISpecification<TSubject>,
		IResultSpecification<TResult>
	{
		[NotNull]
		IEvaluation<TSubject, TResult> Evaluate(TSubject subject);
	}

	public interface ISpecification<TSubject, TResult, out TExpectationBuilder> :
		ISpecification<TSubject, TResult>,
		IChainableSpecification<TExpectationBuilder>,
		IHides<ISpecificationState<TSubject, TResult>>
		where TExpectationBuilder : class, IExpectationBuilder {}

	public interface ISpecificationState
	{
		object Clone(ISpecificationDeadline deadline);
	}

	public interface ISpecificationState<out TSubject> : ISpecificationState
	{
		[CanBeNull]
		string Because { get; }
		[CanBeNull]
		ISource<TSubject> Source { get; }
	}

	public interface ISpecificationState<TSubject, TResult> : ISpecificationState<TSubject>
	{
		[NotNull]
		IExpectation<TResult> Expectation { get; }
		[NotNull]
		IInstrument<TSubject, TResult> Instrument { get; }
	}

	public interface ISpecificationDeadline
	{
		CancellationToken? CancellationToken { get; }
		TimeSpan? Timeout { get; }
	}

	public class SpecificationDeadline : ISpecificationDeadline
	{
		public SpecificationDeadline(TimeSpan timeout)
			: this(timeout.Duration(), null) {}

		private SpecificationDeadline(TimeSpan? timeout, CancellationToken? cancellationToken)
		{
			Timeout = timeout;
			CancellationToken = cancellationToken;
		}

		public CancellationToken? CancellationToken { get; private set; }
		public TimeSpan? Timeout { get; private set; }
	}

	public abstract class Specification : ISpecificationState
	{
		public delegate TSpecification Factory<out TSpecification, TSubject, TResult, in TExpectationBuilder>(
			ISource<TSubject> source,
			IInstrument<TSubject, TResult> instrument,
			IExpectation<TResult> expectation,
			TExpectationBuilder expectationBuilder,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null)
			where TSpecification : class, IChainableSpecification
			where TExpectationBuilder : class, IExpectationBuilder;

		public static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);
		protected static readonly IError[] NoErrors = new IError[0];

		public abstract object Clone(ISpecificationDeadline deadline);
	}

	public abstract class Specification<TSubject> : Specification,
		ISpecification<TSubject>
	{
		protected Specification([CanBeNull] ISource<TSubject> source, [CanBeNull] string because)
		{
			Source = source;
			Because = because;
		}

		public string Because { get; private set; }
		public ISource<TSubject> Source { get; private set; }
	}

	public class Specification<TSubject, TResult, TExpectationBuilder> : Specification<TSubject>,
		IBoundSpecification<TSubject, TResult, TExpectationBuilder>,
		ISpecificationState<TSubject, TResult>
		where TExpectationBuilder : class, IExpectationBuilder
	{
		private readonly ISpecificationDeadline _deadline;
		private readonly IExceptionFilter<TSubject, TResult> _exceptionFilter;
		private readonly TExpectationBuilder _expectationBuilder;

		public Specification([NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] IExpectation<TResult> expectation,
			[NotNull] TExpectationBuilder expectationBuilder,
			ISource<TSubject> source = null,
			string because = null,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null,
			ISpecificationDeadline deadline = null)
			: base(source, because)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			Expectation = expectation.ValidateArgumentIsNotNull();
			_expectationBuilder = expectationBuilder.ValidateArgumentIsNotNull();
			_exceptionFilter = exceptionFilter;
			_deadline = deadline;
		}

		public TExpectationBuilder AndThen
		{
			get { return _expectationBuilder; }
		}

		public IExpectation<TResult> Expectation { get; private set; }
		public IInstrument<TSubject, TResult> Instrument { get; private set; }
		public ISpecificationState<TSubject, TResult> Xray
		{
			get { return this; }
		}

		public override object Clone(ISpecificationDeadline deadline)
		{
			return new Specification<TSubject, TResult, TExpectationBuilder>(Instrument,
				Expectation,
				_expectationBuilder,
				Source,
				Because,
				_exceptionFilter,
				deadline);
		}

		public IEvaluation<TSubject, TResult> Evaluate(TSubject subject)
		{
			return Evaluate(() => subject, UnboundFactory);
		}

		public IBoundEvaluation<TSubject, TResult> Evaluate()
		{
			return Evaluate(Source.Get, BoundFactory);
		}

		private bool ExpectsException
		{
			get { return _exceptionFilter != null; }
		}

		private IBoundEvaluation<TSubject, TResult> BoundFactory(Outcome outcome,
			TResult result,
			bool timedOut,
			params IError[] error)
		{
			return new BoundEvaluation<TSubject, TResult>(this, outcome, result, timedOut, error);
		}

		private TEvaluation Evaluate<TEvaluation>(Func<TSubject> subjectGetter,
			Evaluation.Factory<TSubject, TResult, TEvaluation> evaluationFactory,
			CancellationToken? cancellationToken = null) where TEvaluation : class, IEvaluation<TSubject, TResult>
		{
			TResult result = default(TResult);
			IError[] errors = NoErrors;

			TimeSpan timeout = DefaultTimeout;
			if (_deadline != null)
			{
				if (_deadline.Timeout.HasValue)
				{
					timeout = _deadline.Timeout.Value;
				}
				cancellationToken = cancellationToken ?? _deadline.CancellationToken;
			}
			var millisecondsTimeout = (int) timeout.TotalMilliseconds;

			Task<TResult> task;
			bool timedOut;
			try
			{
				// only trap exceptions while getting the subject or instrumenting it, not while accepting it

				task = new Task<TResult>(() =>
				{
					TSubject subject = subjectGetter.Invoke();
					return Instrument.Sample(subject);
				});
				//task.RunSynchronously();
				task.Start();
				if (cancellationToken.HasValue)
				{
					timedOut = !task.Wait(millisecondsTimeout, cancellationToken.Value);
				} else
				{
					timedOut = !task.Wait(millisecondsTimeout);
				}
				result = task.Result;
			} catch (Exception e)
			{
				Exception thrownException = e;
				if (e is AggregateException)
				{
					thrownException = e.InnerException;
					if (thrownException == null)
					{
						foreach (DictionaryEntry dictionaryEntry in e.Data)
						{
							thrownException = (Exception) dictionaryEntry.Value;
							break;
						}
					}
				}
				TEvaluation evaluation;
				if (ExpectsException
					&& _exceptionFilter.TryFilter(result, thrownException, evaluationFactory, out evaluation))
				{
					return evaluation;
				}
				// allow unexpected exceptions to bubble out
				throw;
			}

			if (ExpectsException && !timedOut && !task.IsFaulted)
			{
				// exception was expected but none was thrown
				return _exceptionFilter.Fail(result, evaluationFactory, timedOut);
			}

			Outcome outcome = Expectation.Accept(result);
			if (task.IsFaulted)
			{
				errors = new IError[] {new Error(task.Exception.GetBaseException())};
			}
			return evaluationFactory.Invoke(outcome, result, timedOut, errors);
		}

		private IEvaluation<TSubject, TResult> UnboundFactory(Outcome outcome,
			TResult result,
			bool timedOut,
			params IError[] error)
		{
			return new Evaluation<TSubject, TResult>(this, outcome, result, timedOut, error);
		}
	}
}
