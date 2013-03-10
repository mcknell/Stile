#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfSpecifications
{
	public interface IFaultSpecificationBuilder {}

	public interface IFaultSpecificationBuilder<out TSpecification, TSubject, TException> :
		IFaultSpecificationBuilder
		where TSpecification : class, IChainableSpecification
		where TException : Exception
	{
		TSpecification Build();
	}

	public interface IFaultSpecificationBuilder<out TSpecification, TSubject, TResult, TException> :
		IFaultSpecificationBuilder<TSpecification, TSubject, TException>
		where TSpecification : class, IChainableSpecification
		where TException : Exception {}

	public abstract class FaultSpecificationBuilder : IFaultSpecificationBuilder {}

	public class FaultSpecificationBuilder<TSpecification, TSubject, TException> : FaultSpecificationBuilder,
		IFaultSpecificationBuilder<TSpecification, TSubject, TException>
		where TSpecification : class, IChainableSpecification, ISpecification<TSubject>
		where TException : Exception
	{
		private readonly IExceptionFilter<TSubject> _exceptionFilter;
		private readonly IProcedure<TSubject> _procedure;
		private readonly FaultSpecification.Factory<TSpecification, TSubject> _specificationFactory;

		public FaultSpecificationBuilder([NotNull] IProcedure<TSubject> procedure,
			[NotNull] IExceptionFilter<TSubject> exceptionFilter,
			[NotNull] FaultSpecification.Factory<TSpecification, TSubject> specificationFactory)
		{
			_procedure = procedure.ValidateArgumentIsNotNull();
			_exceptionFilter = exceptionFilter.ValidateArgumentIsNotNull();
			_specificationFactory = specificationFactory.ValidateArgumentIsNotNull();
		}

		public TSpecification Build()
		{
			return _specificationFactory.Invoke(_procedure, _exceptionFilter);
		}
	}

	public class FaultSpecificationBuilder<TSpecification, TSubject, TResult, TException> :
		IFaultSpecificationBuilder<TSpecification, TSubject, TResult, TException>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		where TException : Exception
	{
		private readonly IExceptionFilter<TSubject, TResult> _exceptionFilter;
		private readonly Func<IExpectation<TSubject, TResult>, IExceptionFilter<TSubject, TResult>, TSpecification>
			_specificationFactory;

		public FaultSpecificationBuilder([NotNull] IExceptionFilter<TSubject, TResult> exceptionFilter,
			[NotNull] Func<IExpectation<TSubject, TResult>, IExceptionFilter<TSubject, TResult>, TSpecification>
				specificationFactory)
		{
			_exceptionFilter = exceptionFilter.ValidateArgumentIsNotNull();
			_specificationFactory = specificationFactory.ValidateArgumentIsNotNull();
		}

		public TSpecification Build()
		{
			Expression<Predicate<TResult>> expression = result => true;
			var expectation = new Expectation<TSubject, TResult>(expression.Compile,
				_exceptionFilter,
				_exceptionFilter.Instrument);

			return _specificationFactory.Invoke(expectation, _exceptionFilter);
		}
	}
}
