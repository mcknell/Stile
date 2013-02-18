#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfSpecifications
{
	public interface IThrowingSpecificationBuilder {}

	public interface IThrowingSpecificationBuilder<out TSpecification, TSubject> : IThrowingSpecificationBuilder
		where TSpecification : class, ISpecification<TSubject>
	{
		TSpecification Build();
	}

	public interface IThrowingSpecificationBuilder<out TSpecification, TSubject, TResult> :
		IThrowingSpecificationBuilder<TSpecification, TSubject>
		where TSpecification : class, ISpecification<TSubject, TResult> {}

	public abstract class ThrowingSpecificationBuilder {}

	public class ThrowingSpecificationBuilder<TSpecification, TSubject> : ThrowingSpecificationBuilder,
		IThrowingSpecificationBuilder<TSpecification, TSubject>
		where TSpecification : class, ISpecification<TSubject>
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

	public class ThrowingSpecificationBuilder<TSpecification, TSubject, TResult> :
		IThrowingSpecificationBuilder<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>
	{
		private readonly ISource<TSubject> _source;
		private readonly IInstrument<TSubject, TResult> _instrument;
		private readonly IExceptionFilter<TResult> _exceptionFilter;
		private readonly Specification.Factory<TSpecification, TSubject, TResult> _specificationFactory;

		public ThrowingSpecificationBuilder([CanBeNull] ISource<TSubject> source,
			[NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] IExceptionFilter<TResult> exceptionFilter,
			[NotNull] Specification.Factory<TSpecification, TSubject, TResult> specificationFactory)
		{
			_source = source;
			_instrument = instrument.ValidateArgumentIsNotNull();
			_exceptionFilter = exceptionFilter.ValidateArgumentIsNotNull();
			_specificationFactory = specificationFactory.ValidateArgumentIsNotNull();
		}

		public TSpecification Build()
		{
			return _specificationFactory.Invoke(_source, _instrument, Criterion<TResult>.UnconditionalAcceptance, _exceptionFilter);
		}
	}
}
