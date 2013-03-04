#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.Builders.Lifecycle;
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

	public interface ISpecification<TSubject, TResult> : ISpecification<TSubject>,
		IResultSpecification<TResult>,
		IHides<ISpecificationState<TSubject, TResult>>,
		IEvaluable<TSubject, TResult, IEvaluation<TSubject, TResult>> {}

	public interface ISpecification<TSubject, TResult, out TExpectationBuilder> :
		ISpecification<TSubject, TResult>,
		IChainableSpecification<TExpectationBuilder>
		where TExpectationBuilder : class, IExpectationBuilder {}

	public interface ISpecificationState
	{
		[System.Diagnostics.Contracts.Pure]
		ISpecification Clone(IDeadline deadline);
	}

	public interface ISpecificationState<out TSubject> : ISpecificationState
	{
		[CanBeNull]
		string Because { get; }
	}

	public interface ISpecificationState<TSubject, TResult> : ISpecificationState<TSubject>,
		IHasExpectation<TSubject, TResult> {}

	public abstract class Specification : ISpecificationState
	{
		public delegate TSpecification Factory<out TSpecification, TSubject, TResult, in TExpectationBuilder>(
			ISource<TSubject> source,
			IInstrument<TSubject, TResult> instrument,
			IExpectation<TResult> expectation,
			TExpectationBuilder expectationBuilder,
			IExceptionFilter<TResult> exceptionFilter = null) where TSpecification : class, IChainableSpecification
			where TExpectationBuilder : class, IExpectationBuilder;

		public abstract ISpecification Clone(IDeadline deadline);
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
		private readonly IDeadline _deadline;
		private readonly IExceptionFilter<TResult> _exceptionFilter;
		private readonly TExpectationBuilder _expectationBuilder;

		public Specification([NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] IExpectation<TResult> expectation,
			[NotNull] TExpectationBuilder expectationBuilder,
			ISource<TSubject> source = null,
			string because = null,
			IExceptionFilter<TResult> exceptionFilter = null,
			IDeadline deadline = null)
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

		public override ISpecification Clone(IDeadline deadline)
		{
			return new Specification<TSubject, TResult, TExpectationBuilder>(Instrument,
				Expectation,
				_expectationBuilder,
				Source,
				Because,
				_exceptionFilter,
				deadline);
		}

		public IEvaluation<TSubject, TResult> Evaluate(TSubject subject, IDeadline deadline = null)
		{
			var source = new Source<TSubject>(subject);
			Evaluation.Factory<TSubject, TResult, IEvaluation<TSubject, TResult>> factory =
				(outcome, result, timedOut, errors) => UnboundFactory(outcome, result, timedOut, source, errors);
			return Evaluate(source, factory, deadline);
		}

		public IBoundEvaluation<TSubject, TResult> Evaluate(IDeadline deadline = null)
		{
			return Evaluate(Source, BoundFactory, deadline);
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

		private TEvaluation Evaluate<TEvaluation>(ISource<TSubject> source,
			Evaluation.Factory<TSubject, TResult, TEvaluation> evaluationFactory,
			IDeadline deadline = null) where TEvaluation : class, IEvaluation<TSubject, TResult>
		{
			IMeasurement<TResult> measurement = Instrument.Sample(source, deadline ?? _deadline);
			if (ExpectsException)
			{
				measurement = _exceptionFilter.Filter(measurement);
			}
			TEvaluation evaluation = Expectation.Evaluate(measurement, ExpectsException, evaluationFactory);
			return evaluation;
		}

		private IEvaluation<TSubject, TResult> UnboundFactory(Outcome outcome,
			TResult result,
			bool timedOut,
			ISource<TSubject> source,
			params IError[] errors)
		{
			return new Evaluation<TSubject, TResult>(this, outcome, result, timedOut, source, errors);
		}
	}
}
