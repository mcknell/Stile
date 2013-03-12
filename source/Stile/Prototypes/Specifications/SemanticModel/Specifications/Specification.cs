#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Patterns.Structural.Hierarchy;
using Stile.Prototypes.Specifications.Builders.Lifecycle;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface ISpecification {}

	public interface ISpecification<in TSubject> : ISpecification {}

	public interface ISpecification<TSubject, TResult> : ISpecification<TSubject>,
		IHides<ISpecificationState<TSubject, TResult>>,
		IEvaluable<TSubject, TResult> {}

	public interface ISpecification<TSubject, TResult, out TExpectationBuilder> :
		ISpecification<TSubject, TResult>,
		IChainableSpecification<TExpectationBuilder>
		where TExpectationBuilder : class, IExpectationBuilder {}

	public interface ISpecificationState
	{
		[System.Diagnostics.Contracts.Pure]
		ISpecification Clone([CanBeNull] IDeadline deadline);
	}

	public interface ISpecificationState<out TSubject> : ISpecificationState
	{
		[CanBeNull]
		string Because { get; }
		[CanBeNull]
		IDeadline Deadline { get; }
		[NotNull]
		IAcceptSpecificationVisitors LastTerm { get; }
	}

	public interface ISpecificationState<TSubject, TResult> : ISpecificationState<TSubject>,
		IHasExpectation<TSubject, TResult>,
		IAcceptEvaluationVisitors
	{
		[CanBeNull]
		IExceptionFilter<TSubject, TResult> ExceptionFilter { get; }
		[CanBeNull]
		ISpecification<TSubject, TResult> Prior { get; }

		TEvaluation Evaluate<TEvaluation>(ISource<TSubject> source,
			Evaluation.Factory<TSubject, TResult, TEvaluation> evaluationFactory,
			IDeadline deadline = null) where TEvaluation : class, IEvaluation<TSubject, TResult>;
	}

	public abstract class Specification : ISpecificationState
	{
		public abstract ISpecification Clone(IDeadline deadline);
	}

	public abstract class Specification<TSubject, TExceptionFilter> : Specification,
		ISpecification<TSubject>
		where TExceptionFilter : class, IExceptionFilter<TSubject>
	{
		protected Specification([NotNull] IAcceptSpecificationVisitors lastTerm,
			[CanBeNull] TExceptionFilter exceptionFilter,
			[CanBeNull] IDeadline deadline,
			[CanBeNull] string because)
		{
			LastTerm = lastTerm.ValidateArgumentIsNotNull();
			ExceptionFilter = exceptionFilter;
			Deadline = deadline;
			Because = because;
		}

		public string Because { get; private set; }
		public IDeadline Deadline { get; private set; }
		public TExceptionFilter ExceptionFilter { get; private set; }
		public IAcceptSpecificationVisitors LastTerm { get; private set; }
	}

	public class Specification<TSubject, TResult, TExpectationBuilder> :
		Specification<TSubject, IExceptionFilter<TSubject, TResult>>,
		IBoundSpecification<TSubject, TResult, TExpectationBuilder>,
		ISpecificationState<TSubject, TResult>
		where TExpectationBuilder : class, IExpectationBuilder
	{
		private readonly IDeadline _deadline;
		private readonly TExpectationBuilder _expectationBuilder;
		private readonly IExpectationBuilderState _expectationBuilderState;

		[Rule]
		public Specification([NotNull] IExpectation<TSubject, TResult> expectation,
			[Symbol] [NotNull] TExpectationBuilder expectationBuilder,
			[NotNull] IAcceptSpecificationVisitors lastTerm,
			[CanBeNull] ISpecification<TSubject, TResult> prior,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null,
			[Symbol] IDeadline deadline = null,
			[Symbol] string because = null)
			: base(lastTerm, exceptionFilter, deadline, because)
		{
			Expectation = expectation.ValidateArgumentIsNotNull();
			_expectationBuilder = expectationBuilder.ValidateArgumentIsNotNull();
			_expectationBuilderState = expectationBuilder as IExpectationBuilderState;
			Prior = prior;
			if (_expectationBuilderState == null)
			{
				throw new ArgumentException(string.Format("Argument of type {0} must be convertible to {1}",
					expectationBuilder.GetType().Name,
					typeof(IExpectationBuilderState).Name));
			}
			_deadline = deadline;
		}

		public TExpectationBuilder AndThen
		{
			get { return (TExpectationBuilder) _expectationBuilderState.CloneFor(this); }
		}

		public IExpectation<TSubject, TResult> Expectation { get; private set; }
		public IAcceptSpecificationVisitors Parent
		{
			get { return null; }
		}
		public ISpecification<TSubject, TResult> Prior { get; private set; }
		public ISpecificationState<TSubject, TResult> Xray
		{
			get { return this; }
		}

		public override ISpecification Clone(IDeadline deadline)
		{
			return new Specification<TSubject, TResult, TExpectationBuilder>(Expectation,
				_expectationBuilder,
				LastTerm,
				Prior,
				ExceptionFilter,
				deadline,
				Because);
		}

		public IEvaluation<TSubject, TResult> Evaluate(ISource<TSubject> source, IDeadline deadline = null)
		{
			return Evaluate(source, UnboundFactory, deadline);
		}

		public IBoundEvaluation<TSubject, TResult> Evaluate(IDeadline deadline = null)
		{
			return Evaluate(Expectation.Xray.Instrument.Xray.Source, BoundFactory, deadline);
		}

		public void Accept(ISpecificationVisitor visitor)
		{
			visitor.Visit3(this);
		}

		public TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit3(this, data);
		}

		public void Accept(IEvaluationVisitor visitor)
		{
			visitor.Visit3(this);
		}

		public TEvaluation Evaluate<TEvaluation>(ISource<TSubject> source,
			Evaluation.Factory<TSubject, TResult, TEvaluation> evaluationFactory,
			IDeadline deadline = null) where TEvaluation : class, IEvaluation<TSubject, TResult>
		{
			if (Prior != null)
			{
				return Prior.Xray.Evaluate(source, evaluationFactory, deadline);
			}
			IMeasurement<TSubject, TResult> measurement = Expectation.Xray.Instrument.Measure(source,
				deadline ?? _deadline);
			if (ExpectsException)
			{
				measurement = ExceptionFilter.Filter(measurement);
			}
			TEvaluation evaluation = Expectation.Evaluate(measurement, ExpectsException, evaluationFactory);
			return evaluation;
		}

		IAcceptEvaluationVisitors IHasParent<IAcceptEvaluationVisitors>.Parent
		{
			get { return null; }
		}

		private bool ExpectsException
		{
			get { return ExceptionFilter != null; }
		}

		private IBoundEvaluation<TSubject, TResult> BoundFactory(IMeasurement<TSubject, TResult> measurement,
			Outcome outcome)
		{
			return new BoundEvaluation<TSubject, TResult>(this, measurement, outcome);
		}

		private IEvaluation<TSubject, TResult> UnboundFactory(IMeasurement<TSubject, TResult> measurement,
			Outcome outcome)
		{
			return new Evaluation<TSubject, TResult>(this, measurement, outcome);
		}
	}
}
