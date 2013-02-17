#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfSpecifications
{
	public interface IThrowingSpecificationBuilder {}

	public interface IThrowingSpecificationBuilder<out TSpecification, TSubject, TException> :
		IThrowingSpecificationBuilder
		where TSpecification : class, ISpecification<TSubject>
		where TException : Exception
	{
		TSpecification Build();
	}

	public abstract class ThrowingSpecificationBuilder {}

	public class ThrowingSpecificationBuilder<TSpecification, TSubject, TException> :
		ThrowingSpecificationBuilder,
		IThrowingSpecificationBuilder<TSpecification, TSubject, TException>
		where TSpecification : class, IThrowingSpecification<TSubject>
		where TException : Exception
	{
		private readonly IExceptionFilter _exceptionFilter;
		private readonly IThrowingInstrument<TSubject> _instrument;
		private readonly ISource<TSubject> _source;
		private readonly ThrowingSpecification.Factory<TSpecification, TSubject> _specificationFactory;

		public ThrowingSpecificationBuilder([CanBeNull] ISource<TSubject> source,
			[NotNull] IThrowingInstrument<TSubject> instrument,
			[NotNull] IExceptionFilter exceptionFilter,
			[NotNull] ThrowingSpecification.Factory<TSpecification, TSubject> specificationFactory)
		{
			_source = source;
			_instrument = instrument.ValidateArgumentIsNotNull();
			_exceptionFilter = exceptionFilter.ValidateArgumentIsNotNull();
			_specificationFactory = specificationFactory.ValidateArgumentIsNotNull();
		}

		public TSpecification Build()
		{
			return _specificationFactory.Invoke(_instrument, _exceptionFilter, _source);
		}
	}
}
