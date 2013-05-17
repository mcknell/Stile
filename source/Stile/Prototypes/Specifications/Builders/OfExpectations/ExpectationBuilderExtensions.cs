#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Enumerable;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations
{
	public static class ExpectationBuilderExtensions
	{
		[Pure]
		public static IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem> CollectingItemsLike
			<TSpecification, TSubject, TResult, TItem>(
			this IBoundExpectationBuilder<TSpecification, TSubject, TResult> builder, TItem throwaway)
			where TSpecification : class,
				IBoundSpecification<TSubject, TResult, IBoundExpectationBuilder<TSpecification, TSubject, TResult>>
			where TResult : class, ICollection<TItem>
		{
			return new FluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem>(builder.Xray, null);
		}

		[Pure]
		public static IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem> OfItemsLike
			<TSpecification, TSubject, TResult, TItem>(
			this IExpectationBuilder<TSpecification, TSubject, TResult> builder, TItem throwaway)
			where TSpecification : class,
				ISpecification<TSubject, TResult, IExpectationBuilder<TSpecification, TSubject, TResult>>
			where TResult : class, IEnumerable<TItem>
		{
			return new FluentEnumerableExpectationBuilder<TSubject, TResult, TItem>(builder.Xray, null);
		}

		[Pure]
		public static IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem> OfItemsLike
			<TSpecification, TSubject, TResult, TItem>(
			this IBoundExpectationBuilder<TSpecification, TSubject, TResult> builder, TItem throwaway)
			where TSpecification : class,
				IBoundSpecification<TSubject, TResult, IBoundExpectationBuilder<TSpecification, TSubject, TResult>>
			where TResult : class, IEnumerable<TItem>
		{
			return new FluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem>(builder.Xray, null);
		}
		[Pure]
		public static ISpecification<TSubject, TResult> WillChangeTo
			<TSpecification, TSubject, TResult>(
			this IExpectationBuilder<TSpecification, TSubject, TResult> builder, TResult expected)
			where TSpecification : class,
				ISpecification<TSubject, TResult, IExpectationBuilder<TSpecification, TSubject, TResult>>
		{
			return builder.Is.Not.EqualTo(expected).AndThen.Is.EqualTo(expected);
		}
	}
}
