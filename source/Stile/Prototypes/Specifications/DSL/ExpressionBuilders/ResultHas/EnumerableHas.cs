#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas
{
	public interface IEnumerableHas : IHas {}

	public interface IEnumerableHas<out TResult, out TItem, out TSpecifies, out TQuantified> : IEnumerableHas,
		IHas<TResult, TSpecifies>
		where TResult : class, IEnumerable<TItem>
		where TSpecifies : class, ISpecification
		where TQuantified : class, IQuantifiedEnumerableHas<TResult, TItem, TSpecifies>
	{
		TQuantified All { get; }
	}

	public interface IEnumerableHas<out TSubject, out TResult, out TItem, out TSpecifies, out TQuantified> :
		IEnumerableHas<TResult, TItem, TSpecifies, TQuantified>
		where TResult : class, IEnumerable<TItem>
		where TSpecifies : class, ISpecification<TSubject, TResult>
		where TQuantified : class, IQuantifiedEnumerableHas<TResult, TItem, TSpecifies> {}

	public abstract class EnumerableHas<TSubject, TResult, TItem, TSpecifies, TQuantified> :
		Has<TSubject, TResult, TSpecifies>,
		IEnumerableHas<TSubject, TResult, TItem, TSpecifies, TQuantified>
		where TResult : class, IEnumerable<TItem>
		where TSpecifies : class, ISpecification<TSubject, TResult>
		where TQuantified : class, IQuantifiedEnumerableHas<TResult, TItem, TSpecifies>
	{
		private readonly Lazy<TQuantified> _lazy;

		protected EnumerableHas([NotNull] Lazy<Func<TSubject, TResult>> instrument)
			: base(instrument)
		{
			_lazy = new Lazy<TQuantified>(MakeAll);
		}

		public TQuantified All
		{
			get
			{
				TQuantified quantified = _lazy.Value;
				return quantified;
			}
		}

		protected abstract TQuantified MakeAll();
	}
}
