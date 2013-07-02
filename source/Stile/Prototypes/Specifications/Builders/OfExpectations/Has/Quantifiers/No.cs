#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantifiers
{
	public interface INo<out TSpecification, TSubject, TResult, TItem> :
		IQuantifier<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification, IChainableSpecification {}

	public class No<TSpecification, TSubject, TResult, TItem> :
		Quantifier<TSpecification, TSubject, TResult, TItem>,
		INo<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification, IChainableSpecification
		where TResult : class, IEnumerable<TItem>
	{
		[RuleFragment(Nonterminal.Enum.EnumerableHas)]
		public No([NotNull] IHasState<TSpecification, TSubject, TResult> hasState)
			: base(hasState) {}

		public override void Accept(IExpectationVisitor visitor)
		{
			visitor.Visit4(this);
		}

		public override TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit4(this, data);
		}

		public static No<TSpecification, TSubject, TResult, TItem> Make(
			[NotNull] IHasState<TSpecification, TSubject, TResult> hasState)
		{
			return new No<TSpecification, TSubject, TResult, TItem>(hasState);
		}

		protected override Predicate<TResult> GetTest(Func<TItem, bool> predicate)
		{
			return x => x.None(predicate);
		}
	}
}
