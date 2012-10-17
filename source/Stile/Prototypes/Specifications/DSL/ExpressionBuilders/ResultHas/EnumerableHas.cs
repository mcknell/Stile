#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas.Quantifiers;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders.Sources;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
#endregion

namespace Stile.Prototypes.Specifications.DSL.ExpressionBuilders.ResultHas
{
	public interface IEnumerableHas : IHas {}

	public interface IEnumerableHas<out TResult, TItem, out TSpecifies> : IEnumerableHas,
		IHas<TResult, TSpecifies>
		where TResult : class, IEnumerable<TItem>
		where TSpecifies : class, ISpecification
	{
		IQuantifiedEnumerableHas<TItem, TSpecifies> All { get; }
	}

	public interface IEnumerableHas<out TSubject, out TResult, TItem, out TSource, out TSpecifies> :
		IEnumerableHas<TResult, TItem, TSpecifies>,
		IHas<TSubject, TResult, TSource, TSpecifies>
		where TResult : class, IEnumerable<TItem>
		where TSource : class, ISource<TSubject>
		where TSpecifies : class, ISpecification<TSubject, TResult> {}

	public class EnumerableHas<TSubject, TResult, TItem, TSource, TSpecifies> :
		Has<TSubject, TResult, TSource, TSpecifies>,
		IEnumerableHas<TSubject, TResult, TItem, TSource, TSpecifies>
		where TResult : class, IEnumerable<TItem>
		where TSource : class, ISource<TSubject>
		where TSpecifies : class, ISpecification<TSubject, TResult>
	{
		private readonly Lazy<IQuantifiedEnumerableHas<TItem, TSpecifies>> _lazy;

		public EnumerableHas([NotNull] TSource source, [NotNull] Lazy<Func<TSubject, TResult>> instrument)
			: base(source, instrument)
		{
			_lazy = new Lazy<IQuantifiedEnumerableHas<TItem, TSpecifies>>(() => new HasAll<TResult, TItem, TSpecifies>());
		}

		public IQuantifiedEnumerableHas<TItem, TSpecifies> All
		{
			get
			{
				IQuantifiedEnumerableHas<TItem, TSpecifies> quantified = _lazy.Value;
				return quantified;
			}
		}
	}
}
