#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultIs
{
	public interface IEnumerableIs : IIs {}

	public interface IEnumerableIs<out TResult, out TItem, out TSpecifies> : IEnumerableIs,
		IIs<TResult, TSpecifies>
		where TResult : class, IEnumerable<TItem>
		where TSpecifies : class, ISpecification {}

	public interface IEnumerableIs<out TSubject, out TResult, out TItem, out TSpecifies> :
		IEnumerableIs<TResult, TItem, TSpecifies>,
		IIs<TSubject, TResult, TSpecifies>
		where TResult : class, IEnumerable<TItem>
		where TSpecifies : class, ISpecification<TSubject, TResult> {}

	public abstract class EnumerableIs<TSubject, TResult, TItem, TNegated, TSpecifies, TEvaluation> :
		Is<TSubject, TResult, TNegated, TSpecifies>,
		IEnumerableIs<TSubject, TResult, TItem, TSpecifies>,
		IEnumerableIsState<TSubject, TResult, TItem>
		where TResult : class, IEnumerable<TItem>
		where TNegated : class, IIs<TSubject, TResult, TSpecifies>
		where TSpecifies : class, ISpecification<TSubject, TResult, TEvaluation>
		where TEvaluation : class, IEvaluation<TResult>
	{
		protected EnumerableIs(Negated negated, [NotNull] Lazy<Func<TSubject, TResult>> instrument)
			: base(negated, instrument) {}
	}
}
