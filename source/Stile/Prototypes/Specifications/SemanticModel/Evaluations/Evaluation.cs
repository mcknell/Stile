#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
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

	public class Evaluation<TSubject, TResult> : Evaluation<TSubject>,
		IEvaluation<TSubject, TResult>,
		IEvaluationState<TSubject, TResult>
	{
		public Evaluation([NotNull] ISpecification<TSubject, TResult> specification,
			[NotNull] IMeasurement<TSubject, TResult> measurement,
			Outcome outcome,
			[CanBeNull] IEvaluation<TSubject, TResult> prior,
			[NotNull] ISpecificationState<TSubject, TResult> tailSpecification)
			: base(measurement, outcome)
		{
			Specification = specification.ValidateArgumentIsNotNull();
			Measurement = measurement.ValidateArgumentIsNotNull();
			Value = Measurement.Value;
			Prior = prior;
			TailSpecification = tailSpecification.ValidateArgumentIsNotNull();
		}

		public IMeasurement<TSubject, TResult> Measurement { get; private set; }
		public IAcceptEvaluationVisitors Parent
		{
			get { return Specification.Xray; }
		}

		public IEvaluation<TSubject, TResult> Prior { get; private set; }
		public ISpecification<TSubject, TResult> Specification { get; private set; }
		public ISpecificationState<TSubject, TResult> TailSpecification { get; private set; }

		public TResult Value { get; private set; }
		public IEvaluationState<TSubject, TResult> Xray
		{
			get { return this; }
		}

		public IEvaluation<TSubject, TResult> EvaluateNext(IDeadline deadline = null)
		{
			return EvaluateNextWith(Measurement.Sample.Source, deadline);
		}

		public IEvaluation<TSubject, TResult> EvaluateNextWith(ISource<TSubject> source, IDeadline deadline = null)
		{
			ISpecification<TSubject, TResult> specification = GetNextSpecification();
			if (specification == null)
			{
				return null;
			}
			IEvaluation<TSubject, TResult> evaluation = specification.Xray.Evaluate(source,
				this,
				TailSpecification,
				deadline);
			return evaluation;
		}

		public IEvaluation<TSubject, TResult> ReEvaluate(IDeadline deadline = null)
		{
			return Specification.Xray.Evaluate(Measurement.Sample.Source, Prior, TailSpecification, deadline);
		}

		public void Accept(IEvaluationVisitor visitor)
		{
			visitor.Visit2(this);
		}

		public TData Accept<TData>(IEvaluationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit2(this, data);
		}

		public IEnumerable<IEvaluation<TSubject, TResult>> Predecessors()
		{
			IEvaluation<TSubject, TResult> prior = this;
			while (prior.Xray.Prior != null)
			{
				prior = prior.Xray.Prior;
				yield return prior;
			}
		}

		[CanBeNull]
		private ISpecification<TSubject, TResult> GetNextSpecification()
		{
			List<IEvaluation<TSubject, TResult>> list = Predecessors().Reverse().ToList();
			int distanceOfNextFromRootSpec = list.Count + 1;
			ISpecification<TSubject, TResult> specification =
				TailSpecification.GetPredecessors(true).Reverse().Skip(distanceOfNextFromRootSpec).FirstOrDefault();
			return specification;
		}
	}
}
