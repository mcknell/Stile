#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
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
		IChainableSpecification
	{
		[NotNull]
		IEvaluation Evaluate(TSubject subject, IDeadline deadline = null);
	}

	public interface IVoidSpecificationState {}

	public interface IVoidSpecificationState<TSubject> : IVoidSpecificationState,
		ISpecificationState<TSubject>
	{
		[NotNull]
		IExceptionFilter ExceptionFilter { get; }
		[NotNull]
		IProcedure<TSubject> Procedure { get; }
	}

	public static class VoidSpecification
	{
		public delegate TSpecification Factory<out TSpecification, TSubject>(
			[NotNull] IProcedure<TSubject> procedure,
			[NotNull] IExceptionFilter exceptionFilter,
			ISource<TSubject> source = null) where TSpecification : class, ISpecification<TSubject>;
	}

	public class VoidSpecification<TSubject> : Specification<TSubject>,
		IVoidBoundSpecification<TSubject>,
		IVoidSpecificationState<TSubject>
	{
		private readonly IDeadline _deadline;

		protected VoidSpecification([NotNull] IProcedure<TSubject> procedure,
			[NotNull] IExceptionFilter exceptionFilter,
			[CanBeNull] ISource<TSubject> source,
			[CanBeNull] string because,
			IDeadline deadline = null)
			: base(source, because)
		{
			Procedure = procedure.ValidateArgumentIsNotNull();
			ExceptionFilter = exceptionFilter.ValidateArgumentIsNotNull();
			_deadline = deadline;
		}

		public IExceptionFilter ExceptionFilter { get; private set; }
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
			return Evaluate(Source.Get, deadline);
		}

		public IEvaluation Evaluate(TSubject subject, IDeadline deadline = null)
		{
			return Evaluate(() => subject, deadline);
		}

		public static VoidSpecification<TSubject> Make([NotNull] IProcedure<TSubject> procedure,
			IExceptionFilter exceptionFilter,
			ISource<TSubject> source = null)
		{
			return new VoidSpecification<TSubject>(procedure, exceptionFilter, null, null);
		}

		public static VoidSpecification<TSubject> MakeBound([NotNull] IProcedure<TSubject> procedure,
			[NotNull] IExceptionFilter exceptionFilter,
			[NotNull] ISource<TSubject> source)
		{
			return new VoidSpecification<TSubject>(procedure, exceptionFilter, source, null);
		}

		private IEvaluation Evaluate(Func<TSubject> subjectGetter, IDeadline deadline = null)
		{
			IObservation observation = Procedure.Sample(subjectGetter, deadline ?? _deadline);
			observation = ExceptionFilter.Filter(observation);
			return Expectation.Evaluate(observation, true);
		}
	}
}
