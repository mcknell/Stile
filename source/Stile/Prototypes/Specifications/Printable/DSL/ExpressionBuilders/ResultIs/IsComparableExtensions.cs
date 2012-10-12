#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.Printable.Output.Explainers;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs
{
	public static class IsComparableExtensions
	{
		[Pure]
		public static IFluentSpecification<TSubject, TResult> ComparablyEquivalentTo<TSubject, TResult, TSpecifies>(
			this IIs<TSubject, TResult, TSpecifies> builder, TResult result) where TResult : IComparable<TResult>
			where TSpecifies : class, ISpecification<TSubject, TResult>
		{
			return Make(builder, x => x == 0, result, Explain.ComparablyEquivalentTo);
		}

		[Pure]
		public static IFluentSpecification<TSubject, TResult> GreaterThan<TSubject, TResult, TSpecifies>(
			this IIs<TSubject, TResult, TSpecifies> builder, TResult result) where TResult : IComparable<TResult>
			where TSpecifies : class, ISpecification<TSubject, TResult>
		{
			return Make(builder, x => x > 0, result, Explain.GreaterThan);
		}

		[Pure]
		public static IFluentSpecification<TSubject, TResult> GreaterThanOrEqualTo<TSubject, TResult, TSpecifies>(
			this IIs<TSubject, TResult, TSpecifies> builder, TResult result) where TResult : IComparable<TResult>
			where TSpecifies : class, ISpecification<TSubject, TResult>
		{
			return Make(builder, x => x >= 0, result, Explain.GreaterThanOrEqualTo);
		}

		[Pure]
		public static IFluentSpecification<TSubject, TResult> LessThan<TSubject, TResult, TSpecifies>(
			this IIs<TSubject, TResult, TSpecifies> builder, TResult result) where TResult : IComparable<TResult>
			where TSpecifies : class, ISpecification<TSubject, TResult>
		{
			return Make(builder, x => x < 0, result, Explain.LessThan);
		}

		[Pure]
		public static IFluentSpecification<TSubject, TResult> LessThanOrEqualTo<TSubject, TResult, TSpecifies>(
			this IIs<TSubject, TResult, TSpecifies> builder, TResult result) where TResult : IComparable<TResult>
			where TSpecifies : class, ISpecification<TSubject, TResult>
		{
			return Make(builder, x => x <= 0, result, Explain.LessThanOrEqualTo);
		}

		[Pure]
		public static IFluentSpecification<TSubject, TResult> Make<TSubject, TResult, TSpecifies>(
			IIs<TSubject, TResult, TSpecifies> builder,
			Predicate<int> predicate,
			TResult result,
			Func<Explain.FluentSubject<TSubject>, TResult, Negated, Explainer<TSubject, TResult>> explainerFactory)
			where TResult : IComparable<TResult> where TSpecifies : class, ISpecification<TSubject, TResult>
		{
			var state = (IIsState<TSubject, TResult>) builder;
			Predicate<TResult> accepter = x => state.Negated.AgreesWith(predicate.Invoke(x.CompareTo(result)));
			Explainer<TSubject, TResult> explainer = explainerFactory.Invoke(Explain.Subject<TSubject>(),
				result,
				state.Negated);
			var specification = new PrintableSpecification<TSubject, TResult>(state.Instrument, accepter, explainer);
			return specification;
		}
	}
}
