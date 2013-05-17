#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Patterns.Structural.Hierarchy;
using Stile.Prototypes.Specifications.Builders.Lifecycle;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Types.Primitives;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface ISpecification {}

	/// <summary>
	/// Might look useless, but is a base interface for <see cref="IFaultSpecification{TSubject}"/>
	/// </summary>
	/// <typeparam name="TSubject"></typeparam>
	public interface ISpecification<in TSubject> : IChainableSpecification {}

	public interface ISpecification<TSubject, TResult> : ISpecification<TSubject>,
		IHides<ISpecificationState<TSubject, TResult>>,
		IEvaluable<TSubject, TResult> {}

	public interface ISpecification<TSubject, TResult, out TExpectationBuilder> :
		ISpecification<TSubject, TResult>,
		IChainableSpecification<TExpectationBuilder>
		where TExpectationBuilder : class, IExpectationBuilder
	{
		ISpecification<TSubject, TResult, TExpectationBuilder> Because(string reason);
	}

	public interface ISpecificationState
	{
		[System.Diagnostics.Contracts.Pure]
		ISpecification Clone([NotNull] IDeadline deadline);

		[System.Diagnostics.Contracts.Pure]
		ISpecification Clone([NotNull] string reason);
	}

	public interface ISpecificationState<out TSubject> : ISpecificationState,
		IAcceptSpecificationVisitors,
		IAcceptEvaluationVisitors
	{
		[CanBeNull]
		IDeadline Deadline { get; }
		[NotNull]
		IAcceptSpecificationVisitors LastTerm { get; }
		[CanBeNull]
		string Reason { get; }
	}

	public interface ISpecificationState<TSubject, TResult> : ISpecificationState<TSubject>,
		IHasExpectation<TSubject, TResult>,
		ICanGetPredecessors<ISpecification<TSubject, TResult>>
	{
		[CanBeNull]
		IExceptionFilter<TSubject, TResult> ExceptionFilter { get; }
		[CanBeNull]
		ISpecification<TSubject, TResult> Prior { get; }

		[NotNull]
		IEvaluation<TSubject, TResult> Evaluate([NotNull] ISource<TSubject> source,
			[CanBeNull] IEvaluation<TSubject, TResult> priorEvaluation,
			[NotNull] ISpecificationState<TSubject, TResult> tailSpecification,
			IDeadline deadline = null);
	}

	public abstract class Specification : ISpecificationState
	{
		public abstract ISpecification Clone(IDeadline deadline);
		public abstract ISpecification Clone(string reason);
	}

	public abstract class Specification<TSubject, TExceptionFilter> : Specification,
		ISpecification<TSubject>
		where TExceptionFilter : class, IExceptionFilter<TSubject>
	{
		protected Specification([NotNull] IAcceptSpecificationVisitors lastTerm,
			[CanBeNull] TExceptionFilter exceptionFilter,
			[CanBeNull] IDeadline deadline,
			[CanBeNull] string reason)
		{
			LastTerm = lastTerm.ValidateArgumentIsNotNull();
			ExceptionFilter = exceptionFilter;
			Deadline = deadline;
			Reason = reason;
		}

		public IDeadline Deadline { get; private set; }
		public TExceptionFilter ExceptionFilter { get; private set; }
		public IAcceptSpecificationVisitors LastTerm { get; private set; }
		public string Reason { get; private set; }
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
		private readonly IInstrument<TSubject, TResult> _instrument;
		private readonly ISource<TSubject> _source;

		public Specification([NotNull] IExpectation<TSubject, TResult> expectation,
			[NotNull] TExpectationBuilder expectationBuilder,
			[NotNull] IAcceptSpecificationVisitors lastTerm,
			[CanBeNull] ISpecification<TSubject, TResult> prior,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null,
			IDeadline deadline = null,
			string reason = null)
			: this(
				expectation.Xray.Instrument.Xray.Source,
				expectation.Xray.Instrument,
				expectation,
				expectationBuilder,
				lastTerm,
				prior,
				exceptionFilter,
				deadline,
				reason) {}

		[Rule(StartsGrammar = true)]
		private Specification([Symbol] [CanBeNull] ISource<TSubject> source,
			[Symbol] [NotNull] IInstrument<TSubject, TResult> instrument,
			[Symbol] [NotNull] IExpectation<TSubject, TResult> expectation,
			[NotNull] TExpectationBuilder expectationBuilder,
			[NotNull] IAcceptSpecificationVisitors lastTerm,
			[CanBeNull] ISpecification<TSubject, TResult> prior,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null,
			[Symbol] IDeadline deadline = null,
			[Symbol] string reason = null)
			: base(lastTerm, exceptionFilter, deadline, reason)
		{
			_source = source;
			_instrument = instrument;
			Expectation = expectation.ValidateArgumentIsNotNull();
			_expectationBuilder = expectationBuilder.ValidateArgumentIsNotNull();
			_expectationBuilderState = expectationBuilder as IExpectationBuilderState;
			Prior = prior;
			if (_expectationBuilderState == null)
			{
				throw new ArgumentException(
					ErrorMessages.Specification_ExpectationBuilderState.InvariantFormat(expectationBuilder.GetType().Name,
						typeof(IExpectationBuilderState).Name));
			}
			_deadline = deadline;
		}

		public TExpectationBuilder AndThen
		{
			get { return (TExpectationBuilder) _expectationBuilderState.ChainFrom(this); }
		}

		public IExpectation<TSubject, TResult> Expectation { get; private set; }
		public IAcceptSpecificationVisitors Parent
		{
			get { return LastTerm; }
		}

		public ISpecification<TSubject, TResult> Prior { get; private set; }
		public ISpecificationState<TSubject, TResult> Xray
		{
			get { return this; }
		}

		public ISpecification<TSubject, TResult, TExpectationBuilder> Because(string reason)
		{
			return (ISpecification<TSubject, TResult, TExpectationBuilder>) Clone(reason);
		}

		IBoundSpecification<TSubject, TResult, TExpectationBuilder>
			IBoundSpecification<TSubject, TResult, TExpectationBuilder>.Because(string reason)
		{
			return (IBoundSpecification<TSubject, TResult, TExpectationBuilder>) Clone(reason);
		}

		public IEvaluation<TSubject, TResult> Evaluate(ISource<TSubject> source, IDeadline deadline = null)
		{
			ISpecification<TSubject, TResult> specification = GetPredecessors().LastOrDefault() ?? this;
			return specification.Xray.Evaluate(source, null, this, deadline);
		}

		public IEvaluation<TSubject, TResult> Evaluate(IDeadline deadline = null)
		{
			ISpecification<TSubject, TResult> specification = GetPredecessors().LastOrDefault() ?? this;
			return specification.Xray.Evaluate(_source, null, this, deadline);
		}

		public TData Accept<TData>(IEvaluationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit3(this, data);
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

		public override ISpecification Clone(IDeadline deadline)
		{
			IDeadline validated = deadline.ValidateArgumentIsNotNull();
			return new Specification<TSubject, TResult, TExpectationBuilder>(Expectation,
				_expectationBuilder,
				LastTerm,
				Prior,
				ExceptionFilter,
				validated,
				Reason);
		}

		public override ISpecification Clone(string reason)
		{
			return new Specification<TSubject, TResult, TExpectationBuilder>(Expectation,
				_expectationBuilder,
				LastTerm,
				Prior,
				ExceptionFilter,
				_deadline,
				reason);
		}

		public IEvaluation<TSubject, TResult> Evaluate(ISource<TSubject> source,
			IEvaluation<TSubject, TResult> priorEvaluation,
			ISpecificationState<TSubject, TResult> tailSpecification,
			IDeadline deadline = null)
		{
			IMeasurement<TSubject, TResult> measurement = _instrument.Measure(source, deadline ?? _deadline);
			if (ExpectsException)
			{
				measurement = ExceptionFilter.Filter(measurement);
			}
			Outcome outcome = Expectation.Evaluate(measurement, ExpectsException);
			var evaluation = new Evaluation<TSubject, TResult>(this,
				measurement,
				outcome,
				priorEvaluation,
				tailSpecification);
			return evaluation;
		}

		public IEnumerable<ISpecification<TSubject, TResult>> GetPredecessors(bool includeSelf = false)
		{
			ISpecification<TSubject, TResult> prior = this;
			if (includeSelf)
			{
				yield return prior;
			}
			while (prior.Xray.Prior != null)
			{
				prior = prior.Xray.Prior;
				yield return prior;
			}
		}

		IAcceptEvaluationVisitors IHasParent<IAcceptEvaluationVisitors>.Parent
		{
			get { return null; }
		}
		private bool ExpectsException
		{
			get { return ExceptionFilter != null; }
		}
	}
}
