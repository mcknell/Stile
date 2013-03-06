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
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IExpectation
	{
		[NotNull]
		IClause Clause { get; }
		[NotNull]
		ILazyDescriptionOfLambda Lambda { get; }
		string Accept([NotNull] IExpectationFormatVisitor visitor);
	}

	public interface IExpectation<TSubject, TResult> : IExpectation
	{
		TEvaluation Evaluate<TEvaluation>(IMeasurement<TSubject, TResult> measurement,
			bool expectedAnException,
			Evaluation.Factory<TSubject, TResult, TEvaluation> factory)
			where TEvaluation : class, IEvaluation<TSubject, TResult>;
	}

	public class Expectation<TSubject>
	{
		public static IEvaluation<TSubject> Evaluate(IObservation<TSubject> observation, bool expectedAnException)
		{
			int handledErrors = observation.Errors.Count(x => x.Handled);
			int allErrorsIfAny = observation.Errors.Length;

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
			return new Evaluation<TSubject>(observation, outcome);
		}

		public static Expectation<TSubject, TResult> From<TResult>(Expression<Predicate<TResult>> expression,
			Negated negated,
			IClause clause)
		{
			Func<Predicate<TResult>> compiler = Expectation<TSubject, TResult>.MakeCompiler(expression, negated);
			return new Expectation<TSubject, TResult>(compiler, new LazyDescriptionOfLambda(expression), clause);
		}
	}

	public class Expectation<TSubject, TResult> : Expectation<TSubject>,
		IExpectation<TSubject, TResult>
	{
		private readonly Lazy<Predicate<IMeasurement<TSubject, TResult>>> _lazyPredicate;

		public Expectation([NotNull] Expression<Predicate<TResult>> expression, [NotNull] IClause clause)
			: this(MakeCompiler(expression), new LazyDescriptionOfLambda(expression), clause) {}

		public Expectation([NotNull] Func<Predicate<TResult>> predicateSource,
			[NotNull] ILazyDescriptionOfLambda lambda,
			[NotNull] IClause clause)
		{
			Clause = clause;
			Lambda = lambda;
			Func<Predicate<TResult>> source = predicateSource.ValidateArgumentIsNotNull();
			_lazyPredicate =
				new Lazy<Predicate<IMeasurement<TSubject, TResult>>>(() => x => source.Invoke().Invoke(x.Value));
		}

		public IClause Clause { get; private set; }
		public ILazyDescriptionOfLambda Lambda { get; private set; }
		public static Expectation<TSubject, TResult> UnconditionalAcceptance
		{
			get { return new Expectation<TSubject, TResult>(result => true, SemanticModel.Clause.AlwaysTrue); }
		}

		public string Accept(IExpectationFormatVisitor visitor)
		{
			return visitor.Format(this);
		}

		public TEvaluation Evaluate<TEvaluation>(IMeasurement<TSubject, TResult> measurement,
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
			} else if (_lazyPredicate.Value.Invoke(measurement))
			{
				outcome = measurement.TaskStatus;
			} else
			{
				outcome = Outcome.Failed;
			}
			return factory.Invoke(measurement, outcome);
		}

		public static Func<Predicate<TResult>> MakeCompiler(Expression<Predicate<TResult>> expression)
		{
			var lazy = new Lazy<Predicate<TResult>>(expression.Compile);
			Predicate<TResult> predicate = x => lazy.Value.Invoke(x);
			Func<Predicate<TResult>> doubleFunc = () => predicate;
			return doubleFunc;
		}

		public static Func<Predicate<TResult>> MakeCompiler(Expression<Predicate<TResult>> expression,
			Negated negated)
		{
			var lazy = new Lazy<Predicate<TResult>>(expression.Compile);
			Predicate<TResult> predicate = x => negated.AgreesWith(lazy.Value.Invoke(x));
			Func<Predicate<TResult>> doubleFunc = () => predicate;
			return doubleFunc;
		}
	}
}
