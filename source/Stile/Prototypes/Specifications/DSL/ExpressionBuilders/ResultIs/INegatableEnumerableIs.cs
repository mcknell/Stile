#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs
{
	public interface INegatableEnumerableIs : IEnumerableIs,
		INegatableIs {}

	public interface INegatableEnumerableIs<out TResult, out TItem, out TNegated, out TSpecifies> :
		INegatableEnumerableIs,
		IEnumerableIs<TResult, TItem, TSpecifies>,
		INegatableIs<TResult, TNegated, TSpecifies>
		where TResult : class, IEnumerable<TItem>
		where TNegated : class, IIs<TResult, TSpecifies>
		where TSpecifies : class, ISpecification {}

	public interface INegatableEnumerableIs<out TSubject, out TResult, out TItem, out TNegated, out TSpecifies> :
		INegatableEnumerableIs<TResult, TItem, TNegated, TSpecifies>,
		IEnumerableIs<TSubject, TResult, TItem, TSpecifies>,
		INegatableIs<TSubject, TResult, TNegated, TSpecifies>
		where TResult : class, IEnumerable<TItem>
		where TNegated : class, IIs<TSubject, TResult, TSpecifies>
		where TSpecifies : class, ISpecification<TSubject, TResult> {}
}
