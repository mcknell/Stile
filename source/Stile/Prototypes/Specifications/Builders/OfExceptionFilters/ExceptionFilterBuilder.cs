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
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExceptionFilters
{
	public interface IExceptionFilterBuilder {}

	public interface IExceptionFilterBuilder<out TSpecification, TSubject> : IExceptionFilterBuilder,
		IHides<IExceptionFilterBuilderState<TSubject>>
		where TSpecification : class, IThrowingSpecification<TSubject>
	{
		IThrowingSpecificationBuilder<TSpecification, TSubject, TException> Throws<TException>()
			where TException : Exception;
	}

	public interface IExceptionFilterBuilderState {}

	public interface IExceptionFilterBuilderState<TSubject> : IExceptionFilterBuilderState
	{
		[NotNull]
		IThrowingInstrument<TSubject> Instrument { get; }
		[CanBeNull]
		ISource<TSubject> Source { get; }
	}

	public class ExceptionFilterBuilder<TSpecification, TSubject> :
		IExceptionFilterBuilder<TSpecification, TSubject>,
		IExceptionFilterBuilderState<TSubject>
		where TSpecification : class, IThrowingSpecification<TSubject>
	{
		private readonly ThrowingSpecification.Factory<TSpecification, TSubject> _specificationFactory;

		protected ExceptionFilterBuilder([NotNull] IThrowingInstrument<TSubject> instrument,
			[NotNull] ThrowingSpecification.Factory<TSpecification, TSubject> specificationFactory,
			ISource<TSubject> source = null)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			_specificationFactory = specificationFactory.ValidateArgumentIsNotNull();
			Source = source;
		}

		public IThrowingInstrument<TSubject> Instrument { get; private set; }
		public ISource<TSubject> Source { get; private set; }
		public IExceptionFilterBuilderState<TSubject> Xray
		{
			get { return this; }
		}

		public IThrowingSpecificationBuilder<TSpecification, TSubject, TException> Throws<TException>()
			where TException : Exception
		{
			var exceptionFilter = new ExceptionFilter(exception => exception is TException);
			var builder = new ThrowingSpecificationBuilder<TSpecification, TSubject, TException>(Source,
				Instrument, exceptionFilter,
				_specificationFactory);
			return builder;
		}

		public static ExceptionFilterBuilder<TSpecification, TSubject> Make(
			[NotNull] IThrowingInstrument<TSubject> instrument,
			[NotNull] ThrowingSpecification.Factory<TSpecification, TSubject> specificationFactory)
		{
			return new ExceptionFilterBuilder<TSpecification, TSubject>(instrument, specificationFactory);
		}

		public static ExceptionFilterBuilder<TSpecification, TSubject> MakeBound([NotNull] ISource<TSubject> source,
			[NotNull] IThrowingInstrument<TSubject> instrument,
			[NotNull] ThrowingSpecification.Factory<TSpecification, TSubject> specificationFactory)
		{
			return new ExceptionFilterBuilder<TSpecification, TSubject>(instrument, specificationFactory, source);
		}
	}

/*
	public static class ExceptionFilterBuilderExtensions
	{
		public static IThrowingSpecificationBuilder<TSpecification, TSubject, TException> Throws
			<TSpecification, TSubject, TException>(this IExceptionFilterBuilder<TSpecification, TSubject> builder)
			where TException : Exception where TSpecification : class, IThrowingSpecification<TSubject>
		{
			IThrowingInstrument<TSubject> instrument = builder.Xray.Instrument;
			ISource<TSubject> source = builder.Xray.Source;
			return ThrowingSpecificationFactory.Resolve<TSpecification, TSubject, TException>(instrument, source);
		}
	}
*/
}
