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
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates
{
	public interface IPredicateBuilder {}

	public interface IPredicateBuilder<TSpecification, TSubject, TResult, out THas, out TIs> : IPredicateBuilder
		where TSpecification : class, ISpecification<TSubject, TResult>
		where THas : class, IPredicateHas<TSpecification, TSubject, TResult>
		where TIs : class, IResultPredicateIs<TSpecification, TResult>
	{
		THas Has { get; }
		TIs Is { get; }
	}

	public interface IPredicateBuilder<TSpecification, TSubject, TResult> :
		IPredicateBuilder
			<TSpecification, TSubject, TResult, IPredicateHas<TSpecification, TSubject, TResult>,
				INegatablePredicateIs<TSpecification, TSubject, TResult, IPredicateIs<TSpecification, TSubject, TResult>>>,
		IHides<IPredicateBuilderState<TSubject, TResult>>
		where TSpecification : class, ISpecification<TSubject, TResult> {}

	public abstract class PredicateBuilder<TSpecification, TSubject, TResult, THas, TIs> :
		IPredicateBuilder<TSpecification, TSubject, TResult, THas, TIs>
		where TSpecification : class, ISpecification<TSubject, TResult>
		where THas : class, IPredicateHas, IPredicateHas<TSpecification, TSubject, TResult>
		where TIs : class, IResultPredicateIs<TSpecification, TResult>
	{
		private readonly Lazy<THas> _lazyHas;
		private readonly Lazy<TIs> _lazyIs;

		protected readonly Specification.Factory<TSpecification, TSubject, TResult> _specificationFactory;

		protected PredicateBuilder([NotNull] Instrument<TSubject, TResult> instrument,
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

		protected abstract THas MakeHas();
		protected abstract TIs MakeIs();
	}

	public class PredicateBuilder<TSpecification, TSubject, TResult> :
		PredicateBuilder
			<TSpecification, TSubject, TResult, IPredicateHas<TSpecification, TSubject, TResult>,
				INegatablePredicateIs<TSpecification, TSubject, TResult, IPredicateIs<TSpecification, TSubject, TResult>>>,
		IPredicateBuilder<TSpecification, TSubject, TResult>,
		IPredicateBuilderState<TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>
	{
		public PredicateBuilder(Instrument<TSubject, TResult> instrument,
			[NotNull] Specification.Factory<TSpecification, TSubject, TResult> specificationFactory,
			ISource<TSubject> source = null)
			: base(instrument, specificationFactory, source) {}

		public new
			INegatablePredicateIs<TSpecification, TSubject, TResult, IPredicateIs<TSpecification, TSubject, TResult>>
			Is
		{
			get { return base.Is; }
		}

		public IPredicateBuilderState<TSubject, TResult> Xray
		{
			get { return this; }
		}

		protected override IPredicateHas<TSpecification, TSubject, TResult> MakeHas()
		{
			var has = new PredicateHas<TSpecification, TSubject, TResult>(Instrument, _specificationFactory, Source);
			return has;
		}

		protected override
			INegatablePredicateIs<TSpecification, TSubject, TResult, IPredicateIs<TSpecification, TSubject, TResult>>
			MakeIs()
		{
			return new PredicateIs<TSpecification, TSubject, TResult>(Instrument,
				Negated.False,
				_specificationFactory, Source);
		}
	}
}
