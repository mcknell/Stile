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
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
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

	public interface IQuantifier<TSpecification, TSubject, TResult, TItem> : IQuantifier<TSpecification, TItem>,
		IHides<IQuantifierState<TSpecification, TSubject, TResult>>
		where TSpecification : class, ISpecification, IChainableSpecification {}

	public interface IQuantifierState : IAcceptExpectationVisitors {}

	public interface IQuantifierState<TSpecification, TSubject, TResult> : IQuantifierState
		where TSpecification : class, IChainableSpecification
	{
		IHasState<TSpecification, TSubject, TResult> Has { get; }
	}

	public abstract class Quantifier<TSpecification, TSubject, TResult, TItem> :
		IQuantifier<TSpecification, TItem>,
		IQuantifierState<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification, IChainableSpecification
		where TResult : class, IEnumerable<TItem>
	{
		private readonly IHasState<TSpecification, TSubject, TResult> _hasState;

		protected Quantifier([NotNull] IHasState<TSpecification, TSubject, TResult> hasState)
		{
			_hasState = hasState.ValidateArgumentIsNotNull();
		}

		public IHasState<TSpecification, TSubject, TResult> Has
		{
			get { return _hasState; }
		}

		public IAcceptExpectationVisitors Parent
		{
			get { return _hasState; }
		}
		public IQuantifierState<TSpecification, TSubject, TResult> Xray
		{
			get { return this; }
		}

		public TSpecification ItemsSatisfying(Expression<Func<TItem, bool>> expression)
		{
			var itemsSatisfying = new ItemsSatisfying<TSpecification, TSubject, TResult, TItem>(expression, this);
			Predicate<TResult> func = MakePredicate(expression);
			var expectation = new Expectation<TSubject, TResult>(_hasState.ExpectationBuilder.Instrument, result => func(result), itemsSatisfying);
			return _hasState.ExpectationBuilder.Make(expectation);
		}

		public abstract void Accept(IExpectationVisitor visitor);
		public abstract TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data);
		protected abstract Predicate<TResult> MakePredicate(Expression<Func<TItem, bool>> expression);
	}
}
