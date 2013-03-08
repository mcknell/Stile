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
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantifiers
{
	public interface IQuantifier {}

	public interface IQuantifier<out TSpecification, TItem> : IQuantifier
		where TSpecification : class, ISpecification
	{
		[System.Diagnostics.Contracts.Pure]
		TSpecification ItemsSatisfying(Expression<Func<TItem, bool>> expression);
	}

	public abstract class Quantifier<TSpecification, TSubject, TResult, TItem> :
		IQuantifier<TSpecification, TItem>
		where TSpecification : class, ISpecification, IChainableSpecification
		where TResult : class, IEnumerable<TItem>
	{
		private readonly IHasState<TSpecification, TSubject, TResult> _hasState;

		protected Quantifier([NotNull] IHasState<TSpecification, TSubject, TResult> hasState)
		{
			_hasState = hasState.ValidateArgumentIsNotNull();
		}

		public TSpecification ItemsSatisfying(Expression<Func<TItem, bool>> expression)
		{
			Predicate<TResult> func = MakePredicate(expression);
			var expectation = new Expectation<TSubject, TResult>(result => func(result), Clause.HasItemsSatisfying,
				_hasState.ExpectationBuilder.Instrument);
			return _hasState.ExpectationBuilder.Make(expectation);
		}

		protected abstract Predicate<TResult> MakePredicate(Expression<Func<TItem, bool>> expression);
	}
}
