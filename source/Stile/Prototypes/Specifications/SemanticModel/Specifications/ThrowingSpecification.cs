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

		public static ThrowingSpecification<TSubject, TException> Make<TSubject, TException>(
			[NotNull] IThrowingInstrument<TSubject> instrument,
			IExceptionFilter<TException> exceptionFilter,
			ISource<TSubject> source = null,
			string because = null) where TException : Exception
		{
			return ThrowingSpecification<TSubject, TException>.Make(instrument, exceptionFilter, source, because);
		}

		public static IThrowingSpecificationBuilder<TSpecification, TSubject, TException> Resolve
			<TSpecification, TSubject, TException>([NotNull] IThrowingInstrument<TSubject> instrument,
				ISource<TSubject> source = null) where TSpecification : class, ISpecification<TSubject>
			where TException : Exception
		{
			throw new NotImplementedException();
		}
	}

	public interface IThrowingSpecificationBuilder {}

	public interface IThrowingSpecificationBuilder<out TSpecification, TSubject, TException> :
		IThrowingSpecificationBuilder
		where TSpecification : class, ISpecification<TSubject>
		where TException : Exception
	{
		TSpecification Build();
	}

	public class ThrowingSpecificationBuilder<TSpecification, TSubject, TException> :
		IThrowingSpecificationBuilder<TSpecification, TSubject, TException>
		where TSpecification : class, IThrowingSpecification<TSubject>
		where TException : Exception
	{
		public TSpecification Build()
		{
			throw new NotImplementedException();
		}
	}

	public class ThrowingSpecification<TSubject, TException> : Specification<TSubject>,
		IThrowingBoundSpecification<TSubject>,
		IThrowingSpecificationState<TSubject>
		where TException : Exception
	{
		protected ThrowingSpecification([NotNull] IThrowingInstrument<TSubject> instrument,
			[NotNull] IExceptionFilter<TException> exceptionFilter,
			[CanBeNull] ISource<TSubject> source,
			[CanBeNull] string because)
			: base(source, because)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			ExceptionFilter = exceptionFilter;
		}

		public IExceptionFilter ExceptionFilter { get; private set; }
		public IThrowingInstrument<TSubject> Instrument { get; private set; }

		public IThrowingSpecificationState<TSubject> Xray
		{
			get { return this; }
		}

		public IEvaluation Evaluate()
		{
			throw new NotImplementedException();
		}

		IEvaluation IThrowingSpecification<TSubject>.Evaluate(TSubject subject)
		{
			return Evaluate(subject);
		}

		public IEvaluation<TException> Evaluate(TSubject subject)
		{
			throw new NotImplementedException();
		}

		public static ThrowingSpecification<TSubject, TException> Make(
			[NotNull] IThrowingInstrument<TSubject> instrument, [NotNull] IExceptionFilter<TException> exceptionFilter)
		{
			return new ThrowingSpecification<TSubject, TException>(instrument, exceptionFilter, null, null);
		}

		public static ThrowingSpecification<TSubject, TException> Make(
			[NotNull] IThrowingInstrument<TSubject> instrument,
			IExceptionFilter<TException> exceptionFilter,
			ISource<TSubject> source = null,
			string because = null)
		{
			return new ThrowingSpecification<TSubject, TException>(instrument, exceptionFilter, source, because);
		}

		public static ThrowingSpecification<TSubject, TException> MakeBound([NotNull] ISource<TSubject> source,
			[NotNull] IThrowingInstrument<TSubject> instrument,
			[NotNull] IExceptionFilter<TException> exceptionFilter)
		{
			return new ThrowingSpecification<TSubject, TException>(instrument, exceptionFilter, source, null);
		}
	}
}
