#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Is
{
	public static class IsExtensions
	{
		[Pure]
		[RuleExpansion(Nonterminal.Enum.Is)]
		public static TSpecification EqualTo<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder, [Symbol] TResult result)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		{
			return Is.EqualTo.Make(x => x.Equals(result), result, builder.Xray);
		}

		[Pure]
		[RuleExpansion(Nonterminal.Enum.Is)]
		public static TSpecification Null<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult?> builder)
			where TSpecification : class, ISpecification<TSubject, TResult?>, IChainableSpecification
			where TResult : struct
		{
			var result = new Nullable<TSpecification, TSubject, TResult>(builder.Xray);
			return result.Build();
		}

		[Pure]
		[RuleExpansion(Nonterminal.Enum.Is)]
		public static TSpecification Null<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class
		{
			var result = new Null<TSpecification, TSubject, TResult>(builder.Xray);
			return result.Build();
		}

		[Pure]
		[RuleExpansion(Nonterminal.Enum.EnumerableIs)]
		public static TSpecification SequenceEqualTo<TSpecification, TSubject, TResult, TItem>(
			this IEnumerableIs<TSpecification, TSubject, TResult, TItem> builder,
			[Symbol] IEnumerable<TItem> sequence,
			[Symbol] IEqualityComparer<TItem> equalityComparer = null)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>
		{
			Predicate<TResult> predicate = x => x.SequenceEquals(sequence, equalityComparer) == -1;
			var lastTerm = new SequenceEqual<TSpecification, TSubject, TResult, TItem>(builder.Xray, sequence);
			return builder.Xray.BuilderState.Make(predicate, lastTerm, builder.Xray.Negated);
		}
	}
}
