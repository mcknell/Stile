#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates.Is
{
	public interface IIs {}

	public interface IResultIs<out TSpecification, out TResult> : IIs
		where TSpecification : class, IChainableSpecification {}

	public interface IIs<out TSpecification, TSubject, out TResult> : IResultIs<TSpecification, TResult>,
		IHides<IIsState<TSpecification, TSubject, TResult>>
		where TSpecification : class, IChainableSpecification {}

	public interface INegatableIs : IIs {}

	public interface INegatableIs<out TSpecification, TSubject, out TResult, out TNegated> : INegatableIs,
		IIs<TSpecification, TSubject, TResult>,
		INegatable<TNegated>
		where TSpecification : class, IChainableSpecification
		where TNegated : class, IIs<TSpecification, TSubject, TResult> {}

	public interface IIsState<out TSpecification, TSubject, out TResult>
		where TSpecification : class, IChainableSpecification
	{
		[NotNull]
		IInstrument<TSubject, TResult> Instrument { get; }
		Negated Negated { get; }
		[CanBeNull]
		ISource<TSubject> Source { get; }

		[NotNull]
		TSpecification Make(ICriterion<TResult> criterion);
	}

	public class Is<TSpecification, TSubject, TResult> :
		INegatableIs<TSpecification, TSubject, TResult, IIs<TSpecification, TSubject, TResult>>,
		IIsState<TSpecification, TSubject, TResult>
		where TSpecification : class, IChainableSpecification
	{
		private readonly Func<ICriterion<TResult>, TSpecification> _specificationFactory;

		public Is([NotNull] IInstrument<TSubject, TResult> instrument,
			Negated negated,
			[NotNull] Func<ICriterion<TResult>, TSpecification> specificationFactory,
			ISource<TSubject> source = null)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			Negated = negated;
			_specificationFactory = specificationFactory.ValidateArgumentIsNotNull();
			Source = source;
		}

		public IInstrument<TSubject, TResult> Instrument { get; private set; }
		public Negated Negated { get; private set; }
		public IIs<TSpecification, TSubject, TResult> Not
		{
			get { return new Is<TSpecification, TSubject, TResult>(Instrument, Negated.True, _specificationFactory, Source); }
		}
		public ISource<TSubject> Source { get; private set; }
		public IIsState<TSpecification, TSubject, TResult> Xray
		{
			get { return this; }
		}

		public TSpecification Make(ICriterion<TResult> criterion)
		{
			return _specificationFactory.Invoke(criterion);
		}
	}
}
