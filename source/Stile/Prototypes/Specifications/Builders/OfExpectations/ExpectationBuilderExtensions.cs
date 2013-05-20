#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Enumerable;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
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
		[Rule(Nonterminal.Enum.Expectation, UseMethodNameAsSymbol = true)]
		public static ISpecification<TSubject, TResult> GetsMeasured<TSpecification, TSubject, TResult>(
			this IExpectationBuilder<TSpecification, TSubject, TResult> builder)
			where TSpecification : class,
				ISpecification<TSubject, TResult, IExpectationBuilder<TSpecification, TSubject, TResult>>
		{
			return
				MakeMeasuredSpecification
					<TSpecification, TSubject, TResult, IExpectationBuilder<TSpecification, TSubject, TResult>>(builder);
		}

		[Pure]
		public static IBoundSpecification<TSubject, TResult> GetsMeasured<TSpecification, TSubject, TResult>(
			this IBoundExpectationBuilder<TSpecification, TSubject, TResult> builder)
			where TSpecification : class,
				IBoundSpecification<TSubject, TResult, IBoundExpectationBuilder<TSpecification, TSubject, TResult>>
		{
			return
				MakeMeasuredSpecification
					<TSpecification, TSubject, TResult, IBoundExpectationBuilder<TSpecification, TSubject, TResult>>(builder);
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
		[Rule(Nonterminal.Enum.Expectation, UseMethodNameAsSymbol = true)]
		public static ISpecification<TSubject, TResult> WillChangeTo<TSpecification, TSubject, TResult>(
			this IExpectationBuilder<TSpecification, TSubject, TResult> builder, [Symbol] TResult expected)
			where TSpecification : class,
				ISpecification<TSubject, TResult, IExpectationBuilder<TSpecification, TSubject, TResult>>
		{
			return builder.Is.Not.EqualTo(expected).AndThen.Is.EqualTo(expected);
		}

		private static Specification<TSubject, TResult, TBuilder> MakeMeasuredSpecification
			<TSpecification, TSubject, TResult, TBuilder>(TBuilder builder)
			where TSpecification : class, ISpecification<TSubject, TResult, TBuilder>
			where TBuilder : class, IExpectationBuilder<TSpecification, TSubject, TResult>
		{
			Predicate<TResult> predicate = x => true;
			GetsMeasured lastTerm = Is.GetsMeasured.Instance;
			var expectation = new Expectation<TSubject, TResult>(builder.Xray.Inspection,
				predicate,
				Negated.False,
				lastTerm);
			return new Specification<TSubject, TResult, TBuilder>(expectation, builder, expectation, builder.Xray.Prior);
		}
	}
}
