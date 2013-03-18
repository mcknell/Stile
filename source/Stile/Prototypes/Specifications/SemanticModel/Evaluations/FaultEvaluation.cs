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
using Stile.Patterns.Structural.Hierarchy;
using Stile.Prototypes.Specifications.Builders.Lifecycle;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Evaluations
{
	public interface IFaultEvaluation<TSubject> : IEvaluation<TSubject>,
		IHides<IFaultEvaluationState<TSubject>> {}

	public interface IFaultEvaluationState<TSubject> : IHasFaultSpecification<TSubject>,
		IAcceptSpecificationVisitors
	{
		[CanBeNull]
		IFaultEvaluation<TSubject> Prior { get; }
		[NotNull]
		IFaultSpecificationState<TSubject> TailSpecification { get; }
	}

	public static class FaultEvaluation
	{
		public static IFaultEvaluation<TSubject> Evaluate<TSubject>(
			[NotNull] this IFaultSpecification<TSubject> specification,
			[CanBeNull] TSubject subject,
			IDeadline deadline = null)
		{
			return specification.Evaluate(() => subject, deadline);
		}

		public static IFaultEvaluation<TSubject> Evaluate<TSubject>(
			[NotNull] this IFaultSpecification<TSubject> specification,
			[NotNull] Expression<Func<TSubject>> source,
			IDeadline deadline = null)
		{
			return specification.Evaluate(new Source<TSubject>(source), deadline);
		}

		public static Outcome Evaluate<TSubject>([NotNull] this IObservation<TSubject> observation,
			bool expectedAnException,
			[NotNull] IFaultSpecificationState<TSubject> tailSpecification,
			[CanBeNull] IFaultEvaluation<TSubject> prior)
		{
			int handledErrors = observation.Errors.Count(x => x.Handled);
			int allErrorsIfAny = observation.Errors.Length;

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
			Specification = specification.ValidateArgumentIsNotNull();
			TailSpecification = tailSpecification.ValidateArgumentIsNotNull();
			Prior = prior;
		}

		public IFaultEvaluation<TSubject> Prior { get; private set; }
		public IFaultSpecification<TSubject> Specification { get; private set; }
		public IFaultSpecificationState<TSubject> TailSpecification { get; private set; }
		public IFaultEvaluationState<TSubject> Xray
		{
			get { return this; }
		}

		public void Accept(ISpecificationVisitor visitor)
		{
			throw new NotImplementedException(NoSpecificationVisitorsPlease);
		}

		public TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data)
		{
			throw new NotImplementedException(NoSpecificationVisitorsPlease);
		}

		IAcceptSpecificationVisitors IHasParent<IAcceptSpecificationVisitors>.Parent
		{
			get { return Specification.Xray; }
		}

		public void Accept(IEvaluationVisitor visitor)
		{
			visitor.Visit1(this);
		}
	}
}
