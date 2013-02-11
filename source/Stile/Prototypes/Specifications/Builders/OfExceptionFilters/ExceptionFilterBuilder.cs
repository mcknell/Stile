#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExceptionFilters
{
	public interface IExceptionFilterBuilder {}

	public interface IExceptionFilterBuilder<out TSpecification, TSubject> : IExceptionFilterBuilder,
		IHides<IExceptionFilterBuilderState<TSubject>>
		where TSpecification : class, IThrowingSpecification<TSubject>
	{
		TSpecification Throws<TException>() where TException : Exception;
	}

	public interface IExceptionFilterBuilderState {}

	public interface IExceptionFilterBuilderState<TSubject> : IExceptionFilterBuilderState
	{
		[NotNull]
		IVoidInstrument<TSubject> Instrument { get; }
		[CanBeNull]
		ISource<TSubject> Source { get; }
	}

	public class ExceptionFilterBuilder<TSpecification, TSubject> :
		IExceptionFilterBuilder<TSpecification, TSubject>,
		IExceptionFilterBuilderState<TSubject>
		where TSpecification : class, IThrowingSpecification<TSubject>
	{
		public delegate Func<ThrowingSpecification.Factory<TSpecification, TSubject, TException>> FactoryFinder
			<TException>() where TException : Exception;

		public ExceptionFilterBuilder([NotNull] IVoidInstrument<TSubject> instrument,
			ISource<TSubject> source = null)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			Source = source;
		}

		public IVoidInstrument<TSubject> Instrument { get; private set; }
		public ISource<TSubject> Source { get; private set; }
		public IExceptionFilterBuilderState<TSubject> Xray
		{
			get { return this; }
		}

		public TSpecification Throws<TException>() where TException : Exception
		{
			return new ThrowingSpecification<TSubject, TException>(Instrument,
				new ExceptionFilter<TException>(),
				Source,
				null);
		}
	}

	public static class ExceptionFilterBuilderExtensions
	{
		public static IThrowingSpecification<TSubject> Throws<TSpecification, TSubject, TException>(
			this IExceptionFilterBuilder<TSpecification, TSubject> builder)
			where TSpecification : class, IThrowingSpecification<TSubject, TException> where TException : Exception
		{
			IExceptionFilterBuilderState<TSubject> state = builder.Xray;
			IVoidInstrument<TSubject> instrument = state.Instrument;
			var exceptionFilter = new ExceptionFilter<TException>();
			ISource<TSubject> source = state.Source;
			return new ThrowingSpecification<TSubject, TException>(instrument, exceptionFilter, source, null);
		}
	}
}
