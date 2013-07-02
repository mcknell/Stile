#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantifiers
{
	public interface ICountedLimit<out TSpecification, TSubject, TResult, TItem> :
		IQuantifier<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification, IChainableSpecification
	{
		int Limit { get; }
	}

	public abstract class CountedLimit<TSpecification, TSubject, TResult, TItem> :
		Quantifier<TSpecification, TSubject, TResult, TItem>,
		ICountedLimit<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification, IChainableSpecification
		where TResult : class, IEnumerable<TItem>
	{
		protected CountedLimit([NotNull] IHasState<TSpecification, TSubject, TResult> hasState, int limit)
			: base(hasState)
		{
			Limit = limit;
		}

		public int Limit { get; private set; }

		protected override Predicate<TResult> GetTest(Func<TItem, bool> predicate)
		{
			return x => Judge(x.Count(predicate));
		}

		protected abstract bool Judge(int count);
	}
}
