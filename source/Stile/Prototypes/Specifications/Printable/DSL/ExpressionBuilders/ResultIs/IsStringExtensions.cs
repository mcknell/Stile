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
using Stile.Prototypes.Specifications.Printable.Output.Explainers.ResultIs.Strings;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs
{
	public static class IsStringExtensions
	{
		[Pure]
		public static TSpecifies NullOrEmpty<TSubject, TSpecifies>(this IIs<TSubject, string, TSpecifies> builder)
			where TSpecifies : class, IPrintableSpecification<TSubject, string>
		{
			var state = (IFluentIsState<TSubject, string, TSpecifies>) builder;
			Predicate<string> accepter = x => state.Negated.AgreesWith(string.IsNullOrEmpty(x));
			ExplainStringNullOrEmpty<TSubject> explainer = Explain.Subject<TSubject>().NullOrEmpty(state.Negated);
			return state.Make(accepter, explainer);
		}

		[Pure]
		public static TSpecifies NullOrWhitespace<TSubject, TSpecifies>(this IIs<TSubject, string, TSpecifies> builder)
			where TSpecifies : class, IPrintableSpecification<TSubject, string>
		{
			var state = (IFluentIsState<TSubject, string, TSpecifies>) builder;
			Predicate<string> accepter = x => state.Negated.AgreesWith(string.IsNullOrWhiteSpace(x));
			ExplainStringNullOrWhitespace<TSubject> explainer = Explain.Subject<TSubject>().NullOrWhitespace(state.Negated);
			return state.Make(accepter, explainer);
		}

		[Pure]
		public static TSpecifies StringContaining<TSubject, TSpecifies>(this IIs<TSubject, string, TSpecifies> builder,
			string expected) where TSpecifies : class, IPrintableSpecification<TSubject, string>
		{
			var state = (IFluentIsState<TSubject, string, TSpecifies>) builder;
			Predicate<string> accepter = x => state.Negated.AgreesWith(x.Contains(expected));
			ExplainStringContaining<TSubject> explainer = Explain.Subject<TSubject>().Containing(expected, state.Negated);
			return state.Make(accepter, explainer);
		}

		[Pure]
		public static TSpecifies StringEndingWith<TSubject, TSpecifies>(this IIs<TSubject, string, TSpecifies> builder,
			string expected) where TSpecifies : class, IPrintableSpecification<TSubject, string>
		{
			var state = (IFluentIsState<TSubject, string, TSpecifies>) builder;
			Predicate<string> accepter = x => state.Negated.AgreesWith(x.EndsWith(expected));
			ExplainStringEndingWith<TSubject> explainer = Explain.Subject<TSubject>().EndingWith(expected, state.Negated);
			return state.Make(accepter, explainer);
		}

		[Pure]
		public static TSpecifies StringStartingWith<TSubject, TSpecifies>(this IIs<TSubject, string, TSpecifies> builder,
			string expected) where TSpecifies : class, IPrintableSpecification<TSubject, string>
		{
			var state = (IFluentIsState<TSubject, string, TSpecifies>) builder;
			Predicate<string> accepter = x => state.Negated.AgreesWith(x.StartsWith(expected));
			ExplainStringStartingWith<TSubject> explainer = Explain.Subject<TSubject>().StartingWith(expected, state.Negated);
			return state.Make(accepter, explainer);
		}
	}
}
