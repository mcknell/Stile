#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.SpecificationBuilders
{
	public interface IEnumerableSpecificationBuilder : ISpecificationBuilder {}

	public interface IEnumerableSpecificationBuilder<out TResult, out TItem, out THas, out TNegatableIs, out TIs,
		out TSpecifies, out TQuantified> : IEnumerableSpecificationBuilder,
			ISpecificationBuilder<TResult, THas, TNegatableIs, TIs, TSpecifies>
		where TResult : class, IEnumerable<TItem>
		where THas : class, IEnumerableHas<TResult, TItem, TSpecifies, TQuantified>
		where TNegatableIs : class, INegatableEnumerableIs<TResult, TItem, TIs, TSpecifies>
		where TIs : class, IEnumerableIs<TResult, TItem, TSpecifies>
		where TSpecifies : class, ISpecification
		where TQuantified : class, IQuantifiedEnumerableHas<TResult, TItem, TSpecifies> {}

	public interface IEnumerableSpecificationBuilder<out TSubject, out TResult, out TItem, out THas, out TNegatableIs,
		out TIs, out TSpecifies, out TQuantified> :
			IEnumerableSpecificationBuilder<TResult, TItem, THas, TNegatableIs, TIs, TSpecifies, TQuantified>,
			ISpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies>
		where TResult : class, IEnumerable<TItem>
		where THas : class, IEnumerableHas<TSubject, TResult, TItem, TSpecifies, TQuantified>
		where TNegatableIs : class, INegatableEnumerableIs<TResult, TItem, TIs, TSpecifies>
		where TIs : class, IEnumerableIs<TResult, TItem, TSpecifies>
		where TSpecifies : class, ISpecification<TSubject, TResult>
		where TQuantified : class, IQuantifiedEnumerableHas<TResult, TItem, TSpecifies> {}

	public abstract class EnumerableSpecificationBuilder<TSubject, TResult, TItem, THas, TNegatableIs, TIs, TSpecifies,
		TQuantified> : SpecificationBuilder<TSubject, TResult, THas, TNegatableIs, TIs, TSpecifies>,
			IEnumerableSpecificationBuilder<TSubject, TResult, TItem, THas, TNegatableIs, TIs, TSpecifies, TQuantified>
		where TResult : class, IEnumerable<TItem>
		where THas : class, IEnumerableHas<TSubject, TResult, TItem, TSpecifies, TQuantified>
		where TNegatableIs : class, INegatableEnumerableIs<TResult, TItem, TIs, TSpecifies>
		where TIs : class, IEnumerableIs<TResult, TItem, TSpecifies>
		where TSpecifies : class, ISpecification<TSubject, TResult>
		where TQuantified : class, IQuantifiedEnumerableHas<TResult, TItem, TSpecifies> {}
}
