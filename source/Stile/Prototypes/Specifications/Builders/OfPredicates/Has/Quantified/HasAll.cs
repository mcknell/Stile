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

namespace Stile.Prototypes.Specifications.Builders.OfPredicates.Has.Quantified
{
	public class HasAll<TSpecification, TResult, TItem> : QuantifiedHas<TSpecification, TResult, TItem>
		where TSpecification : class, ISpecification
		where TResult : class, IEnumerable<TItem>
	{
		public HasAll([NotNull] Func<ICriterion<TResult>, TSpecification> factory)
			: base(factory) {}

		protected override Func<TResult, bool> MakePredicate(Expression<Func<TItem, bool>> expression)
		{
			return result => result.All(expression.Compile());
		}
	}
}
