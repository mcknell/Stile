#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantifiers
{
	public interface IAll<out TSpecification, TSubject, TResult, TItem> :
		IQuantifier<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification, IChainableSpecification {}

	public class All<TSpecification, TSubject, TResult, TItem> :
		Quantifier<TSpecification, TSubject, TResult, TItem>,
		IAll<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification, IChainableSpecification
		where TResult : class, IEnumerable<TItem>
	{
		[RuleFragment(Nonterminal.Enum.EnumerableHas)]
		public All([NotNull] IHasState<TSpecification, TSubject, TResult> hasState)
			: base(hasState) {}

		public override void Accept(IExpectationVisitor visitor)
		{
			visitor.Visit4(this);
		}

		public override TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit4(this, data);
		}

		public static All<TSpecification, TSubject, TResult, TItem> Make(
			[NotNull] IHasState<TSpecification, TSubject, TResult> hasState)
		{
			return new All<TSpecification, TSubject, TResult, TItem>(hasState);
		}

		protected override Predicate<TResult> MakePredicate(Expression<Func<TItem, bool>> expression)
		{
			var func = new Lazy<Func<TItem, bool>>(expression.Compile);
			return result => result.All(func.Value);
		}
	}
}
