#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.Builders.OfPredicates.Has;
using Stile.Prototypes.Specifications.Builders.OfPredicates.Is;
using Stile.Prototypes.Specifications.Builders.OfSpecifications;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates
{
	public interface IPredicateBuilder {}

	public interface IPredicateBuilder<out TSpecification, TSubject, TResult, out THas, out TIs> :
		IPredicateBuilder
		where TSpecification : class, ISpecification<TSubject, TResult>
		where THas : class, IHas<TSpecification, TSubject, TResult>
		where TIs : class, IResultIs<TSpecification, TResult>
	{
		THas Has { get; }
		TIs Is { get; }
		IThrowingSpecificationBuilder<TSpecification, TSubject> Throws<TException>() where TException : Exception;
	}

	public interface IPredicateBuilder<TSpecification, TSubject, TResult> :
		IPredicateBuilder
			<TSpecification, TSubject, TResult, IHas<TSpecification, TSubject, TResult>,
				INegatableIs<TSpecification, TSubject, TResult, IIs<TSpecification, TSubject, TResult>>>,
		IHides<IPredicateBuilderState<TSubject, TResult>>
		where TSpecification : class, ISpecification<TSubject, TResult> {}

	public interface IPredicateBuilder<TSpecification, TSubject, TResult, TPredicateBuilder> :
		IPredicateBuilder<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>
		where TPredicateBuilder : class, IPredicateBuilder<TSpecification, TSubject, TResult> {}

	public abstract class PredicateBuilder<TSpecification, TSubject, TResult, THas, TIs> :
		IPredicateBuilder<TSpecification, TSubject, TResult, THas, TIs>
		where TSpecification : class, ISpecification<TSubject, TResult>
		where THas : class, IHas, IHas<TSpecification, TSubject, TResult>
		where TIs : class, IResultIs<TSpecification, TResult>
	{
		private readonly Lazy<THas> _lazyHas;
		private readonly Lazy<TIs> _lazyIs;

		protected readonly Specification.Factory<TSpecification, TSubject, TResult> _specificationFactory;

		protected PredicateBuilder([NotNull] IInstrument<TSubject, TResult> instrument,
			Specification.Factory<TSpecification, TSubject, TResult> specificationFactory,
			ISource<TSubject> source = null)
		{
			Source = source;
			Instrument = instrument.ValidateArgumentIsNotNull();
			_specificationFactory = specificationFactory.ValidateArgumentIsNotNull();
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

		public IThrowingSpecificationBuilder<TSpecification, TSubject> Throws<TException>()
			where TException : Exception
		{
			var exceptionFilter = new ExceptionFilter<TSubject, TResult>(exception => exception is TException);
			var builder = new ThrowingSpecificationBuilder<TSpecification, TSubject, TResult>(Source,
				Instrument,
				exceptionFilter,
				_specificationFactory);
			return builder;
		}

		protected abstract THas MakeHas();
		protected abstract TIs MakeIs();
	}

	public class PredicateBuilder<TSpecification, TSubject, TResult> :
		PredicateBuilder
			<TSpecification, TSubject, TResult, IHas<TSpecification, TSubject, TResult>,
				INegatableIs<TSpecification, TSubject, TResult, IIs<TSpecification, TSubject, TResult>>>,
		IPredicateBuilder<TSpecification, TSubject, TResult>,
		IPredicateBuilderState<TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>
	{
		public PredicateBuilder(IInstrument<TSubject, TResult> instrument,
			[NotNull] Specification.Factory<TSpecification, TSubject, TResult> specificationFactory,
			ISource<TSubject> source = null)
			: base(instrument, specificationFactory, source) {}

		public new INegatableIs<TSpecification, TSubject, TResult, IIs<TSpecification, TSubject, TResult>> Is
		{
			get { return base.Is; }
		}

		public IPredicateBuilderState<TSubject, TResult> Xray
		{
			get { return this; }
		}

		protected override IHas<TSpecification, TSubject, TResult> MakeHas()
		{
			var has = new Has<TSpecification, TSubject, TResult>(Instrument, _specificationFactory, Source);
			return has;
		}

		protected override INegatableIs<TSpecification, TSubject, TResult, IIs<TSpecification, TSubject, TResult>>
			MakeIs()
		{
			return new Is<TSpecification, TSubject, TResult>(Instrument, Negated.False, _specificationFactory, Source);
		}
	}

	public abstract class PredicateBuilder<TSpecification, TSubject, TResult, TPredicateBuilder> :
		PredicateBuilder<TSpecification, TSubject, TResult>,
		IPredicateBuilder<TSpecification, TSubject, TResult, TPredicateBuilder>
		where TSpecification : class, ISpecification<TSubject, TResult, TPredicateBuilder>
		where TPredicateBuilder : class, IPredicateBuilder<TSpecification, TSubject, TResult>
	{
		protected PredicateBuilder(IInstrument<TSubject, TResult> instrument,
			[NotNull] Specification.Factory<TSpecification, TSubject, TResult> specificationFactory,
			ISource<TSubject> source = null)
			: base(instrument, specificationFactory, source) {}
	}
}
