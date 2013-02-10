#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates.Is
{
	public interface IPredicateIs {}

	public interface IResultPredicateIs<TSpecification, out TResult> : IPredicateIs
		where TSpecification : class, IResultSpecification<TResult> {}

	public interface IPredicateIs<TSpecification, TSubject, out TResult> :
		IResultPredicateIs<TSpecification, TResult>,
		IHides<IPredicateIsState<TSpecification, TSubject, TResult>>
		where TSpecification : class, ISpecification<TSubject, TResult> {}

	public interface INegatablePredicateIs : IPredicateIs {}

	public interface INegatablePredicateIs<TSpecification, TSubject, out TResult, out TNegated> :
		INegatablePredicateIs,
		IPredicateIs<TSpecification, TSubject, TResult>,
		INegatable<TNegated>
		where TSpecification : class, ISpecification<TSubject, TResult>
		where TNegated : class, IPredicateIs<TSpecification, TSubject, TResult> {}

	public interface IPredicateIsState<out TSpecification, TSubject, out TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>
	{
		[NotNull]
		IInstrument<TSubject, TResult> Instrument { get; }
		Negated Negated { get; }
		[CanBeNull]
		ISource<TSubject> Source { get; }

		[NotNull]
		TSpecification Make(ICriterion<TResult> criterion);
	}

	public class PredicateIs<TSpecification, TSubject, TResult> :
		INegatablePredicateIs<TSpecification, TSubject, TResult, IPredicateIs<TSpecification, TSubject, TResult>>,
		IPredicateIsState<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>
	{
		private readonly Specification.Factory<TSpecification, TSubject, TResult> _specificationFactory;

		public PredicateIs([NotNull] IInstrument<TSubject, TResult> instrument,
			Negated negated,
			[NotNull] Specification.Factory<TSpecification, TSubject, TResult> specificationFactory,
			ISource<TSubject> source = null)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			Negated = negated;
			_specificationFactory = specificationFactory.ValidateArgumentIsNotNull();
			Source = source;
		}

		public IInstrument<TSubject, TResult> Instrument { get; private set; }
		public Negated Negated { get; private set; }
		public IPredicateIs<TSpecification, TSubject, TResult> Not
		{
			get
			{
				return new PredicateIs<TSpecification, TSubject, TResult>(Instrument,
					Negated.True,
					_specificationFactory,
					Source);
			}
		}
		public ISource<TSubject> Source { get; private set; }
		public IPredicateIsState<TSpecification, TSubject, TResult> Xray
		{
			get { return this; }
		}

		public TSpecification Make(ICriterion<TResult> criterion)
		{
			return _specificationFactory.Invoke(Source, Instrument, criterion);
		}
	}
}
