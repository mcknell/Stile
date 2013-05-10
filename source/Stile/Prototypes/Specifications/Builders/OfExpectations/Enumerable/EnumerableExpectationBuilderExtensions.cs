#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Enumerable
{
	public static class EnumerableExpectationBuilderExtensions
	{
		[Pure]
		[RuleExpansion(Nonterminal.Enum.EnumerableResult, "Contains \"item\"")]
		public static TSpecification Contains<TSpecification, TSubject, TResult, TItem>(
			this IExpectationBuilder<TSpecification, TSubject, TResult> builder, TItem item)
			where TSpecification : class,
				ISpecification<TSubject, TResult, IExpectationBuilder<TSpecification, TSubject, TResult>>
			where TResult : class, IEnumerable<TItem>
		{
			var contains = new Contains<TItem>(item);
			var expectation = new Expectation<TSubject, TResult>(builder.Xray.Instrument,
				x => x.Contains(item),
				contains);
			return builder.Xray.Make(expectation);
		}
	}
}
