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
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantified
{
	public interface IQuantifiedHas {}

	public interface IQuantifiedHas<out TSpecification, TItem> : IQuantifiedHas
		where TSpecification : class, ISpecification
	{
		[System.Diagnostics.Contracts.Pure]
		TSpecification ItemsSatisfying(Expression<Func<TItem, bool>> expression);
	}

	public abstract class QuantifiedHas<TSpecification, TSubject, TResult, TItem> :
		IQuantifiedHas<TSpecification, TItem>
		where TSpecification : class, ISpecification
		where TResult : class, IEnumerable<TItem>
	{
		private readonly Func<IExpectation<TSubject, TResult>, TSpecification> _factory;

		protected QuantifiedHas([NotNull] Func<IExpectation<TSubject, TResult>, TSpecification> factory)
		{
			_factory = factory.ValidateArgumentIsNotNull();
		}

		public TSpecification ItemsSatisfying(Expression<Func<TItem, bool>> expression)
		{
			Predicate<TResult> func = MakePredicate(expression);
			var expectation = new Expectation<TSubject, TResult>(result => func(result), Clause.HasItemsSatisfying);
			return _factory.Invoke(expectation);
		}

		protected abstract Predicate<TResult> MakePredicate(Expression<Func<TItem, bool>> expression);
	}
}
