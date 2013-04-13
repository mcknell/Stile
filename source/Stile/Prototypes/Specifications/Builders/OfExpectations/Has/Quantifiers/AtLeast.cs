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
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantifiers
{
	public interface IAtLeast<out TSpecification, TSubject, TResult, TItem> :
		IQuantifier<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification, IChainableSpecification
	{
		int Limit { get; }
	}

	public class AtLeast<TSpecification, TSubject, TResult, TItem> :
		Quantifier<TSpecification, TSubject, TResult, TItem>,
		IAtLeast<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification, IChainableSpecification
		where TResult : class, IEnumerable<TItem>
	{
		public AtLeast([NotNull] IHasState<TSpecification, TSubject, TResult> hasState, int limit)
			: base(hasState)
		{
			Limit = limit;
		}

		public int Limit { get; private set; }

		public override void Accept(IExpectationVisitor visitor)
		{
			visitor.Visit4(this);
		}

		public override TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit4(this, data);
		}

		protected override Predicate<TResult> MakePredicate(Expression<Func<TItem, bool>> expression)
		{
			var func = new Lazy<Func<TItem, bool>>(expression.Compile);
			return result => result.Count(func.Value) >= Limit;
		}
	}
}
