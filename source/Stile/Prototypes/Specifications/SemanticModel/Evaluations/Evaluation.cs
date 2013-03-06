#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.Builders.Lifecycle;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Evaluations
{
	public interface IEvaluation
	{
		[System.Diagnostics.Contracts.Pure]
		IError[] Errors { get; }
		[System.Diagnostics.Contracts.Pure]
		Outcome Outcome { get; }
		bool TimedOut { get; }
	}

	public interface IEvaluation<out TSubject> : IEvaluation
	{
		ISample<TSubject> Sample { get; }
	}

	public interface IEvaluation<TSubject, TResult> : IEvaluation<TSubject>,
		IEvaluable<TSubject, TResult>,
		IHides<IEvaluationState<TSubject, TResult>>
	{
		TResult Value { get; }
	}

	public interface IEvaluationState<TSubject, TResult> : IHasSpecification<TSubject, TResult> {}

	public abstract class Evaluation : IEvaluation
	{
		public delegate TEvaluation Factory<in TSubject, in TResult, out TEvaluation>(
			IMeasurement<TSubject, TResult> sample, Outcome outcome)
			where TEvaluation : class, IEvaluation<TSubject, TResult>;

		protected Evaluation(IObservation observation, Outcome outcome)
		{
			Errors = observation.ValidateArgumentIsNotNull().Errors;
			Outcome = outcome;
			TimedOut = observation.TimedOut;
			if (TimedOut)
			{
				Outcome = Outcome.Incomplete;
			}
		}

		public IError[] Errors { get; private set; }
		public Outcome Outcome { get; private set; }
		public bool TimedOut { get; private set; }
	}

	public class Evaluation<TSubject> : Evaluation,
		IEvaluation<TSubject>
	{
		public Evaluation(IObservation<TSubject> observation, Outcome outcome)
			: base(observation, outcome)
		{
			Sample = observation.Sample;
		}

		public ISample<TSubject> Sample { get; private set; }
	}

	public class Evaluation<TSubject, TResult> : Evaluation<TSubject>,
		IEvaluation<TSubject, TResult>,
		IEvaluationState<TSubject, TResult>
	{
		public Evaluation([NotNull] ISpecification<TSubject, TResult> specification,
			[NotNull] IMeasurement<TSubject, TResult> measurement,
			Outcome outcome)
			: base(measurement, outcome)
		{
			Specification = specification.ValidateArgumentIsNotNull();
			IMeasurement<TSubject, TResult> validMeasurement = measurement.ValidateArgumentIsNotNull();
			Value = validMeasurement.Value;
			ISpecificationState<TSubject, TResult> xray = Specification.Xray;
			Expectation = xray.Expectation.ValidateArgumentIsNotNull();
			Instrument = xray.Instrument.ValidateArgumentIsNotNull();
			Source = xray.Source;
		}

		public IExpectation<TSubject, TResult> Expectation { get; private set; }
		public IInstrument<TSubject, TResult> Instrument { get; private set; }
		public ISource<TSubject> Source { get; private set; }
		public ISpecification<TSubject, TResult> Specification { get; private set; }
		public TResult Value { get; private set; }
		public IEvaluationState<TSubject, TResult> Xray
		{
			get { return this; }
		}

		public IEvaluation<TSubject, TResult> Evaluate(ISource<TSubject> source, IDeadline deadline = null)
		{
			return Specification.Evaluate(source, deadline);
		}

		public void Accept(IDescriptionVisitor visitor)
		{
			visitor.DescribeOverload2(this);
		}
	}
}
