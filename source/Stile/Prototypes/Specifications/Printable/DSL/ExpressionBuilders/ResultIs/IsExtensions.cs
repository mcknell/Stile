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
using Stile.Prototypes.Specifications.Printable.Output.Explainers.ResultIs;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs
{
	public static class IsExtensions
	{
		[Pure]
		public static IFluentSpecification<TSubject, TResult> EqualTo<TSubject, TResult, TSpecifies>(
			this IIs<TSubject, TResult, TSpecifies> builder, TResult result) where TResult : IEquatable<TResult>
			where TSpecifies : class, ISpecification<TSubject, TResult>
		{
			var state = (IIsState<TSubject, TResult>) builder;
			Predicate<TResult> accepter = x => state.Negated.AgreesWith(x.Equals(result));
			ExplainIs<TSubject, TResult> explainer = Explain.Subject<TSubject>().Is(result, state.Negated);
			return Make(builder, accepter, explainer);
		}

		[Pure]
		public static IFluentSpecification<TSubject, bool> False<TSubject, TSpecifies>(
			this IIs<TSubject, bool, TSpecifies> builder) where TSpecifies : class, ISpecification<TSubject, bool>
		{
			var state = (IIsState<TSubject, bool>) builder;
			Predicate<bool> accepter = x => state.Negated.AgreesWith(!x);
			ExplainFalse<TSubject, bool> explainer = Explain.Subject<TSubject>().False(state.Negated);
			return Make(builder, accepter, explainer);
		}

		[Pure]
		public static IFluentSpecification<TSubject, TResult> Make<TSubject, TResult, TSpecifies>(
			IIs<TSubject, TResult, TSpecifies> builder, Predicate<TResult> accepter, Explainer<TSubject, TResult> explainer)
			where TSpecifies : class, ISpecification<TSubject, TResult>
		{
			var state = (IIsState<TSubject, TResult>) builder;
			var specification = new PrintableSpecification<TSubject, TResult>(state.Instrument, accepter, explainer);
			return specification;
		}

		[Pure]
		public static IFluentSpecification<TSubject, TResult> Null<TSubject, TResult, TSpecifies>(
			this IIs<TSubject, TResult, TSpecifies> builder) where TResult : class
			where TSpecifies : class, ISpecification<TSubject, TResult>
		{
			var state = (IIsState<TSubject, TResult>) builder;
			Predicate<TResult> accepter = x => state.Negated.AgreesWith(x == null);
			ExplainNull<TSubject, TResult> explainer = Explain.Subject<TSubject>().Null<TSubject, TResult>(state.Negated);
			return Make(builder, accepter, explainer);
		}

		[Pure]
		public static IFluentSpecification<TSubject, TResult?> Null<TSubject, TResult, TSpecifies>(
			this IIs<TSubject, TResult?, TSpecifies> builder) where TResult : struct
			where TSpecifies : class, ISpecification<TSubject, TResult?>
		{
			var state = (IIsState<TSubject, TResult?>) builder;
			Predicate<TResult?> accepter = x => state.Negated.AgreesWith(x == null);
			ExplainNull<TSubject, TResult?> explainer = Explain.Subject<TSubject>().Null<TSubject, TResult?>(state.Negated);
			return Make(builder, accepter, explainer);
		}

		[Pure]
		public static IFluentSpecification<TSubject, bool> True<TSubject, TSpecifies>(
			this IIs<TSubject, bool, TSpecifies> builder) where TSpecifies : class, ISpecification<TSubject, bool>
		{
			var state = (IIsState<TSubject, bool>) builder;
			Predicate<bool> accepter = x => state.Negated.AgreesWith(x);
			ExplainTrue<TSubject, bool> explainer = Explain.Subject<TSubject>().True(state.Negated);
			return Make(builder, accepter, explainer);
		}
	}
}
