#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.Builders.OfSpecifications;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface IThrowingSpecification : ISpecification {}

	public interface IThrowingSpecification<TSubject> : IThrowingSpecification,
		ISpecification<TSubject>,
		IHides<IThrowingSpecificationState<TSubject>>
	{
		[NotNull]
		IEvaluation Evaluate(TSubject subject);
	}

	public interface IThrowingSpecificationState {}

	public interface IThrowingSpecificationState<TSubject> : IThrowingSpecificationState,
		ISpecificationState<TSubject>
	{
		[CanBeNull]
		IExceptionFilter ExceptionFilter { get; }
		[NotNull]
		IThrowingInstrument<TSubject> Instrument { get; }
	}

	public static class ThrowingSpecificationFactory
	{
		public delegate TSpecification Method<out TSpecification, TSubject, TException>(
			[CanBeNull] ISource<TSubject> source,
			[NotNull] IThrowingInstrument<TSubject> instrument,
			[NotNull] IExceptionFilter<TException> exceptionFilter) where TSpecification : class
			where TException : Exception;

		public static IThrowingSpecification<TSubject> Make<TSubject, TException>(
			[NotNull] IThrowingInstrument<TSubject> instrument,
			IExceptionFilter<TException> exceptionFilter,
			ISource<TSubject> source = null) where TException : Exception
		{
			return ThrowingSpecification<TSubject>.Make(instrument, exceptionFilter, source);
		}

		public static IThrowingSpecificationBuilder<TSpecification, TSubject, TException> Resolve
			<TSpecification, TSubject, TException>([NotNull] IThrowingInstrument<TSubject> instrument,
				ISource<TSubject> source = null) where TSpecification : class, ISpecification<TSubject>
			where TException : Exception
		{
			throw new NotImplementedException();
		}
	}

	public static class ThrowingSpecification
	{
		public delegate TSpecification Factory<out TSpecification, TSubject>(
			[NotNull] IThrowingInstrument<TSubject> instrument,
			[NotNull] IExceptionFilter exceptionFilter,
			ISource<TSubject> source = null) where TSpecification : class, IThrowingSpecification<TSubject>;
	}

	public class ThrowingSpecification<TSubject> : Specification<TSubject>,
		IThrowingBoundSpecification<TSubject>,
		IThrowingSpecificationState<TSubject>
	{
		protected ThrowingSpecification([NotNull] IThrowingInstrument<TSubject> instrument,
			[NotNull] IExceptionFilter exceptionFilter,
			[CanBeNull] ISource<TSubject> source,
			[CanBeNull] string because)
			: base(source, because)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			ExceptionFilter = exceptionFilter.ValidateArgumentIsNotNull();
		}

		public IExceptionFilter ExceptionFilter { get; private set; }
		public IThrowingInstrument<TSubject> Instrument { get; private set; }

		public IThrowingSpecificationState<TSubject> Xray
		{
			get { return this; }
		}

		public IEvaluation Evaluate()
		{
			return Evaluate(Source.Get);
		}

		public IEvaluation Evaluate(TSubject subject)
		{
			return Evaluate(() => subject);
		}

		public static ThrowingSpecification<TSubject> Make([NotNull] IThrowingInstrument<TSubject> instrument,
			IExceptionFilter exceptionFilter,
			ISource<TSubject> source = null)
		{
			return new ThrowingSpecification<TSubject>(instrument, exceptionFilter, null, null);
		}

		public static ThrowingSpecification<TSubject> MakeBound([NotNull] IThrowingInstrument<TSubject> instrument,
			[NotNull] IExceptionFilter exceptionFilter,
			[NotNull] ISource<TSubject> source)
		{
			return new ThrowingSpecification<TSubject>(instrument, exceptionFilter, source, null);
		}

		private IEvaluation Evaluate(Func<TSubject> subjectGetter)
		{
			try
			{
				TSubject subject = subjectGetter.Invoke();
				Instrument.Sample(subject);
			} catch (Exception e)
			{
				IEvaluation evaluation;
				if (ExceptionFilter.TryFilterBeforeResult(e, out evaluation))
				{
					return evaluation;
				}
				// allow unexpected exceptions to bubble out
				throw;
			}

			// exception was expected but none was thrown
			return ExceptionFilter.FailBeforeResult();
		}
	}
}
