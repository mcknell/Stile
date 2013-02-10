#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
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

	public interface ISpecification<TSubject, TResult> : ISpecification<TSubject>,
		IResultSpecification<TResult>,
		IHides<ISpecificationState<TSubject, TResult>>
	{
		[NotNull]
		IEvaluation<TResult> Evaluate(TSubject subject);
	}

	public interface ISpecificationState<TSubject, TResult>
	{
		[CanBeNull]
		string Because { get; }
		[NotNull]
		ICriterion<TResult> Criterion { get; }
		[NotNull]
		IInstrument<TSubject, TResult> Instrument { get; }
		[CanBeNull]
		ISource<TSubject> Source { get; }
	}

	public abstract class Specification
	{
		public delegate TSpecification Factory<out TSpecification, TSubject, TResult>(
			ISource<TSubject> source, IInstrument<TSubject, TResult> instrument, ICriterion<TResult> criterion)
			where TSpecification : class, ISpecification<TSubject, TResult>;

		protected static readonly IError[] NoErrors = new IError[0];

		public static Specification<TSubject, TResult> Make<TSubject, TResult>(
			[NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] ICriterion<TResult> criterion,
			ISource<TSubject> source = null,
			string because = null,
			IExceptionFilter<TResult> exceptionFilter = null)
		{
			return Specification<TSubject, TResult>.Make(instrument, criterion, source, because, exceptionFilter);
		}

		public static Specification<TSubject, TResult> MakeBound<TSubject, TResult>(
			[NotNull] ISource<TSubject> source,
			[NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] ICriterion<TResult> criterion,
			string because = null,
			IExceptionFilter<TResult> exceptionFilter = null)
		{
			return Specification<TSubject, TResult>.MakeBound(source, instrument, criterion, because, exceptionFilter);
		}

		public static Factory<IBoundSpecification<TSubject, TResult>, TSubject, TResult> MakeBoundFactory
			<TSubject, TResult>()
		{
			return (source, instrument, criterion) => MakeBound(source, instrument, criterion);
		}

		public static Factory<ISpecification<TSubject, TResult>, TSubject, TResult> MakeUnboundFactory
			<TSubject, TResult>()
		{
			return (source, instrument, criterion) => Make(instrument, criterion);
		}
	}

	public class Specification<TSubject, TResult> : Specification,
		IBoundSpecification<TSubject, TResult>,
		ISpecificationState<TSubject, TResult>
	{
		private readonly IExceptionFilter<TResult> _exceptionFilter;
		private readonly bool _expectsException;

		protected Specification([NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] ICriterion<TResult> criterion,
			ISource<TSubject> source = null,
			string because = null,
			IExceptionFilter<TResult> exceptionFilter = null)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			Criterion = criterion.ValidateArgumentIsNotNull();
			Source = source;
			Because = because;
			_exceptionFilter = exceptionFilter;
			_expectsException = _exceptionFilter != null;
		}

		public string Because { get; private set; }
		public ICriterion<TResult> Criterion { get; private set; }
		public IInstrument<TSubject, TResult> Instrument { get; private set; }
		public ISource<TSubject> Source { get; private set; }
		public ISpecificationState<TSubject, TResult> Xray
		{
			get { return this; }
		}

		public IEvaluation<TResult> Evaluate(TSubject subject)
		{
			return Evaluate(() => subject);
		}

		public IEvaluation<TResult> Evaluate()
		{
			return Evaluate(Source.Get);
		}

		public static Specification<TSubject, TResult> Make([NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] ICriterion<TResult> criterion,
			ISource<TSubject> source = null,
			string because = null,
			IExceptionFilter<TResult> exceptionFilter = null)
		{
			return new Specification<TSubject, TResult>(instrument, criterion, source, because, exceptionFilter);
		}

		public static Specification<TSubject, TResult> MakeBound([NotNull] ISource<TSubject> source,
			[NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] ICriterion<TResult> criterion,
			string because = null,
			IExceptionFilter<TResult> exceptionFilter = null)
		{
			return new Specification<TSubject, TResult>(instrument, criterion, source, because, exceptionFilter);
		}

		private IEvaluation<TResult> Evaluate(Func<TSubject> subjectGetter)
		{
			TResult result = default(TResult);
			IError[] errors = NoErrors;
			IEvaluation<TResult> evaluation;
			try
			{
				// only trap exceptions while getting the subject or instrumenting it, not while accepting it
				TSubject subject = subjectGetter.Invoke();
				result = Instrument.Sample(subject);
			} catch (Exception e)
			{
				if (_expectsException && _exceptionFilter.TryFilter(result, e, out evaluation))
				{
					return evaluation;
				}
				// allow unexpected exceptions to bubble out
				throw;
			}

			if (_expectsException)
			{
				// exception was expected but none was thrown
				return _exceptionFilter.Fail(result);
			}

			Outcome outcome = Criterion.Accept(result);
			evaluation = new Evaluation<TResult>(outcome, result, errors);
			return evaluation;
		}
	}
}
