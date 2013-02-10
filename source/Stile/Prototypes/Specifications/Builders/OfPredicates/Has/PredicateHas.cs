#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates.Has
{
	public interface IPredicateHas {}

	public interface IPredicateHas<TSpecification, TSubject, out TResult> : IPredicateHas,
		IHides<IPredicateHasState<TSpecification, TSubject, TResult>>
		where TSpecification : class, ISpecification<TSubject, TResult> {}

	public interface IPredicateHasState<TSpecification, TSubject, out TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>
	{
		[NotNull]
		IInstrument<TSubject, TResult> Instrument { get; }
		[CanBeNull]
		ISource<TSubject> Source { get; }

		[NotNull]
		TSpecification Make(ICriterion<TResult> criterion);
	}

	public class PredicateHas<TSpecification, TSubject, TResult> :
		IPredicateHas<TSpecification, TSubject, TResult>,
		IPredicateHasState<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>
	{
		private readonly Specification.Factory<TSpecification, TSubject, TResult> _specificationFactory;

		public PredicateHas([NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] Specification.Factory<TSpecification, TSubject, TResult> specificationFactory,
			ISource<TSubject> source = null)
		{
			Instrument = instrument.ValidateArgumentIsNotNull();
			_specificationFactory = specificationFactory.ValidateArgumentIsNotNull();
			Source = source;
		}

		public IInstrument<TSubject, TResult> Instrument { get; private set; }
		public ISource<TSubject> Source { get; private set; }

		public IPredicateHasState<TSpecification, TSubject, TResult> Xray
		{
			get { return this; }
		}

		public TSpecification Make(ICriterion<TResult> criterion)
		{
			var specification = _specificationFactory.Invoke(Source, Instrument, criterion);
			return specification;
		}
	}

	public interface IEnumerablePredicateHas : IPredicateHas {}

	public interface IEnumerablePredicateHas<TSpecification, TSubject, out TResult, TItem> :
		IEnumerablePredicateHas,
		IPredicateHas<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>
		where TResult : class, IEnumerable<TItem>
	{
		IQuantifiedEnumerablePredicateHas<TSpecification, TItem> All { get; }
	}

	public interface IQuantifiedEnumerablePredicateHas {}

	public interface IQuantifiedEnumerablePredicateHas<out TSpecification, TItem> :
		IQuantifiedEnumerablePredicateHas
		where TSpecification : class, ISpecification
	{
		[System.Diagnostics.Contracts.Pure]
		TSpecification ItemsSatisfying(Expression<Func<TItem, bool>> expression);
	}
}
