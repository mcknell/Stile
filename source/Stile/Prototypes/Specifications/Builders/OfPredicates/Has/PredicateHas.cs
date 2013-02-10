#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates.Has
{
	public interface IPredicateHas {}

	public interface IPredicateHas<TSpecification, TSubject, out TResult> : IPredicateHas,
		IHides<IPredicateHasState<TSubject, TResult>>
		where TSpecification : class, ISpecification<TSubject, TResult> {}

	public interface IPredicateHasState<TSubject, out TResult>
	{
		IInstrument<TSubject, TResult> Instrument { get; }
		ISource<TSubject> Source { get; }
	}

	public class PredicateHas<TSpecification, TSubject, TResult> : IPredicateHas<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>
	{
		public IPredicateHasState<TSubject, TResult> Xray { get; private set; }
	}

	public interface IEnumerablePredicateHas : IPredicateHas {}

	public interface IEnumerablePredicateHas<TSpecification, TSubject, out TResult, TItem> : IEnumerablePredicateHas,
		IPredicateHas<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>
		where TResult : class, IEnumerable<TItem>
	{
		IQuantifiedEnumerablePredicateHas<TSpecification, TItem> All { get; }
	}

	public interface IQuantifiedEnumerablePredicateHas {}

	public interface IQuantifiedEnumerablePredicateHas<out TSpecification, TItem> : IQuantifiedEnumerablePredicateHas
		where TSpecification : class, ISpecification
	{
		[Pure]
		TSpecification ItemsSatisfying(Expression<Func<TItem, bool>> expression);
	}
}
