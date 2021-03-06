#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Enumerable
{
	public static class EnumerableExpectationBuilderExtensions
	{
		[Pure]
		[RuleFragment(Nonterminal.Enum.EnumerableResult)]
		public static TSpecification Contains<TSpecification, TSubject, TResult, TItem>(
			this IExpectationBuilder<TSpecification, TSubject, TResult> builder,
			[Symbol] TItem item,
			[Symbol] IEqualityComparer<TItem> equalityComparer = null)
			where TSpecification : class,
				ISpecification<TSubject, TResult, IExpectationBuilder<TSpecification, TSubject, TResult>>
			where TResult : class, IEnumerable<TItem>
		{
			var contains = new Contains<TItem>(item);
			Predicate<TResult> predicate = x => x.Contains(item, equalityComparer);
			return builder.Xray.Make(predicate, contains, Negated.False);
		}
	}
}
