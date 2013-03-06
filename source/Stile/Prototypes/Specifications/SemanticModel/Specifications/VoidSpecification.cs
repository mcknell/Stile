#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface IVoidSpecification : ISpecification {}

	public interface IVoidSpecification<TSubject> : IVoidSpecification,
		ISpecification<TSubject>,
		IHides<IVoidSpecificationState<TSubject>>,
		IChainableSpecification,
		IEvaluable<TSubject> {}

	public interface IVoidSpecificationState {}

	public interface IVoidSpecificationState<TSubject> : IVoidSpecificationState,
		ISpecificationState<TSubject>
	{
		[NotNull]
		IExceptionFilter<TSubject> ExceptionFilter { get; }
		[NotNull]
		IProcedure<TSubject> Procedure { get; }
	}

	public static class VoidSpecification
	{
		public delegate TSpecification Factory<out TSpecification, TSubject>(
			[NotNull] IProcedure<TSubject> procedure,
			[NotNull] IExceptionFilter<TSubject> exceptionFilter,
			ISource<TSubject> source = null) where TSpecification : class, ISpecification<TSubject>;
	}

	public class VoidSpecification<TSubject> : Specification<TSubject>,
		IVoidBoundSpecification<TSubject>,
		IVoidSpecificationState<TSubject>
	{
		private readonly IDeadline _deadline;

		protected VoidSpecification([NotNull] IProcedure<TSubject> procedure,
			[NotNull] IExceptionFilter<TSubject> exceptionFilter,
			[CanBeNull] ISource<TSubject> source,
			[CanBeNull] string because,
			IDeadline deadline = null)
			: base(source, because)
		{
			Procedure = procedure.ValidateArgumentIsNotNull();
			ExceptionFilter = exceptionFilter.ValidateArgumentIsNotNull();
			_deadline = deadline;
		}

		public IExceptionFilter<TSubject> ExceptionFilter { get; private set; }
		public IProcedure<TSubject> Procedure { get; private set; }

		public IVoidSpecificationState<TSubject> Xray
		{
			get { return this; }
		}

		public override ISpecification Clone(IDeadline deadline)
		{
			return new VoidSpecification<TSubject>(Procedure, ExceptionFilter, Source, Because, deadline);
		}

		public IEvaluation Evaluate(IDeadline deadline = null)
		{
			return Evaluate(Source, deadline);
		}

		public IEvaluation<TSubject> Evaluate(ISource<TSubject> source, IDeadline deadline = null)
		{
			IObservation<TSubject> observation = Procedure.Observe(source, deadline ?? _deadline);
			observation = ExceptionFilter.Filter(observation);
			return Expectation<TSubject>.Evaluate(observation, true);
		}

		public static VoidSpecification<TSubject> Make([NotNull] IProcedure<TSubject> procedure,
			IExceptionFilter<TSubject> exceptionFilter,
			ISource<TSubject> source = null)
		{
			return new VoidSpecification<TSubject>(procedure, exceptionFilter, null, null);
		}

		public static VoidSpecification<TSubject> MakeBound([NotNull] IProcedure<TSubject> procedure,
			[NotNull] IExceptionFilter<TSubject> exceptionFilter,
			[NotNull] ISource<TSubject> source)
		{
			return new VoidSpecification<TSubject>(procedure, exceptionFilter, source, null);
		}
	}
}
