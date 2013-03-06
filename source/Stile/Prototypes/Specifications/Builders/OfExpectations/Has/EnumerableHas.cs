#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantified;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has
{
	public interface IEnumerableHas : IHas {}

	public interface IEnumerableHas<out TSpecification, TSubject, TResult, TItem> : IEnumerableHas,
		IHas<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		where TResult : class, IEnumerable<TItem>
	{
		IQuantifiedHas<TSpecification, TItem> All { get; }
	}

	public class EnumerableHas<TSpecification, TSubject, TResult, TItem> : Has<TSpecification, TSubject, TResult>,
		IEnumerableHas<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		where TResult : class, IEnumerable<TItem>
	{
		private readonly Lazy<IQuantifiedHas<TSpecification, TItem>> _lazyAll;

		public EnumerableHas([NotNull] IExpectationBuilderState<TSpecification, TSubject, TResult> builderState)
			: base(builderState)
		{
			_lazyAll =
				new Lazy<IQuantifiedHas<TSpecification, TItem>>(
					() => new HasAll<TSpecification, TSubject, TResult, TItem>(expectation => Make(expectation)));
		}

		public IQuantifiedHas<TSpecification, TItem> All
		{
			get
			{
				IQuantifiedHas<TSpecification, TItem> all = _lazyAll.Value;
				return all;
			}
		}
	}
}
