#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
#endregion

#region using...
using System.Collections.Generic;
using System.Diagnostics.Contracts;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders
{
	public static class SpecificationBuilderExtensions
	{
		/// <summary>
		/// Syntactical gimmick for implicitly specifying the type of items an <see cref="IEnumerable{T}"/>,
		/// i.e., type inference will resolve the type without the coder having to hard-code an explicit 
		/// type parameter (which is much easier and more likely to be correct after refactorings).
		/// </summary>
		/// <typeparam name="TSubject"></typeparam>
		/// <typeparam name="TResult"> </typeparam>
		/// <typeparam name="TItem"></typeparam>
		/// <param name="builder"></param>
		/// <param name="item">The value of this item is never used; only its type matters.</param>
		/// <returns></returns>
		[Pure]
		public static IFluentEnumerableSpecificationBuilder<TSubject, TResult, TItem> Of<TSubject, TResult, TItem>(
			this IFluentSpecificationBuilder<TSubject, TResult> builder, TItem item)
			where TSubject : class, IEnumerable<TItem> where TResult : class, IEnumerable<TItem>
		{
			var state =
				(
				IPrintableSpecificationBuilderState
					<TSubject, TResult, IFluentSpecification<TSubject, TResult>, IPrintableEvaluation<TResult>>) builder;
			return new PrintableEnumerableSpecificationBuilder<TSubject, TResult, TItem>(state.Source, state.Instrument);
		}

		/// <summary>
		/// Syntactical gimmick for implicitly specifying the type of items an <see cref="IEnumerable{T}"/>,
		/// i.e., type inference will resolve the type without the coder having to hard-code an explicit 
		/// type parameter (which is much easier and more likely to be correct after refactorings).
		/// </summary>
		/// <typeparam name="TSubject"></typeparam>
		/// <typeparam name="TResult"> </typeparam>
		/// <typeparam name="TItem"></typeparam>
		/// <param name="builder"></param>
		/// <param name="item">The value of this item is never used; only its type matters.</param>
		/// <returns></returns>
		[Pure]
		public static IFluentBoundEnumerableSpecificationBuilder<TSubject, TResult, TItem> Of<TSubject, TResult, TItem>(
			this IFluentBoundSpecificationBuilder<TSubject, TResult> builder, TItem item)
			where TSubject : class, IEnumerable<TItem> where TResult : class, IEnumerable<TItem>
		{
			var state =
				(
				IPrintableSpecificationBuilderState
					<TSubject, TResult, IFluentSpecification<TSubject, TResult>, IPrintableEvaluation<TResult>>) builder;
			return new PrintableEnumerableSpecificationBuilder<TSubject, TResult, TItem>(state.Source, state.Instrument);
		}
	}
}
