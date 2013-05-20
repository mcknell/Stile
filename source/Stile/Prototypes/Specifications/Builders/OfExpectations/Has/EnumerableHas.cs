#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantifiers;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has
{
	public interface IEnumerableHas : IHas {}

	public interface IEnumerableHas<out TSpecification, TSubject, TResult, TItem> : IEnumerableHas,
		IHas<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		where TResult : class, IEnumerable<TItem>
	{
		IQuantifier<TSpecification, TItem> All { get; }
		IQuantifier<TSpecification, TItem> No { get; }
	}

	public class EnumerableHas<TSpecification, TSubject, TResult, TItem> : Has<TSpecification, TSubject, TResult>,
		IEnumerableHas<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		where TResult : class, IEnumerable<TItem>
	{
		private readonly Lazy<IQuantifier<TSpecification, TItem>> _lazyAll;
		private readonly Lazy<IQuantifier<TSpecification, TItem>> _lazyNo;

		[RuleExpansion(Nonterminal.Enum.Has, alias : NonterminalSymbol.IfEnumerable)]
		public EnumerableHas([NotNull] IExpectationBuilderState<TSpecification, TSubject, TResult> builderState)
			: base(builderState)
		{
			_lazyAll = MakeLazy(All<TSpecification, TSubject, TResult, TItem>.Make);
			_lazyNo = MakeLazy(No<TSpecification, TSubject, TResult, TItem>.Make);
		}

		public IQuantifier<TSpecification, TItem> All
		{
			get
			{
				IQuantifier<TSpecification, TItem> all = _lazyAll.Value;
				return all;
			}
		}
		public IQuantifier<TSpecification, TItem> No
		{
			get
			{
				IQuantifier<TSpecification, TItem> quantifier = _lazyNo.Value;
				return quantifier;
			}
		}

		protected Lazy<IQuantifier<TSpecification, TItem>> MakeLazy(
			Func<IHasState<TSpecification, TSubject, TResult>, IQuantifier<TSpecification, TItem>> factory)
		{
			return new Lazy<IQuantifier<TSpecification, TItem>>(() => factory.Invoke(Xray));
		}
	}
}
