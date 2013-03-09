#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Patterns.Structural.Hierarchy;
using Stile.Prototypes.Specifications.Builders.Lifecycle;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
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
		ISpecification Clone(IDeadline deadline);
	}

	public interface ISpecificationState<out TSubject> : ISpecificationState
	{
		[CanBeNull]
		string Because { get; }

		[NotNull]
		IAcceptSpecificationVisitors LastTerm { get; }
	}

	public interface ISpecificationState<TSubject, TResult> : ISpecificationState<TSubject>,
		IHasExpectation<TSubject, TResult>,
		IAcceptEvaluationVisitors {}

	public abstract class Specification : ISpecificationState
	{
		public abstract ISpecification Clone(IDeadline deadline);
	}

	public abstract class Specification<TSubject> : Specification,
		ISpecification<TSubject>
	{
		protected Specification([CanBeNull] string because)
		{
			Because = because;
		}

		public string Because { get; private set; }
	}

	public class Specification<TSubject, TResult, TExpectationBuilder> : Specification<TSubject>,
		IBoundSpecification<TSubject, TResult, TExpectationBuilder>,
		ISpecificationState<TSubject, TResult>
		where TExpectationBuilder : class, IExpectationBuilder
	{
		private readonly IDeadline _deadline;
		private readonly IExceptionFilter<TSubject, TResult> _exceptionFilter;
		private readonly TExpectationBuilder _expectationBuilder;

		public Specification([NotNull] IExpectation<TSubject, TResult> expectation,
			[NotNull] TExpectationBuilder expectationBuilder,
			[NotNull] IAcceptSpecificationVisitors lastTerm,
			string because = null,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null,
			IDeadline deadline = null)
			: base(because)
		{
			Expectation = expectation.ValidateArgumentIsNotNull();
			_expectationBuilder = expectationBuilder.ValidateArgumentIsNotNull();
			LastTerm = lastTerm.ValidateArgumentIsNotNull();
			_exceptionFilter = exceptionFilter;
			_deadline = deadline;
		}

		public TExpectationBuilder AndThen
		{
			get { return _expectationBuilder; }
		}

		public IExpectation<TSubject, TResult> Expectation { get; private set; }
		public IAcceptSpecificationVisitors LastTerm { get; private set; }
		public IAcceptSpecificationVisitors Parent
		{
			get { return null; }
		}
		public ISpecificationState<TSubject, TResult> Xray
		{
			get { return this; }
		}

		public override ISpecification Clone(IDeadline deadline)
		{
			return new Specification<TSubject, TResult, TExpectationBuilder>(Expectation,
				_expectationBuilder,
				LastTerm,
				Because,
				_exceptionFilter,
				deadline);
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

		IAcceptEvaluationVisitors IHasParent<IAcceptEvaluationVisitors>.Parent
		{
			get { return null; }
		}

		private bool ExpectsException
		{
			get { return _exceptionFilter != null; }
		}

		private IBoundEvaluation<TSubject, TResult> BoundFactory(IMeasurement<TSubject, TResult> measurement,
			Outcome outcome)
		{
			return new BoundEvaluation<TSubject, TResult>(this, measurement, outcome);
		}

		private TEvaluation Evaluate<TEvaluation>(ISource<TSubject> source,
			Evaluation.Factory<TSubject, TResult, TEvaluation> evaluationFactory,
			IDeadline deadline = null) where TEvaluation : class, IEvaluation<TSubject, TResult>
		{
			IMeasurement<TSubject, TResult> measurement = Expectation.Xray.Instrument.Measure(source,
				deadline ?? _deadline);
			if (ExpectsException)
			{
				measurement = _exceptionFilter.Filter(measurement);
			}
			TEvaluation evaluation = Expectation.Evaluate(measurement, ExpectsException, evaluationFactory);
			return evaluation;
		}

		private IEvaluation<TSubject, TResult> UnboundFactory(IMeasurement<TSubject, TResult> measurement,
			Outcome outcome)
		{
			return new Evaluation<TSubject, TResult>(this, measurement, outcome);
		}
	}
}
