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

	public interface IThrowingSpecificationBuilder<out TSpecification, TSubject> : IThrowingSpecificationBuilder
		where TSpecification : class, IChainableSpecification
	{
		TSpecification Build();
	}

	public interface IThrowingSpecificationBuilder<out TSpecification, TSubject, TResult> :
		IThrowingSpecificationBuilder<TSpecification, TSubject>
		where TSpecification : class, IChainableSpecification {}

	public abstract class ThrowingSpecificationBuilder {}

	public class ThrowingSpecificationBuilder<TSpecification, TSubject> : ThrowingSpecificationBuilder,
		IThrowingSpecificationBuilder<TSpecification, TSubject>
		where TSpecification : class, IChainableSpecification, ISpecification<TSubject>
	{
		private readonly IExceptionFilter _exceptionFilter;
		private readonly IProcedure<TSubject> _procedure;
		private readonly ISource<TSubject> _source;
		private readonly VoidSpecification.Factory<TSpecification, TSubject> _specificationFactory;

		public ThrowingSpecificationBuilder([CanBeNull] ISource<TSubject> source,
			[NotNull] IProcedure<TSubject> procedure,
			[NotNull] IExceptionFilter exceptionFilter,
			[NotNull] VoidSpecification.Factory<TSpecification, TSubject> specificationFactory)
		{
			_source = source;
			_procedure = procedure.ValidateArgumentIsNotNull();
			_exceptionFilter = exceptionFilter.ValidateArgumentIsNotNull();
			_specificationFactory = specificationFactory.ValidateArgumentIsNotNull();
		}

		public TSpecification Build()
		{
			return _specificationFactory.Invoke(_procedure, _exceptionFilter, _source);
		}
	}

	public class ThrowingSpecificationBuilder<TSpecification, TSubject, TResult> :
		IThrowingSpecificationBuilder<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
	{
		private readonly IExceptionFilter<TSubject, TResult> _exceptionFilter;
		private readonly Func<ICriterion<TResult>, IExceptionFilter<TSubject, TResult>, TSpecification>
			_specificationFactory;

		public ThrowingSpecificationBuilder([NotNull] IExceptionFilter<TSubject, TResult> exceptionFilter,
			[NotNull] Func<ICriterion<TResult>, IExceptionFilter<TSubject, TResult>, TSpecification> specificationFactory)
		{
			_exceptionFilter = exceptionFilter.ValidateArgumentIsNotNull();
			_specificationFactory = specificationFactory.ValidateArgumentIsNotNull();
		}

		public TSpecification Build()
		{
			return _specificationFactory.Invoke(Criterion<TResult>.UnconditionalAcceptance, _exceptionFilter);
		}
	}
}
