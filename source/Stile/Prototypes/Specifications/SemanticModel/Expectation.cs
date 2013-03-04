#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IExpectation<TResult>
	{
		Lazy<string> Description { get; }

		TEvaluation Evaluate<TSubject, TEvaluation>(IMeasurement<TResult> measurement,
			bool expectedAnException,
			Evaluation.Factory<TSubject, TResult, TEvaluation> factory)
			where TEvaluation : class, IEvaluation<TSubject, TResult>;
	}

	public class Expectation
	{
		public static IEvaluation Evaluate(IObservation measurement, bool expectedAnException)
		{
			int handledErrors = measurement.Errors.Count(x => x.Handled);
			int allErrorsIfAny = measurement.Errors.Length;

			Outcome outcome;
			if (handledErrors < allErrorsIfAny)
			{
				outcome = Outcome.Failed;
			} else if (expectedAnException && handledErrors == 0)
			{
				outcome = Outcome.Failed;
			} else if (handledErrors == allErrorsIfAny && handledErrors > 0)
			{
				outcome = Outcome.Succeeded;
			} else
			{
				outcome = Outcome.Failed;
			}
			return new Evaluation(outcome, measurement.TimedOut, measurement.Errors);
		}
	}

	public class Expectation<TResult> : Expectation,
		IExpectation<TResult>
	{
		private readonly Lazy<Func<TResult, Outcome>> _lazyPredicate;

		public Expectation([NotNull] Expression<Func<TResult, Outcome>> expression)
			: this(expression.Compile, expression.ToLazyDebugString()) {}

		private Expectation([NotNull] Func<Func<TResult, Outcome>> predicateSource,
			[NotNull] Lazy<string> description)
		{
			Func<Func<TResult, Outcome>> source = predicateSource.ValidateArgumentIsNotNull();
			_lazyPredicate = new Lazy<Func<TResult, Outcome>>(source);
			Description = description.ValidateArgumentIsNotNull();
		}

		public Lazy<string> Description { get; private set; }

		public static Expectation<TResult> UnconditionalAcceptance
		{
			get { return new Expectation<TResult>(result => Outcome.Succeeded); }
		}

		public TEvaluation Evaluate<TSubject, TEvaluation>(IMeasurement<TResult> measurement,
			bool expectedAnException,
			Evaluation.Factory<TSubject, TResult, TEvaluation> factory)
			where TEvaluation : class, IEvaluation<TSubject, TResult>
		{
			int handledErrors = measurement.Errors.Count(x => x.Handled);
			int allErrorsIfAny = measurement.Errors.Length;

			Outcome outcome;
			if (handledErrors < allErrorsIfAny)
			{
				outcome = Outcome.Failed;
			} else if (expectedAnException && handledErrors == 0)
			{
				outcome = Outcome.Failed;
			} else if (handledErrors == allErrorsIfAny && handledErrors > 0)
			{
				outcome = Outcome.Succeeded;
			} else if (_lazyPredicate.Value.Invoke(measurement.Value))
			{
				outcome = measurement.TaskStatus;
			} else
			{
				outcome = Outcome.Failed;
			}
			return factory.Invoke(outcome, measurement.Value, measurement.TimedOut, measurement.Errors);
		}
	}
}
