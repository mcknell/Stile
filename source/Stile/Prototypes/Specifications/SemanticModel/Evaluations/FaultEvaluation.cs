#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.Builders.Lifecycle;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Evaluations
{
	public interface IFaultEvaluation<TSubject> : IEvaluation<TSubject>,
		IHides<IFaultEvaluationState<TSubject>>
	{
		[CanBeNull]
		[System.Diagnostics.Contracts.Pure]
		IFaultEvaluation<TSubject> EvaluateNext(IDeadline deadline = null);

		[CanBeNull]
		[System.Diagnostics.Contracts.Pure]
		IFaultEvaluation<TSubject> EvaluateNextWith([NotNull] ISource<TSubject> source, IDeadline deadline = null);
	}

	public interface IFaultEvaluationState<TSubject> : IHasFaultSpecification<TSubject>,
		IAcceptEvaluationVisitors
	{
		[NotNull]
		IObservation<TSubject> Observation { get; }
		[CanBeNull]
		IFaultEvaluation<TSubject> Prior { get; }
		[NotNull]
		IFaultSpecificationState<TSubject> TailSpecification { get; }

		[NotNull]
		IEnumerable<IFaultEvaluation<TSubject>> Predecessors();
	}

	public static class FaultEvaluation
	{
		public static IFaultEvaluation<TSubject> Evaluate<TSubject>(
			[NotNull] this IFaultSpecification<TSubject> specification,
			[CanBeNull] TSubject subject,
			IDeadline deadline = null)
		{
			return specification.Evaluate(() => subject, null, deadline);
		}

		public static IFaultEvaluation<TSubject> Evaluate<TSubject>(
			[NotNull] this IFaultSpecification<TSubject> specification,
			[NotNull] Expression<Func<TSubject>> source,
			IFaultEvaluation<TSubject> prior = null,
			IDeadline deadline = null)
		{
			return specification.Xray.Evaluate(new Source<TSubject>(source), prior, specification.Xray, deadline);
		}

		public static Outcome Evaluate<TSubject>([NotNull] this IObservation<TSubject> observation,
			bool expectedAnException)
		{
			int handledErrors = observation.Errors.Count(x => x.Handled);
			int allErrorsIfAny = observation.Errors.Count;

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
			else
			{
				outcome = Outcome.Failed;
			}
			return outcome;
		}
	}

	public class FaultEvaluation<TSubject> : Evaluation<TSubject>,
		IFaultEvaluation<TSubject>,
		IFaultEvaluationState<TSubject>
	{
		public FaultEvaluation([NotNull] IObservation<TSubject> observation,
			Outcome outcome,
			[NotNull] IFaultSpecificationState<TSubject> tailSpecification,
			[CanBeNull] IFaultEvaluation<TSubject> prior,
			[NotNull] IFaultSpecification<TSubject> specification)
			: base(observation, outcome)
		{
			Observation = observation.ValidateArgumentIsNotNull();
			Specification = specification.ValidateArgumentIsNotNull();
			TailSpecification = tailSpecification.ValidateArgumentIsNotNull();
			Prior = prior;
		}

		public IObservation<TSubject> Observation { get; private set; }
		public IAcceptEvaluationVisitors Parent
		{
			get { return Specification.Xray; }
		}
		public IFaultEvaluation<TSubject> Prior { get; private set; }
		public IFaultSpecification<TSubject> Specification { get; private set; }
		public IFaultSpecificationState<TSubject> TailSpecification { get; private set; }

		public IFaultEvaluationState<TSubject> Xray
		{
			get { return this; }
		}

		public IFaultEvaluation<TSubject> EvaluateNext(IDeadline deadline = null)
		{
			return EvaluateNextWith(Observation.Sample.Source, deadline);
		}

		public IFaultEvaluation<TSubject> EvaluateNextWith(ISource<TSubject> source, IDeadline deadline = null)
		{
			IFaultSpecification<TSubject> specification = GetNextSpecification();
			if (specification == null)
			{
				return null;
			}
			IFaultEvaluation<TSubject> evaluation = specification.Xray.Evaluate(source,
				this,
				TailSpecification,
				deadline);
			return evaluation;
		}

		public void Accept(IEvaluationVisitor visitor)
		{
			visitor.Visit1(this);
		}

		public TData Accept<TData>(IEvaluationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit1(this, data);
		}

		public IEnumerable<IFaultEvaluation<TSubject>> Predecessors()
		{
			IFaultEvaluation<TSubject> prior = this;
			while (prior.Xray.Prior != null)
			{
				prior = prior.Xray.Prior;
				yield return prior;
			}
		}

		[CanBeNull]
		private IFaultSpecification<TSubject> GetNextSpecification()
		{
			List<IFaultEvaluation<TSubject>> list = Predecessors().Reverse().ToList();
			int distanceOfNextFromRootSpec = list.Count + 1;
			IFaultSpecification<TSubject> specification =
				TailSpecification.GetPredecessors(true).Reverse().Skip(distanceOfNextFromRootSpec).FirstOrDefault();
			return specification;
		}
	}
}
