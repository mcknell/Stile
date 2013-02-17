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
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates.Has.Quantified
{
	public interface IQuantifiedHas {}

	public interface IQuantifiedHas<out TSpecification, TItem> : IQuantifiedHas
		where TSpecification : class, ISpecification
	{
		[System.Diagnostics.Contracts.Pure]
		TSpecification ItemsSatisfying(Expression<Func<TItem, bool>> expression);
	}

	public abstract class QuantifiedHas<TSpecification, TResult, TItem> : IQuantifiedHas<TSpecification, TItem>
		where TSpecification : class, ISpecification
		where TResult : class, IEnumerable<TItem>
	{
		private readonly Func<ICriterion<TResult>, TSpecification> _factory;

		protected QuantifiedHas([NotNull] Func<ICriterion<TResult>, TSpecification> factory)
		{
			_factory = factory.ValidateArgumentIsNotNull();
		}

		public TSpecification ItemsSatisfying(Expression<Func<TItem, bool>> expression)
		{
			Func<TResult, bool> func = MakePredicate(expression);
			var criterion = new Criterion<TResult>(result => func.Invoke(result) ? Outcome.Succeeded : Outcome.Failed);
			return _factory.Invoke(criterion);
		}

		protected abstract Func<TResult, bool> MakePredicate(Expression<Func<TItem, bool>> expression);
	}
}
