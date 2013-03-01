#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.Builders.OfPredicates;
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
		IHides<ISpecificationState<TSubject, TResult, TExpectationBuilder>>
		where TExpectationBuilder : class, IExpectationBuilder {}

	public interface ISpecificationState {}

	public interface ISpecificationState<out TSubject> : ISpecificationState
	{
		[CanBeNull]
		string Because { get; }
		[CanBeNull]
		ISource<TSubject> Source { get; }
	}

	public interface ISpecificationState<TSubject, TResult, out TExpectationBuilder> :
		ISpecificationState<TSubject>
		where TExpectationBuilder : class, IExpectationBuilder
	{
		[NotNull]
		ICriterion<TResult> Criterion { get; }
		[NotNull]
		IInstrument<TSubject, TResult> Instrument { get; }
	}

	public abstract class Specification
	{
		public delegate TSpecification Factory<out TSpecification, TSubject, TResult, in TExpectationBuilder>(
			ISource<TSubject> source,
			IInstrument<TSubject, TResult> instrument,
			ICriterion<TResult> criterion,
			TExpectationBuilder expectationBuilder,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null)
			where TSpecification : class, IChainableSpecification
			where TExpectationBuilder : class, IExpectationBuilder;

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

	public class Specification<TSubject, TResult, TExpectationBuilder> : Specification<TSubject>,
		IBoundSpecification<TSubject, TResult, TExpectationBuilder>,
		ISpecificationState<TSubject, TResult, TExpectationBuilder>
		where TExpectationBuilder : class, IExpectationBuilder
	{
		private readonly IExceptionFilter<TSubject, TResult> _exceptionFilter;
		private readonly TExpectationBuilder _expectationBuilder;

		protected Specification([NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] ICriterion<TResult> criterion,
			[NotNull] TExpectationBuilder expectationBuilder,
			ISource<TSubject> source = null,
			string because = null,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null)
			: base(source, because)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			Criterion = criterion.ValidateArgumentIsNotNull();
			_expectationBuilder = expectationBuilder.ValidateArgumentIsNotNull();
			_exceptionFilter = exceptionFilter;
		}

		public TExpectationBuilder AndThen
		{
			get { return _expectationBuilder; }
		}

		public ICriterion<TResult> Criterion { get; private set; }
		public IInstrument<TSubject, TResult> Instrument { get; private set; }
		public ISpecificationState<TSubject, TResult, TExpectationBuilder> Xray
		{
			get { return this; }
		}

		public IEvaluation<TSubject, TResult> Evaluate(TSubject subject)
		{
			return Evaluate(() => subject);
		}

		public IEvaluation<TSubject, TResult> Evaluate()
		{
			return Evaluate(Source.Get);
		}

		public static Specification<TSubject, TResult, TExpectationBuilder> Make(
			[CanBeNull] ISource<TSubject> source,
			[NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] ICriterion<TResult> criterion,
			[NotNull] TExpectationBuilder expectationBuilder,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null)
		{
			return new Specification<TSubject, TResult, TExpectationBuilder>(instrument,
				criterion,
				expectationBuilder,
				source,
				exceptionFilter : exceptionFilter);
		}

		public static Specification<TSubject, TResult, TExpectationBuilder> MakeBound(
			[NotNull] ISource<TSubject> source,
			[NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] ICriterion<TResult> criterion,
			[NotNull] TExpectationBuilder expectationBuilder,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null)
		{
			ISource<TSubject> validatedSource = source.ValidateArgumentIsNotNull();
			return new Specification<TSubject, TResult, TExpectationBuilder>(instrument,
				criterion,
				expectationBuilder,
				validatedSource,
				exceptionFilter : exceptionFilter);
		}

		private bool ExpectsException
		{
			get { return _exceptionFilter != null; }
		}

		private IEvaluation<TSubject, TResult> Evaluate(Func<TSubject> subjectGetter)
		{
			TResult result = default(TResult);
			IError[] errors = NoErrors;
			IEvaluation<TSubject, TResult> evaluation;
			try
			{
				// only trap exceptions while getting the subject or instrumenting it, not while accepting it
				TSubject subject = subjectGetter.Invoke();
				result = Instrument.Sample(subject);
			} catch (Exception e)
			{
				if (ExpectsException && _exceptionFilter.TryFilter(this, result, e, out evaluation))
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
			evaluation = new Evaluation<TSubject, TResult>(this, outcome, result, errors);
			return evaluation;
		}
	}
}
