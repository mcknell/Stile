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

	public interface ISpecification<TSubject, TResult> : ISpecification<TSubject>,
		IResultSpecification<TResult>,
		IHides<ISpecificationState<TSubject, TResult>>
	{
		[NotNull]
		IEvaluation<TResult> Evaluate(TSubject subject);
	}

	public interface ISpecificationState {}

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
		ICriterion<TResult> Criterion { get; }
		[NotNull]
		IInstrument<TSubject, TResult> Instrument { get; }
	}

	public abstract class Specification
	{
		public delegate TSpecification Factory<out TSpecification, TSubject, TResult>(
			ISource<TSubject> source, IInstrument<TSubject, TResult> instrument, ICriterion<TResult> criterion)
			where TSpecification : class, ISpecification<TSubject, TResult>;

		protected static readonly IError[] NoErrors = new IError[0];
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

	public class Specification<TSubject, TResult> : Specification<TSubject>,
		IBoundSpecification<TSubject, TResult>,
		ISpecificationState<TSubject, TResult>
	{
		private readonly IExceptionFilter<TResult> _exceptionFilter;

		protected Specification([NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] ICriterion<TResult> criterion,
			ISource<TSubject> source = null,
			string because = null,
			IExceptionFilter<TResult> exceptionFilter = null)
			: base(source, because)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			Criterion = criterion.ValidateArgumentIsNotNull();
			_exceptionFilter = exceptionFilter;
		}

		public ICriterion<TResult> Criterion { get; private set; }
		public IInstrument<TSubject, TResult> Instrument { get; private set; }
		public ISpecificationState<TSubject, TResult> Xray
		{
			get { return this; }
		}
		private bool ExpectsException
		{
			get { return _exceptionFilter != null; }
		}

		public IEvaluation<TResult> Evaluate()
		{
			return Evaluate(Source.Get);
		}

		public IEvaluation<TResult> Evaluate(TSubject subject)
		{
			return Evaluate(() => subject);
		}

		public static Specification<TSubject, TResult> Make([CanBeNull] ISource<TSubject> source,
			[NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] ICriterion<TResult> criterion)
		{
			return new Specification<TSubject, TResult>(instrument, criterion, source);
		}

		public static Specification<TSubject, TResult> MakeBound([NotNull] ISource<TSubject> source,
			[NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] ICriterion<TResult> criterion)
		{
			ISource<TSubject> validatedSource = source.ValidateArgumentIsNotNull();
			return new Specification<TSubject, TResult>(instrument, criterion, validatedSource);
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
				if (ExpectsException && _exceptionFilter.TryFilter(result, e, out evaluation))
				{
					return evaluation;
				}
				// allow unexpected exceptions to bubble out
				throw;
			}

			if (ExpectsException)
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
