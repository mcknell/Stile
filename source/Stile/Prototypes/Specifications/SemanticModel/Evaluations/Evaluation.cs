#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.Builders.Lifecycle;
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

	public interface IEvaluation<out TResult> : IEvaluation
	{
		TResult Value { get; }
	}

	public interface IEvaluation<TSubject, TResult> : IEvaluation<TResult>,
		IEvaluable<TSubject, TResult, IEvaluation<TSubject, TResult>>,
		IHides<IEvaluationState<TSubject, TResult>> {}

	public interface IEvaluationState<TSubject, TResult> : IHasSpecification<TSubject, TResult> {}

	public class Evaluation : IEvaluation
	{
		public delegate TEvaluation Factory<in TSubject, in TResult, out TEvaluation>(
			Outcome outcome, TResult result, bool timedOut, params IError[] errors)
			where TEvaluation : class, IEvaluation<TSubject, TResult>;

		public Evaluation(Outcome outcome, Exception handledExpectedException)
			: this(outcome, false, handledExpectedException) {}

		public Evaluation(Outcome outcome, bool timedOut, Exception handledExpectedException)
			: this(outcome, timedOut, new Error(handledExpectedException, true)) {}

		public Evaluation(Outcome outcome, bool timedOut, params IError[] errors)
		{
			Errors = errors;
			Outcome = outcome;
			TimedOut = timedOut;
			if (TimedOut)
			{
				Outcome = Outcome.Incomplete;
			}
		}

		public IError[] Errors { get; private set; }
		public Outcome Outcome { get; private set; }
		public bool TimedOut { get; private set; }
	}

	public class Evaluation<TResult> : Evaluation,
		IEvaluation<TResult>
	{
		public Evaluation(Outcome outcome, TResult value, bool timedOut, params IError[] errors)
			: base(outcome, timedOut, errors)
		{
			Value = value;
		}

		public TResult Value { get; private set; }
	}

	public class Evaluation<TSubject, TResult> : Evaluation<TResult>,
		IEvaluation<TSubject, TResult>,
		IEvaluationState<TSubject, TResult>
	{
		public Evaluation([NotNull] ISpecification<TSubject, TResult> specification,
			Outcome outcome,
			TResult value,
			bool timedOut,
			[NotNull] ISource<TSubject> source,
			params IError[] errors)
			: base(outcome, value, timedOut, errors)
		{
			Specification = specification.ValidateArgumentIsNotNull();
			ISpecificationState<TSubject, TResult> xray = Specification.Xray;
			Expectation = xray.Expectation.ValidateArgumentIsNotNull();
			Instrument = xray.Instrument.ValidateArgumentIsNotNull();
			Source = source.ValidateArgumentIsNotNull();
		}

		public IExpectation<TResult> Expectation { get; private set; }
		public IInstrument<TSubject, TResult> Instrument { get; private set; }
		public ISource<TSubject> Source { get; private set; }
		public ISpecification<TSubject, TResult> Specification { get; private set; }
		public IEvaluationState<TSubject, TResult> Xray
		{
			get { return this; }
		}

		public IEvaluation<TSubject, TResult> Evaluate(TSubject subject, IDeadline deadline = null)
		{
			return Specification.Evaluate(subject, deadline);
		}
	}
}
