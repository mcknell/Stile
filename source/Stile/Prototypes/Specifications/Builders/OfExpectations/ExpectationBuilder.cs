#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Builders.OfSpecifications;
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
		THas Has { get; }
		TIs Is { get; }
		IThrowingSpecificationBuilder<TSpecification, TSubject> Throws<TException>() where TException : Exception;
	}

	public interface IExpectationBuilder<out TSpecification, TSubject, TResult> :
		IExpectationBuilder
			<TSpecification, TSubject, TResult, IHas<TSpecification, TSubject, TResult>,
				INegatableIs<TSpecification, TSubject, TResult, IIs<TSpecification, TSubject, TResult>>>,
		IHides<IExpectationBuilderState<TSpecification, TSubject, TResult>>
		where TSpecification : class, IChainableSpecification {}

	public interface IExpectationBuilderState<out TSpecification, TSubject, TResult>
		where TSpecification : class, IChainableSpecification
	{
		[NotNull]
		IInstrument<TSubject, TResult> Instrument { get; }
		[CanBeNull]
		ISource<TSubject> Source { get; }

		TSpecification Make([NotNull] ICriterion<TResult> criterion,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null);
	}

	public abstract class ExpectationBuilder
	{
		public delegate TSpecification SpecificationFactory<in TSubject, TResult, out TSpecification>(
			ICriterion<TResult> criterion, IExceptionFilter<TSubject, TResult> exceptionFilter = null)
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

		public IThrowingSpecificationBuilder<TSpecification, TSubject> Throws<TException>()
			where TException : Exception
		{
			var exceptionFilter = new ExceptionFilter<TSubject, TResult>(exception => exception is TException);
			var builder = new ThrowingSpecificationBuilder<TSpecification, TSubject, TResult>(exceptionFilter, Make);
			return builder;
		}

		public TSpecification Make(ICriterion<TResult> criterion,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null)
		{
			return SpecFactory.Invoke(criterion, exceptionFilter);
		}

		protected abstract TBuilder Builder { get; }
		protected abstract Func<ICriterion<TResult>, IExceptionFilter<TSubject, TResult>, TSpecification>
			SpecFactory { get; }

		protected IBoundSpecification<TSubject, TResult, TBuilder> MakeBoundSpecification(
			ICriterion<TResult> criterion, IExceptionFilter<TSubject, TResult> exceptionFilter = null)
		{
			return new Specification<TSubject, TResult, TBuilder>(Instrument,
				criterion,
				Builder,
				Source,
				exceptionFilter : exceptionFilter);
		}

		protected abstract THas MakeHas();
		protected abstract TIs MakeIs();

		protected ISpecification<TSubject, TResult, TBuilder> MakeUnboundSpecification(
			ICriterion<TResult> criterion, IExceptionFilter<TSubject, TResult> exceptionFilter = null)
		{
			return new Specification<TSubject, TResult, TBuilder>(Instrument,
				criterion,
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
			var has = new Has<TSpecification, TSubject, TResult>(Instrument, Make, Source);
			return has;
		}

		protected override INegatableIs<TSpecification, TSubject, TResult, IIs<TSpecification, TSubject, TResult>>
			MakeIs()
		{
			return new Is<TSpecification, TSubject, TResult>(Instrument, Negated.False, Make, Source);
		}
	}
}
