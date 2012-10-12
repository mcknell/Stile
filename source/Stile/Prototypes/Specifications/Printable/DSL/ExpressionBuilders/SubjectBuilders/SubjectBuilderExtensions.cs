#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Sources;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders;
using Stile.Types;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders
{
	public static class SubjectBuilderExtensions
	{
		[System.Diagnostics.Contracts.Pure]
		public static TBuilder DescribedBy<TBuilder>(this TBuilder builder, [NotNull] Lazy<string> description)
			where TBuilder : class, IPrintableSubjectBuilder
		{
			var state = (IPrintableSubjectBuilderState<TBuilder>) builder;
			TBuilder printableSubjectBuilder = state.Make(description);
			return printableSubjectBuilder;
		}

		[System.Diagnostics.Contracts.Pure]
		public static TBuilder DescribedBy<TBuilder>(this TBuilder builder, [NotNull] string description)
			where TBuilder : class, IPrintableSubjectBuilder
		{
			return DescribedBy(builder, description.ToLazy());
		}

		[System.Diagnostics.Contracts.Pure]
		public static TBuilder DescribedBy<TBuilder>(this TBuilder builder, [NotNull] Func<string> description)
			where TBuilder : class, IPrintableSubjectBuilder
		{
			return DescribedBy(builder, new Lazy<string>(description));
		}

		/// <summary>
		/// Syntactical gimmick for implicitly specifying the type of items an <see cref="IEnumerable{T}"/>,
		/// i.e., type inference will resolve the type without the coder having to hard-code an explicit 
		/// type parameter (which is much easier and more likely to be correct after refactorings).
		/// </summary>
		/// <typeparam name="TSubject"></typeparam>
		/// <typeparam name="TItem"></typeparam>
		/// <param name="builder"></param>
		/// <param name="item">The value of this item is never used; only its type matters.</param>
		/// <returns></returns>
		[System.Diagnostics.Contracts.Pure]
		public static IPrintableBoundEnumerableSubjectBuilder<TSubject, TItem> Of<TSubject, TItem>(
			this IPrintableBoundSubjectBuilder<TSubject> builder, TItem item) where TSubject : class, IEnumerable<TItem>
		{
			var state = (IPrintableBoundSubjectBuilderState<TSubject>) builder;
			return new PrintableBoundEnumerableSubjectBuilder<TSubject, TItem>(state.Source);
		}

		[System.Diagnostics.Contracts.Pure]
		public static TBuilder That<TBuilder>(this TBuilder builder, [NotNull] Func<string> description)
			where TBuilder : class, IPrintableSubjectBuilder
		{
			return DescribedBy(builder, new Lazy<string>(description));
		}

		[System.Diagnostics.Contracts.Pure]
		public static IFluentSpecificationBuilder<TSubject, TResult> That<TSubject, TResult>(
			this IPrintableSubjectBuilder<TSubject> builder, Expression<Func<TSubject, TResult>> expression)
		{
			return new PrintableSpecificationBuilder<TSubject, TResult>(expression);
		}

		[System.Diagnostics.Contracts.Pure]
		public static IFluentBoundSpecificationBuilder<TSubject, TResult> That<TSubject, TResult>(
			this IPrintableBoundSubjectBuilder<TSubject> builder, Expression<Func<TSubject, TResult>> expression)
		{
			var state = (IPrintableBoundSubjectBuilderState<TSubject>)builder;
			return new PrintableBoundSpecificationBuilder<TSubject, TResult>(state.Source, expression);
		}
	}
}
