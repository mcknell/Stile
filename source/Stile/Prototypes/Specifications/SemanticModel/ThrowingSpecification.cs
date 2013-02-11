#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel
{
	public interface IThrowingSpecification : ISpecification {}

	public interface IThrowingSpecification<TSubject> : IThrowingSpecification {}

	public interface IThrowingSpecification<TSubject, TException> : IThrowingSpecification<TSubject>,
		ISpecification<TSubject, TException, IVoidSpecificationState<TSubject, TException>>
		where TException : Exception {}

	public interface IVoidSpecificationState {}

	public interface IVoidSpecificationState<TSubject, TException> : IVoidSpecificationState,
		ISpecificationState<TSubject>
		where TException : Exception
	{
		[CanBeNull]
		IExceptionFilter<TException> ExceptionFilter { get; }
		[NotNull]
		IVoidInstrument<TSubject> Instrument { get; }
	}

	public static class ThrowingSpecification
	{
		public delegate TSpecification Factory<out TSpecification, TSubject, TException>(
			IVoidInstrument<TSubject> instrument,
			IExceptionFilter<TException> exceptionFilter,
			ISource<TSubject> source = null) where TSpecification : class, IThrowingSpecification<TSubject>
			where TException : Exception;

		public static ThrowingSpecification<TSubject, TException> Make<TSubject, TException>(
			[NotNull] IVoidInstrument<TSubject> instrument,
			IExceptionFilter<TException> exceptionFilter,
			ISource<TSubject> source = null,
			string because = null) where TException : Exception
		{
			return ThrowingSpecification<TSubject, TException>.Make(instrument, exceptionFilter, source, because);
		}
	}

	public class ThrowingSpecification<TSubject, TException> : Specification<TSubject>,
		IThrowingBoundSpecification<TSubject, TException>,
		IVoidSpecificationState<TSubject, TException>
		where TException : Exception
	{
		public ThrowingSpecification([NotNull] IVoidInstrument<TSubject> instrument,
			[NotNull] IExceptionFilter<TException> exceptionFilter,
			[CanBeNull] ISource<TSubject> source,
			[CanBeNull] string because)
			: base(source, because)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			ExceptionFilter = exceptionFilter;
		}

		public IExceptionFilter<TException> ExceptionFilter { get; private set; }
		public IVoidInstrument<TSubject> Instrument { get; private set; }

		public IVoidSpecificationState<TSubject, TException> Xray
		{
			get { return this; }
		}

		public IEvaluation<TException> Evaluate(TSubject subject)
		{
			throw new NotImplementedException();
		}

		public IEvaluation<TException> Evaluate()
		{
			throw new NotImplementedException();
		}

		public static ThrowingSpecification<TSubject, TException> Make([NotNull] IVoidInstrument<TSubject> instrument,
			IExceptionFilter<TException> exceptionFilter,
			ISource<TSubject> source = null,
			string because = null)
		{
			return new ThrowingSpecification<TSubject, TException>(instrument, exceptionFilter, source, because);
		}
	}
}
