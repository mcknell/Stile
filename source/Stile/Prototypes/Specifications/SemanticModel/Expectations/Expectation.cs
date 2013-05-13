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
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.Builders.Lifecycle;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Expectations
{
	public interface IExpectation : IAcceptSpecificationVisitors
	{
		void Accept([NotNull] IExpectationVisitor visitor);
		TData Accept<TData>([NotNull] IExpectationVisitor<TData> visitor, TData data = default(TData));
	}

	public interface IExpectation<TSubject, TResult> : IExpectation,
		IHides<IExpectationState<TSubject, TResult>>
	{
		Outcome Evaluate(IMeasurement<TSubject, TResult> measurement, bool expectedAnException);
	}

	public interface IExpectationState<TSubject, out TResult> : IHasInstrument<TSubject, TResult>,
		IAcceptSpecificationVisitors
	{
		[NotNull]
		IAcceptExpectationVisitors LastTerm { get; }
	}

	public abstract class Expectation<TSubject>
	{
		public static Expectation<TSubject, TResult> From<TResult>(Expression<Predicate<TResult>> expression,
			Negated negated,
			[NotNull] IInstrument<TSubject, TResult> instrument,
			IAcceptExpectationVisitors lastTerm)
		{
			return new Expectation<TSubject, TResult>(instrument, expression, lastTerm, negated);
		}
	}

	public class Expectation<TSubject, TResult> : Expectation<TSubject>,
		IExpectation<TSubject, TResult>,
		IExpectationState<TSubject, TResult>
	{
		private readonly Lazy<Predicate<IMeasurement<TSubject, TResult>>> _lazyPredicate;

		public Expectation([NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] Expression<Predicate<TResult>> expression,
			[NotNull] IAcceptExpectationVisitors lastTerm,
			Negated negated)
			: this(instrument, expression.Compile, lastTerm, negated) {}

		public Expectation([NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] Predicate<TResult> predicate,
			Negated negated,
			[NotNull] IAcceptExpectationVisitors lastTerm)
			: this(instrument, () => predicate, lastTerm, negated) {}

		public Expectation([NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] Func<Predicate<TResult>> predicateFactory,
			[NotNull] IAcceptExpectationVisitors lastTerm,
			Negated negated)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			LastTerm = lastTerm.ValidateArgumentIsNotNull();
			Func<Predicate<TResult>> factory = predicateFactory.ValidateArgumentIsNotNull();
			_lazyPredicate = MakeLazyPredicate(factory, negated);
		}

		public IInstrument<TSubject, TResult> Instrument { get; private set; }
		public IAcceptExpectationVisitors LastTerm { get; private set; }

		public IAcceptSpecificationVisitors Parent
		{
			get { return Instrument; }
		}

		public IExpectationState<TSubject, TResult> Xray
		{
			get { return this; }
		}

		public void Accept(IExpectationVisitor visitor)
		{
			visitor.Visit2(this);
		}

		public TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit2(this, data);
		}

		public void Accept(ISpecificationVisitor visitor)
		{
			visitor.Visit2(this);
		}

		public TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit2(this, data);
		}

		public Outcome Evaluate(IMeasurement<TSubject, TResult> measurement, bool expectedAnException)
		{
			int handledErrors = measurement.Errors.Count(x => x.Handled);
			int allErrorsIfAny = measurement.Errors.Count;

			Outcome outcome;
			if (handledErrors < allErrorsIfAny)
			{
				outcome = Outcome.Failed;
			}
			else if (expectedAnException && handledErrors == 0)
			{
				outcome = Outcome.Failed;
			}
			else if (handledErrors == allErrorsIfAny && handledErrors > 0)
			{
				outcome = Outcome.Succeeded;
			}
			else if (_lazyPredicate.Value.Invoke(measurement))
			{
				outcome = measurement.TaskStatus;
			}
			else
			{
				outcome = Outcome.Failed;
			}
			return outcome;
		}

		private static Lazy<Predicate<IMeasurement<TSubject, TResult>>> MakeLazyPredicate(
			Func<Predicate<TResult>> factory, Negated negated)
		{
			return new Lazy<Predicate<IMeasurement<TSubject, TResult>>>(() => MakeNegatablePredicate(factory, negated));
		}

		private static Predicate<IMeasurement<TSubject, TResult>> MakeNegatablePredicate(
			Func<Predicate<TResult>> factory, Negated negated)
		{
			Predicate<TResult> compiled = factory.Invoke(); // possibly compiling an expression
			Predicate<IMeasurement<TSubject, TResult>> predicate = x => negated.AgreesWith(compiled.Invoke(x.Value));
			return predicate;
		}
	}
}
