#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantifiers;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has
{
	public static class HasExtensions
	{
		[Pure]
		[RuleExpansion(Nonterminal.Enum.EnumerableHas, "AtLeast \"limit\"")]
		public static IQuantifier<TSpecification, TItem> AtLeast<TSpecification, TSubject, TResult, TItem>(
			this IEnumerableHas<TSpecification, TSubject, TResult, TItem> has, int limit)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>
		{
			return new AtLeast<TSpecification, TSubject, TResult, TItem>(has.Xray, limit);
		}

		[Pure]
		[RuleExpansion(Nonterminal.Enum.EnumerableHas, "AtMost \"limit\"")]
		public static IQuantifier<TSpecification, TItem> AtMost<TSpecification, TSubject, TResult, TItem>(
			this IEnumerableHas<TSpecification, TSubject, TResult, TItem> has, int limit)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>
		{
			return new AtMost<TSpecification, TSubject, TResult, TItem>(has.Xray, limit);
		}

		[Pure]
		[RuleExpansion(Nonterminal.Enum.EnumerableHas, "Exactly \"limit\"")]
		public static IQuantifier<TSpecification, TItem> Exactly<TSpecification, TSubject, TResult, TItem>(
			this IEnumerableHas<TSpecification, TSubject, TResult, TItem> has, int limit)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>
		{
			return new Exactly<TSpecification, TSubject, TResult, TItem>(has.Xray, limit);
		}

		[Pure]
		[RuleExpansion(Nonterminal.Enum.Has)]
		public static TSpecification HashCode<TSpecification, TSubject, TResult>(
			this IHas<TSpecification, TSubject, TResult> has, int hashCode)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		{
			var hashcode = new Hashcode<TSpecification, TSubject, TResult>(has.Xray, hashCode);
			var expectation = new Expectation<TSubject, TResult>(has.Xray.ExpectationBuilder.Instrument,
				x => x.GetHashCode() == hashCode,
				hashcode);
			TSpecification specification = has.Xray.ExpectationBuilder.Make(expectation);
			return specification;
		}
	}
}
