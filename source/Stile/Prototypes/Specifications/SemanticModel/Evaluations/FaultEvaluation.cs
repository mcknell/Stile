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

		[NotNull]
		[System.Diagnostics.Contracts.Pure]
		IFaultEvaluation<TSubject> ReEvaluate(IDeadline deadline = null);
	}

	public interface IFaultEvaluationState<TSubject> : IHasFaultSpecification<TSubject>,
		IAcceptEvaluationVisitors
	{
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

	public class FaultEvaluation<TSubject> :
		Evaluation
			<IFaultSpecification<TSubject>, TSubject, IFaultSpecificationState<TSubject>, IFaultEvaluation<TSubject>>,
		IFaultEvaluation<TSubject>,
		IFaultEvaluationState<TSubject>
	{
		public FaultEvaluation([NotNull] IObservation<TSubject> observation,
			Outcome outcome,
			[NotNull] IFaultSpecificationState<TSubject> tailSpecification,
			[CanBeNull] IFaultEvaluation<TSubject> prior,
			[NotNull] IFaultSpecification<TSubject> specification)
			: base(observation, outcome, specification, prior, tailSpecification)
		{
			Observation = observation.ValidateArgumentIsNotNull();
		}

		public IObservation<TSubject> Observation { get; private set; }

		public IFaultEvaluationState<TSubject> Xray
		{
			get { return this; }
		}

		public override void Accept(IEvaluationVisitor visitor)
		{
			visitor.Visit1(this);
		}

		public override TData Accept<TData>(IEvaluationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit1(this, data);
		}

		public override IEnumerable<IFaultEvaluation<TSubject>> Predecessors()
		{
			IFaultEvaluation<TSubject> prior = this;
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
		protected override Func<IFaultSpecificationState<TSubject>, Evaluator> GetEvaluator
		{
			get { return x => x.Evaluate; }
		}
	}
}
