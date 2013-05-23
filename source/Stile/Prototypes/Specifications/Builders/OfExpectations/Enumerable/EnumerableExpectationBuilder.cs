#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Enumerable
{
	public interface IEnumerableExpectationBuilder<out TSpecification, TSubject, TResult, TItem> :
		IExpectationBuilder
			<TSpecification, TSubject, TResult, IEnumerableHas<TSpecification, TSubject, TResult, TItem>,
				INegatableEnumerableIs
					<TSpecification, TSubject, TResult, IEnumerableIs<TSpecification, TSubject, TResult, TItem>, TItem>>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		where TResult : class, IEnumerable<TItem> {}

	public abstract class EnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem, TBuilder> :
		ExpectationBuilder
			<TSpecification, TSubject, TResult, IEnumerableHas<TSpecification, TSubject, TResult, TItem>,
				INegatableEnumerableIs
					<TSpecification, TSubject, TResult, IEnumerableIs<TSpecification, TSubject, TResult, TItem>, TItem>,
				TBuilder>,
		IEnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification<TBuilder>
		where TResult : class, IEnumerable<TItem>
		where TBuilder : class, IExpectationBuilder
	{
		[Rule(Nonterminal.Enum.Expectation, NonterminalSymbol.IfEnumerable, Nonterminal.Enum.EnumerableResult)]
		protected EnumerableExpectationBuilder(
			[NotNull] IExpectationBuilderState<IChainableSpecification, TSubject, TResult> state,
			[CanBeNull] TSpecification prior)
			: base(state.Inspection, prior) {}

		protected override IEnumerableHas<TSpecification, TSubject, TResult, TItem> MakeHas()
		{
			var has = new EnumerableHas<TSpecification, TSubject, TResult, TItem>(this);
			return has;
		}

		protected override
			INegatableEnumerableIs
				<TSpecification, TSubject, TResult, IEnumerableIs<TSpecification, TSubject, TResult, TItem>, TItem> MakeIs
			()
		{
			return new EnumerableIs<TSpecification, TSubject, TResult, TItem>(this, Negated.False);
		}
	}
}
