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
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Evaluations
{
	public interface IEvaluation
	{
		[System.Diagnostics.Contracts.Pure]
		IReadOnlyList<IError> Errors { get; }
		[System.Diagnostics.Contracts.Pure]
		Outcome Outcome { get; }
		bool TimedOut { get; }
	}

	public interface IEvaluation<TSubject> : IEvaluation
	{
		[CanBeNull]
		ISample<TSubject> Sample { get; }
	}

	public interface IEvaluation<TSubject, TResult> : IEvaluation<TSubject>,
		IHides<IEvaluationState<TSubject, TResult>>
	{
		TResult Value { get; }

		[CanBeNull]
		[System.Diagnostics.Contracts.Pure]
		IEvaluation<TSubject, TResult> EvaluateNext(IDeadline deadline = null);

		[CanBeNull]
		[System.Diagnostics.Contracts.Pure]
		IEvaluation<TSubject, TResult> EvaluateNextWith([NotNull] ISource<TSubject> source,
			IDeadline deadline = null);

		[NotNull]
		[System.Diagnostics.Contracts.Pure]
		IEvaluation<TSubject, TResult> ReEvaluate(IDeadline deadline = null);
	}

	public interface IEvaluationState<TSubject, TResult> : IHasSpecification<TSubject, TResult>,
		IAcceptEvaluationVisitors
	{
		[CanBeNull]
		IEvaluation<TSubject, TResult> Prior { get; }
		ISpecificationState<TSubject, TResult> TailSpecification { get; }
		IEnumerable<IEvaluation<TSubject, TResult>> Predecessors();
	}

	public abstract class Evaluation : IEvaluation
	{
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

		public IReadOnlyList<IError> Errors { get; private set; }
		public Outcome Outcome { get; private set; }
		public bool TimedOut { get; private set; }
	}

	public abstract class Evaluation<TSubject> : Evaluation,
		IEvaluation<TSubject>
	{
		protected Evaluation(IObservation<TSubject> observation, Outcome outcome)
			: base(observation, outcome)
		{
			Sample = observation.Sample;
		}

		public ISample<TSubject> Sample { get; private set; }
	}

	public abstract class Evaluation<TSpecification, TSubject, TState, TEvaluation> : Evaluation<TSubject>,
		IAcceptEvaluationVisitors
		where TSpecification : class, ISpecification<TSubject>, IHides<TState>
		where TState : class, IAcceptEvaluationVisitors, ICanGetPredecessors<TSpecification>
		where TEvaluation : class
	{
		protected Evaluation(IObservation<TSubject> observation,
			Outcome outcome,
			TSpecification specification,
			TEvaluation prior,
			TState tailSpecification)
			: base(observation, outcome)
		{
			TailSpecification = tailSpecification.ValidateArgumentIsNotNull();
			Prior = prior;
			Specification = specification.ValidateArgumentIsNotNull();
		}

		public IAcceptEvaluationVisitors Parent
		{
			get { return Specification.Xray; }
		}

		public TEvaluation Prior { get; private set; }
		public TSpecification Specification { get; private set; }
		public TState TailSpecification { get; private set; }
		public abstract void Accept(IEvaluationVisitor visitor);
		public abstract TData Accept<TData>(IEvaluationVisitor<TData> visitor, TData data);

		public TEvaluation EvaluateNext(IDeadline deadline = null)
		{
			return EvaluateNextWith(Sample.Source, deadline);
		}

		public TEvaluation EvaluateNextWith(ISource<TSubject> source, IDeadline deadline = null)
		{
			TSpecification specification = GetNextSpecification();
			if (specification == null)
			{
				return null;
			}
			Evaluator evaluator = GetEvaluator.Invoke(specification.Xray);
			TEvaluation evaluation = evaluator.Invoke(source, this as TEvaluation, TailSpecification, deadline);
			return evaluation;
		}

		public abstract IEnumerable<TEvaluation> Predecessors();

		public TEvaluation ReEvaluate(IDeadline deadline = null)
		{
			return Evaluate(Sample.Source, Prior, TailSpecification, deadline);
		}

		protected abstract Evaluator Evaluate { get; }
		protected abstract Func<TState, Evaluator> GetEvaluator { get; }

		[CanBeNull]
		private TSpecification GetNextSpecification()
		{
			List<TEvaluation> list = Predecessors().Reverse().ToList();
			int distanceOfNextFromRootSpec = list.Count + 1;
			TSpecification specification =
				TailSpecification.GetPredecessors(true).Reverse().Skip(distanceOfNextFromRootSpec).FirstOrDefault();
			return specification;
		}

		protected delegate TEvaluation Evaluator(
			ISource<TSubject> source, TEvaluation evaluation, TState state, IDeadline deadline);
	}

	public class Evaluation<TSubject, TResult> :
		Evaluation
			<ISpecification<TSubject, TResult>, TSubject, ISpecificationState<TSubject, TResult>,
				IEvaluation<TSubject, TResult>>,
		IEvaluation<TSubject, TResult>,
		IEvaluationState<TSubject, TResult>
	{
		public Evaluation([NotNull] ISpecification<TSubject, TResult> specification,
			[NotNull] IMeasurement<TSubject, TResult> measurement,
			Outcome outcome,
			[CanBeNull] IEvaluation<TSubject, TResult> prior,
			[NotNull] ISpecificationState<TSubject, TResult> tailSpecification)
			: base(measurement, outcome, specification, prior, tailSpecification)
		{
			Value = measurement.ValidateArgumentIsNotNull().Value;
		}

		public TResult Value { get; private set; }
		public IEvaluationState<TSubject, TResult> Xray
		{
			get { return this; }
		}

		public override void Accept(IEvaluationVisitor visitor)
		{
			visitor.Visit2(this);
		}

		public override TData Accept<TData>(IEvaluationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit2(this, data);
		}

		public override IEnumerable<IEvaluation<TSubject, TResult>> Predecessors()
		{
			IEvaluation<TSubject, TResult> prior = this;
			while (prior.Xray.Prior != null)
			{
				prior = prior.Xray.Prior;
				yield return prior;
			}
		}

		protected override Evaluator Evaluate
		{
			get { return Specification.Xray.Evaluate; }
		}
		protected override Func<ISpecificationState<TSubject, TResult>, Evaluator> GetEvaluator
		{
			get { return x => x.Evaluate; }
		}
	}
}
