#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfSpecifications
{
	public interface IFaultSpecificationBuilder {}

	public interface IFaultSpecificationBuilder<out TSpecification, TSubject, TException> :
		IFaultSpecificationBuilder,
		ISpecificationBuilder<TSpecification>,
		IHides<IFaultSpecificationBuilderState<TSubject, IExceptionFilter<TSubject>>>
		where TSpecification : class, IChainableSpecification
		where TException : Exception {}

	public interface IFaultSpecificationBuilder<out TSpecification, TSubject, TResult, TException> :
		IFaultSpecificationBuilder<TSpecification, TSubject, TException>
		where TSpecification : class, IChainableSpecification
		where TException : Exception {}

	public interface IFaultSpecificationBuilderState<TSubject, out TExceptionFilter>
		where TExceptionFilter : class, IExceptionFilter<TSubject>
	{
		[NotNull]
		TExceptionFilter ExceptionFilter { get; }
	}

	public abstract class FaultSpecificationBuilder : IFaultSpecificationBuilder {}

	public class FaultSpecificationBuilder<TSpecification, TSubject, TException> : FaultSpecificationBuilder,
		IFaultSpecificationBuilder<TSpecification, TSubject, TException>,
		IFaultSpecificationBuilderState<TSubject, IExceptionFilter<TSubject>>
		where TSpecification : class, IChainableSpecification, ISpecification<TSubject>
		where TException : Exception
	{
		private readonly IProcedure<TSubject> _procedure;
		private readonly FaultSpecification.Factory<TSpecification, TSubject> _specificationFactory;

		public FaultSpecificationBuilder([NotNull] IProcedure<TSubject> procedure,
			[NotNull] IExceptionFilter<TSubject> exceptionFilter,
			[NotNull] FaultSpecification.Factory<TSpecification, TSubject> specificationFactory)
		{
			_procedure = procedure.ValidateArgumentIsNotNull();
			ExceptionFilter = exceptionFilter.ValidateArgumentIsNotNull();
			_specificationFactory = specificationFactory.ValidateArgumentIsNotNull();
		}

		public IExceptionFilter<TSubject> ExceptionFilter { get; private set; }
		public IFaultSpecificationBuilderState<TSubject, IExceptionFilter<TSubject>> Xray
		{
			get { return this; }
		}

		public TSpecification Build()
		{
			return _specificationFactory.Invoke(_procedure, ExceptionFilter);
		}
	}

	public class FaultSpecificationBuilder<TSpecification, TSubject, TResult, TException> :
		IFaultSpecificationBuilder<TSpecification, TSubject, TResult, TException>,
		IFaultSpecificationBuilderState<TSubject, IExceptionFilter<TSubject, TResult>>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		where TException : Exception
	{
		private readonly FaultSpecification.Factory<TSpecification, TSubject, TResult> _specificationFactory;

		public FaultSpecificationBuilder([NotNull] IExceptionFilter<TSubject, TResult> exceptionFilter,
			[NotNull] FaultSpecification.Factory<TSpecification, TSubject, TResult> specificationFactory)
		{
			ExceptionFilter = exceptionFilter.ValidateArgumentIsNotNull();
			_specificationFactory = specificationFactory.ValidateArgumentIsNotNull();
		}

		public IExceptionFilter<TSubject, TResult> ExceptionFilter { get; private set; }
		public IFaultSpecificationBuilderState<TSubject, IExceptionFilter<TSubject>> Xray
		{
			get { return this; }
		}

		public TSpecification Build()
		{
			Expression<Predicate<TResult>> expression = result => true;
			var expectation = new Expectation<TSubject, TResult>(expression.Compile,
				ExceptionFilter,
				ExceptionFilter.Instrument);

			return _specificationFactory.Invoke(expectation, ExceptionFilter);
		}
	}

	public static class FaultSpecificationBuilderExtensions
	{
		public static TSpecification Before<TSpecification, TSubject, TException>(
			[NotNull] this IFaultSpecificationBuilder<TSpecification, TSubject, TException> builder, TimeSpan timeout)
			where TSpecification : class, IChainableSpecification, ISpecificationState where TException : Exception
		{
			return builder.Build().Before(timeout);
		}

		public static TSpecification Satisfying<TSpecification, TSubject, TException>(
			[NotNull] this IFaultSpecificationBuilder<TSpecification, TSubject, TException> builder,
			[NotNull] Expression<Predicate<TException>> expression)
			where TSpecification : class, IChainableSpecification where TException : Exception
		{
			//builder.Xray.ExceptionFilter
			throw new NotImplementedException();
		}
	}
}
