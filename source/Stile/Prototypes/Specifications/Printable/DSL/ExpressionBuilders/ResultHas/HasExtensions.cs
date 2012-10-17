#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Sources;
using Stile.Prototypes.Specifications.Printable.Output.Explainers;
using Stile.Prototypes.Specifications.Printable.Output.Explainers.ResultHas;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultHas
{
	public static class HasExtensions
	{
		[Pure]
		public static IFluentSpecification<TSubject, TResult> HashCode<TSubject, TResult>(
			this IHas<TSubject, TResult, IPrintableSource<TSubject>, IPrintableSpecification<TSubject, TResult>> has, int hashCode)
		{
			var state = (IHasState<TSubject, TResult, IPrintableSource<TSubject>>) has;

			Predicate<TResult> predicate = x => x.GetHashCode() == hashCode;
			IExplainer<TSubject, TResult> explainer = new ExplainHashCode<TSubject, TResult>(hashCode);
			return new PrintableSpecification<TSubject, TResult>(state.Source,state.Instrument, predicate, explainer);
		}
		[Pure]
		public static IFluentSpecification<TSubject, TResult> AtLeast<TSubject, TItem, TResult>(
			this IEnumerableHas<TResult, TItem, IPrintableSpecification<TSubject, TResult>> has, int hashCode) where TResult : class, IEnumerable<TItem>
		{
			var state = (IHasState<TSubject, TResult, IPrintableSource<TSubject>>)has;

			Predicate<TResult> predicate = x => x.GetHashCode() == hashCode;
			IExplainer<TSubject, TResult> explainer = new ExplainHashCode<TSubject, TResult>(hashCode);
			return new PrintableSpecification<TSubject, TResult>(state.Instrument, predicate, explainer);
		}
	}
}

