#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Is
{
	public interface IEnumerableIs<out TSpecification, TSubject, TResult, TItem> :
		IIs<TSpecification, TSubject, TResult>
		where TResult : class, IEnumerable<TItem>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
	{
		[System.Diagnostics.Contracts.Pure]
		TSpecification Empty { get; }
	}

	public interface INegatableEnumerableIs<out TSpecification, TSubject, TResult, out TNegated, TItem> :
		IEnumerableIs<TSpecification, TSubject, TResult, TItem>,
		INegatable<TNegated>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		where TNegated : class, IEnumerableIs<TSpecification, TSubject, TResult, TItem>
		where TResult : class, IEnumerable<TItem> {}

	public class EnumerableIs<TSpecification, TSubject, TResult, TItem> : Is<TSpecification, TSubject, TResult>,
		INegatableEnumerableIs
			<TSpecification, TSubject, TResult, IEnumerableIs<TSpecification, TSubject, TResult, TItem>, TItem>
		where TSpecification : class, IChainableSpecification, ISpecification<TSubject, TResult>
		where TResult : class, IEnumerable<TItem>
	{
		private static readonly Expression<Predicate<TResult>> _all = x => x.None();
		private readonly Lazy<TSpecification> _lazyEmpty;

		public EnumerableIs([NotNull] IExpectationBuilderState<TSpecification, TSubject, TResult> builderState,
			Negated negated)
			: base(builderState, negated)
		{
			_lazyEmpty =
				new Lazy<TSpecification>(
					() => builderState.Make(Expectation<TSubject>.From(_all, negated, builderState.Instrument, this)));
		}

		public TSpecification Empty
		{
			get
			{
				TSpecification specification = _lazyEmpty.Value;
				return specification;
			}
		}
		public new IEnumerableIs<TSpecification, TSubject, TResult, TItem> Not
		{
			get { return new EnumerableIs<TSpecification, TSubject, TResult, TItem>(BuilderState, Negated.True); }
		}
	}
}
