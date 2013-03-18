#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Patterns.Structural.Hierarchy;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface IFaultSpecification : ISpecification {}

	public interface IFaultSpecification<TSubject> : IFaultSpecification,
		ISpecification<TSubject>,
		IHides<IFaultSpecificationState<TSubject>>,
		IChainableSpecification
	{
		[NotNull]
		[System.Diagnostics.Contracts.Pure]
		IFaultEvaluation<TSubject> Evaluate([NotNull] ISource<TSubject> source, IDeadline deadline = null);
	}

	public interface IFaultSpecificationState {}

	public interface IFaultSpecificationState<TSubject> : IFaultSpecificationState,
		ISpecificationState<TSubject>
	{
		[NotNull]
		IExceptionFilter<TSubject> ExceptionFilter { get; }
		[NotNull]
		IProcedure<TSubject> Procedure { get; }
	}

	public static class FaultSpecification
	{
		public delegate TSpecification Factory<out TSpecification, TSubject>(
			[NotNull] IProcedure<TSubject> procedure, [NotNull] IExceptionFilter<TSubject> exceptionFilter)
			where TSpecification : class, ISpecification<TSubject>;

		public delegate TSpecification Factory<out TSpecification, TSubject, TResult>(
			[NotNull] IExpectation<TSubject, TResult> expectation,
			[NotNull] IExceptionFilter<TSubject, TResult> exceptionFilter)
			where TSpecification : class, ISpecification<TSubject, TResult>;
	}

	public class FaultSpecification<TSubject> : Specification<TSubject, IExceptionFilter<TSubject>>,
		IBoundFaultSpecification<TSubject>,
		IFaultSpecificationState<TSubject>
	{
		protected FaultSpecification([NotNull] IProcedure<TSubject> procedure,
			[NotNull] IExceptionFilter<TSubject> exceptionFilter,
			IDeadline deadline = null,
			string because = null)
			: base(exceptionFilter, exceptionFilter, deadline, because)
		{
			Procedure = procedure.ValidateArgumentIsNotNull();
		}

		public IAcceptEvaluationVisitors Parent
		{
			get { return null; }
		}

		public IProcedure<TSubject> Procedure { get; private set; }

		public IFaultSpecificationState<TSubject> Xray
		{
			get { return this; }
		}

		public override ISpecification Clone(IDeadline deadline)
		{
			return new FaultSpecification<TSubject>(Procedure, ExceptionFilter, deadline, Because);
		}

		public IFaultEvaluation<TSubject> Evaluate(IDeadline deadline = null)
		{
			return Evaluate(Procedure.Xray.Source, deadline);
		}

		public IFaultEvaluation<TSubject> Evaluate(ISource<TSubject> source, IDeadline deadline = null)
		{
			IObservation<TSubject> observation = Procedure.Observe(source, deadline ?? Deadline);
			observation = ExceptionFilter.Filter(observation);
			Outcome outcome = observation.Evaluate(true, this, null);
			return new FaultEvaluation<TSubject>(observation, outcome, this, null, this);
		}

		public void Accept(ISpecificationVisitor visitor)
		{
			visitor.Visit1(this);
		}

		public TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit1(this, data);
		}

		public TData Accept<TData>(IEvaluationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit1(this, data);
		}

		public void Accept(IEvaluationVisitor visitor)
		{
			visitor.Visit1(this);
		}

		IAcceptSpecificationVisitors IHasParent<IAcceptSpecificationVisitors>.Parent
		{
			get { return ExceptionFilter; }
		}

		public static FaultSpecification<TSubject> Make([NotNull] IProcedure<TSubject> procedure,
			IExceptionFilter<TSubject> exceptionFilter)
		{
			return new FaultSpecification<TSubject>(procedure, exceptionFilter);
		}

		public static FaultSpecification<TSubject> MakeBound([NotNull] IProcedure<TSubject> procedure,
			[NotNull] IExceptionFilter<TSubject> exceptionFilter)
		{
			return new FaultSpecification<TSubject>(procedure, exceptionFilter);
		}
	}
}
