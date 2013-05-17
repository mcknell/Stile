#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel;
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
		private static readonly Predicate<TResult> Predicate = x => x.None();

		[RuleExpansion(Nonterminal.Enum.Is, NonterminalSymbol.IfEnumerable)]
		public EnumerableIs([NotNull] IExpectationBuilderState<TSpecification, TSubject, TResult> builderState,
			Negated negated)
			: base(builderState, negated) {}

		public TSpecification Empty
		{
			get
			{
				var lastTerm = new Empty<TSpecification, TSubject, TResult>(this);
				TSpecification specification = BuilderState.Make(Predicate, lastTerm, Negated);
				return specification;
			}
		}
		public new IEnumerableIs<TSpecification, TSubject, TResult, TItem> Not
		{
			get { return new EnumerableIs<TSpecification, TSubject, TResult, TItem>(BuilderState, Negated.True); }
		}
	}
}
