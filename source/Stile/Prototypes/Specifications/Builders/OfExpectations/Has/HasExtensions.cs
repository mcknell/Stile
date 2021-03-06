#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantifiers;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has
{
	public static class HasExtensions
	{
		[Pure]
		[RuleFragment(Nonterminal.Enum.EnumerableHas)]
		public static IQuantifier<TSpecification, TItem> AtLeast<TSpecification, TSubject, TResult, TItem>(
			this IEnumerableHas<TSpecification, TSubject, TResult, TItem> has, [Symbol] int limit)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>
		{
			return new AtLeast<TSpecification, TSubject, TResult, TItem>(has.Xray, limit);
		}

		[Pure]
		[RuleFragment(Nonterminal.Enum.EnumerableHas)]
		public static IQuantifier<TSpecification, TItem> AtMost<TSpecification, TSubject, TResult, TItem>(
			this IEnumerableHas<TSpecification, TSubject, TResult, TItem> has, [Symbol] int limit)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>
		{
			return new AtMost<TSpecification, TSubject, TResult, TItem>(has.Xray, limit);
		}

		[Pure]
		[RuleFragment(Nonterminal.Enum.EnumerableHas)]
		public static IQuantifier<TSpecification, TItem> Exactly<TSpecification, TSubject, TResult, TItem>(
			this IEnumerableHas<TSpecification, TSubject, TResult, TItem> has, [Symbol] int limit)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>
		{
			return new Exactly<TSpecification, TSubject, TResult, TItem>(has.Xray, limit);
		}

		[Pure]
		[RuleFragment(Nonterminal.Enum.EnumerableHas)]
		public static IQuantifier<TSpecification, TItem> FewerThan<TSpecification, TSubject, TResult, TItem>(
			this IEnumerableHas<TSpecification, TSubject, TResult, TItem> has, [Symbol] int limit)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>
		{
			return new FewerThan<TSpecification, TSubject, TResult, TItem>(has.Xray, limit);
		}

		[Pure]
		[RuleFragment(Nonterminal.Enum.Has)]
		public static TSpecification HashCode<TSpecification, TSubject, TResult>(
			this IHas<TSpecification, TSubject, TResult> has, [Symbol] int hashCode)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		{
			var hashcode = new Hashcode<TSpecification, TSubject, TResult>(has.Xray, hashCode);
			Predicate<TResult> predicate = x => x.GetHashCode() == hashCode;
			TSpecification specification = has.Xray.ExpectationBuilder.Make(predicate, hashcode, Negated.False);
			return specification;
		}

		[Pure]
		[RuleFragment(Nonterminal.Enum.EnumerableHas)]
		public static IQuantifier<TSpecification, TItem> MoreThan<TSpecification, TSubject, TResult, TItem>(
			this IEnumerableHas<TSpecification, TSubject, TResult, TItem> has, [Symbol] int limit)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>
		{
			return new MoreThan<TSpecification, TSubject, TResult, TItem>(has.Xray, limit);
		}
	}
}
