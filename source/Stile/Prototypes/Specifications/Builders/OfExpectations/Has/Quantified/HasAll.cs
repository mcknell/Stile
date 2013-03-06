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
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantified
{
	public class HasAll<TSpecification, TSubject, TResult, TItem> :
		QuantifiedHas<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification
		where TResult : class, IEnumerable<TItem>
	{
		public HasAll([NotNull] Func<IExpectation<TSubject, TResult>, TSpecification> factory)
			: base(factory) {}

		protected override Predicate<TResult> MakePredicate(Expression<Func<TItem, bool>> expression)
		{
			return result => result.All(expression.Compile());
		}
	}
}
