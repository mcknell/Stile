#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
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

	public interface IEvaluation<in TSubject, out TResult> : IEvaluation<TResult>
	{
		IEvaluation<TResult> Evaluate(TSubject subject);
	}

	public class Evaluation : IEvaluation
	{
		public delegate TEvaluation Factory<in TSubject, in TResult, out TEvaluation>(
			Outcome outcome, TResult result, bool timedOut, params IError[] errors)
			where TEvaluation : class, IEvaluation<TSubject, TResult>;

		public delegate TEvaluation Factory<out TEvaluation>(Outcome outcome, bool timedOut, params IError[] errors)
			where TEvaluation : class, IEvaluation;

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
		IEvaluation<TSubject, TResult>
	{
		private readonly ISpecification<TSubject, TResult> _specification;

		public Evaluation([NotNull] ISpecification<TSubject, TResult> specification,
			Outcome outcome,
			TResult value,
			bool timedOut,
			params IError[] errors)
			: base(outcome, value, timedOut, errors)
		{
			_specification = specification.ValidateArgumentIsNotNull();
		}

		public IEvaluation<TResult> Evaluate(TSubject subject)
		{
			return _specification.Evaluate(subject);
		}
	}
}
