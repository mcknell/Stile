#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantifiers
{
	public class HasAll<TSpecification, TSubject, TResult, TItem> :
		Quantifier<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification, IChainableSpecification
		where TResult : class, IEnumerable<TItem>
	{
		public HasAll([NotNull] IHasState<TSpecification, TSubject, TResult> hasState)
			: base(hasState) {}

		protected override Predicate<TResult> MakePredicate(Expression<Func<TItem, bool>> expression)
		{
			return result => result.All(expression.Compile());
		}
	}
}
