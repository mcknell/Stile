#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Is
{
	public static class IsExtensions
	{
		[Pure]
		public static TSpecification EqualTo<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder, TResult result)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		{
			Func<TResult, bool> accepterForDebugging = x =>
			{
				return x.Equals(result);
			};
			return Is.EqualTo.Make(x => accepterForDebugging.Invoke(x), result, builder.Xray);
		}

		[Pure]
		public static TSpecification Null<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult?> builder)
			where TSpecification : class, ISpecification<TSubject, TResult?>, IChainableSpecification
			where TResult : struct
		{
			var result = new Nullable<TSpecification, TSubject, TResult>(builder.Xray);
			return result.Build();
		}

		[Pure]
		public static TSpecification Null<TSpecification, TSubject, TResult>(
			this IIs<TSpecification, TSubject, TResult> builder)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class
		{
			var result = new Null<TSpecification, TSubject, TResult>(builder.Xray);
			return result.Build();
		}

		[Pure]
		public static TSpecification SequenceEqualTo<TSpecification, TSubject, TResult, TItem>(
			this IEnumerableIs<TSpecification, TSubject, TResult, TItem> builder, IEnumerable<TItem> sequence)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>
		{
			var expectation = new Expectation<TSubject, TResult>(builder.Xray.BuilderState.Inspection,
				x => x.SequenceEquals(sequence) == -1,
				new SequenceEqual<TSpecification, TSubject, TResult, TItem>(builder.Xray, sequence),
				builder.Xray.Negated);
			return builder.Xray.BuilderState.Make(expectation);
		}
	}
}
