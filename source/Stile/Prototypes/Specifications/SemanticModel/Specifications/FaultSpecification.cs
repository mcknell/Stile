#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface IFaultSpecification : ISpecification {}

	public interface IFaultSpecification<TSubject> : IFaultSpecification,
		ISpecification<TSubject>,
		IHides<IFaultSpecificationState<TSubject>>,
		IChainableSpecification,
		IEvaluable<TSubject> {}

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

		public IProcedure<TSubject> Procedure { get; private set; }

		public IFaultSpecificationState<TSubject> Xray
		{
			get { return this; }
		}

		public override ISpecification Clone(IDeadline deadline)
		{
			return new FaultSpecification<TSubject>(Procedure, ExceptionFilter, deadline, Because);
		}

		public IEvaluation Evaluate(IDeadline deadline = null)
		{
			return Evaluate(Procedure.Xray.Source, deadline);
		}

		public IEvaluation<TSubject> Evaluate(ISource<TSubject> source, IDeadline deadline = null)
		{
			IObservation<TSubject> observation = Procedure.Observe(source, deadline ?? Deadline);
			observation = ExceptionFilter.Filter(observation);
			return Expectation<TSubject>.Evaluate(observation, true);
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
