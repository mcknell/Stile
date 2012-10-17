#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders
{
	public static class SubjectBuilderExtensions
	{
		[System.Diagnostics.Contracts.Pure]
		public static IPrintableSubjectBuilder<TBuilder> DescribedBy<TBuilder>(
			this IPrintableSubjectBuilder<TBuilder> builder, [NotNull] Lazy<string> description)
		{
			var state = (IPrintableSubjectBuilderState<TBuilder>) builder;
			return state.Make(description);
		}

		[System.Diagnostics.Contracts.Pure]
		public static IPrintableBoundSubjectBuilder<TBuilder> DescribedBy<TBuilder>(
			this IPrintableBoundSubjectBuilder<TBuilder> builder, [NotNull] Lazy<string> description)
		{
			var state = (IPrintableSubjectBuilderState<TBuilder>) builder;
			return state.MakeBound(description);
		}

		[System.Diagnostics.Contracts.Pure]
		public static IFluentSpecificationBuilder<TSubject, TResult> That<TSubject, TResult>(
			this IPrintableSubjectBuilder<TSubject> builder, Expression<Func<TSubject, TResult>> expression)
		{
			var state = (IPrintableSubjectBuilderState<TSubject>) builder;
			return state.Make(expression);
		}
		[System.Diagnostics.Contracts.Pure]
		public static IFluentBoundSpecificationBuilder<TSubject, TResult> That<TSubject, TResult>(
			this IPrintableBoundSubjectBuilder<TSubject> builder, Expression<Func<TSubject, TResult>> expression)
		{
			var state = (IPrintableSubjectBuilderState<TSubject>) builder;
			return state.MakeBound(expression);
		}

/*

		[System.Diagnostics.Contracts.Pure]
		public static IFluentBoundSpecificationBuilder<TSubject, TResult> That<TSubject, TResult>(
			this IPrintableBoundSubjectBuilder<TSubject> builder, Expression<Func<TSubject, TResult>> expression)
		{
			var state = (IPrintableSubjectBuilderState<TSubject>) builder;
			return new PrintableSpecificationBuilder<TSubject, TResult>(state.Source, expression);
		}
*/
	}

	public static class EnumerableSubjectBuilderExtensions {}
}
