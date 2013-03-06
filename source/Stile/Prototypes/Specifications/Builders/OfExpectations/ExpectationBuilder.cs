#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.Builders.Lifecycle;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Builders.OfSpecifications;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations
{
	public interface IExpectationBuilder {}

	public interface IExpectationBuilder<out TSpecification, TSubject, TResult, out THas, out TIs> :
		IExpectationBuilder
		where TSpecification : class, IChainableSpecification
		where THas : class, IHas<TSpecification, TSubject, TResult>
		where TIs : class, IResultIs<TSpecification, TResult>
	{
		[System.Diagnostics.Contracts.Pure]
		THas Has { get; }
		[System.Diagnostics.Contracts.Pure]
		TIs Is { get; }

		[System.Diagnostics.Contracts.Pure]
		IVoidSpecificationBuilder<TSpecification, TSubject, TException> Throws<TException>()
			where TException : Exception;
	}

	public interface IExpectationBuilder<out TSpecification, TSubject, TResult> :
		IExpectationBuilder
			<TSpecification, TSubject, TResult, IHas<TSpecification, TSubject, TResult>,
				INegatableIs<TSpecification, TSubject, TResult, IIs<TSpecification, TSubject, TResult>>>,
		IHides<IExpectationBuilderState<TSpecification, TSubject, TResult>>
		where TSpecification : class, IChainableSpecification {}

	public interface IExpectationBuilderState<out TSpecification, TSubject, TResult> :
		IHasInstrument<TSubject, TResult>
		where TSpecification : class, IChainableSpecification
	{
		TSpecification Make([NotNull] IExpectation<TSubject, TResult> expectation,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null);
	}

	public abstract class ExpectationBuilder
	{
		public delegate TSpecification SpecificationFactory<TSubject, TResult, out TSpecification>(
			IExpectation<TSubject, TResult> expectation, IExceptionFilter<TSubject, TResult> exceptionFilter = null)
			where TSpecification : class, IChainableSpecification;
	}

	public abstract class ExpectationBuilder<TSpecification, TSubject, TResult, THas, TIs, TBuilder> :
		ExpectationBuilder,
		IExpectationBuilder<TSpecification, TSubject, TResult, THas, TIs>,
		IExpectationBuilderState<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification<TBuilder>
		where THas : class, IHas, IHas<TSpecification, TSubject, TResult>
		where TIs : class, IResultIs<TSpecification, TResult>
		where TBuilder : class, IExpectationBuilder

	{
		private readonly Lazy<THas> _lazyHas;
		private readonly Lazy<TIs> _lazyIs;

		protected ExpectationBuilder([NotNull] IInstrument<TSubject, TResult> instrument,
			ISource<TSubject> source = null)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			Source = source;
			_lazyHas = new Lazy<THas>(MakeHas);
			_lazyIs = new Lazy<TIs>(MakeIs);
		}

		public THas Has
		{
			get
			{
				THas value = _lazyHas.Value;
				return value;
			}
		}
		public IInstrument<TSubject, TResult> Instrument { get; private set; }
		public TIs Is
		{
			get
			{
				TIs value = _lazyIs.Value;
				return value;
			}
		}

		public ISource<TSubject> Source { get; private set; }
		public IExpectationBuilderState<TSpecification, TSubject, TResult> Xray
		{
			get { return this; }
		}

		public IVoidSpecificationBuilder<TSpecification, TSubject, TException> Throws<TException>()
			where TException : Exception
		{
			var exceptionFilter = new ExceptionFilter<TSubject, TResult>(exception => exception is TException);
			var builder = new VoidSpecificationBuilder<TSpecification, TSubject, TResult, TException>(exceptionFilter,
				Make);
			return builder;
		}

		public abstract void Accept(IDescriptionVisitor visitor);

		public TSpecification Make(IExpectation<TSubject, TResult> expectation,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null)
		{
			return SpecFactory.Invoke(expectation, exceptionFilter);
		}

		protected abstract TBuilder Builder { get; }
		protected abstract
			Func<IExpectation<TSubject, TResult>, IExceptionFilter<TSubject, TResult>, TSpecification> SpecFactory { get; }

		protected IBoundSpecification<TSubject, TResult, TBuilder> MakeBoundSpecification(
			IExpectation<TSubject, TResult> expectation, IExceptionFilter<TSubject, TResult> exceptionFilter = null)
		{
			return new Specification<TSubject, TResult, TBuilder>(Instrument,
				expectation,
				Builder,
				Source,
				exceptionFilter : exceptionFilter);
		}

		protected abstract THas MakeHas();
		protected abstract TIs MakeIs();

		protected ISpecification<TSubject, TResult, TBuilder> MakeUnboundSpecification(
			IExpectation<TSubject, TResult> expectation, IExceptionFilter<TSubject, TResult> exceptionFilter = null)
		{
			return new Specification<TSubject, TResult, TBuilder>(Instrument,
				expectation,
				Builder,
				Source,
				exceptionFilter : exceptionFilter);
		}
	}

	public abstract class ExpectationBuilder<TSpecification, TSubject, TResult, TBuilder> :
		ExpectationBuilder
			<TSpecification, TSubject, TResult, IHas<TSpecification, TSubject, TResult>,
				INegatableIs<TSpecification, TSubject, TResult, IIs<TSpecification, TSubject, TResult>>, TBuilder>,
		IExpectationBuilder<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification<TBuilder>
		where TBuilder : class, IExpectationBuilder
	{
		protected ExpectationBuilder([NotNull] IInstrument<TSubject, TResult> instrument,
			ISource<TSubject> source = null)
			: base(instrument, source) {}

		protected override IHas<TSpecification, TSubject, TResult> MakeHas()
		{
			var has = new Has<TSpecification, TSubject, TResult>(this);
			return has;
		}

		protected override INegatableIs<TSpecification, TSubject, TResult, IIs<TSpecification, TSubject, TResult>>
			MakeIs()
		{
			return new Is<TSpecification, TSubject, TResult>(this, Negated.False);
		}
	}
}
