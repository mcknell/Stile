#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Types.Comparison;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Is
{
	public static class IsComparableExtensions
	{
		[Pure]
		[RuleExpansion(Nonterminal.Enum.ComparableExpectationTerm)]
		public static TSpecification ComparablyEquivalentTo<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder, [Symbol] TResult expected)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : IComparable<TResult>
		{
			return Make(builder.Xray, expected, ComparisonRelation.Equal);
		}

		[Pure]
		[RuleExpansion(Nonterminal.Enum.ComparableExpectationTerm)]
		public static TSpecification GreaterThan<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder, [Symbol] TResult expected)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : IComparable<TResult>
		{
			return Make(builder.Xray, expected, ComparisonRelation.GreaterThan);
		}

		[Pure]
		[RuleExpansion(Nonterminal.Enum.ComparableExpectationTerm)]
		public static TSpecification GreaterThanOrEqualTo<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder, [Symbol] TResult expected)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : IComparable<TResult>
		{
			return Make(builder.Xray, expected, ComparisonRelation.GreaterThanOrEqual);
		}

		[Pure]
		[RuleExpansion(Nonterminal.Enum.ComparableExpectationTerm)]
		public static TSpecification LessThan<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder, [Symbol] TResult expected)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : IComparable<TResult>
		{
			return Make(builder.Xray, expected, ComparisonRelation.LessThan);
		}

		[Pure]
		[RuleExpansion(Nonterminal.Enum.ComparableExpectationTerm)]
		public static TSpecification LessThanOrEqualTo<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder, [Symbol] TResult expected)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : IComparable<TResult>
		{
			return Make(builder.Xray, expected, ComparisonRelation.LessThanOrEqual);
		}

		private static TSpecification Make<TSpecification, TSubject, TResult>(
			IIsState<TSpecification, TSubject, TResult> state, TResult expected, ComparisonRelation comparison)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : IComparable<TResult>
		{
			var term = new ComparableExpectationTerm<TSpecification, TSubject, TResult>(state, expected, comparison);
			Predicate<TResult> predicate = x => comparison.PassesFor(x, expected);
			return state.BuilderState.Make(predicate, term, state.Negated);
		}
	}
}
