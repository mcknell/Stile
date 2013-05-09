#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantifiers
{
	public interface IAtMost<out TSpecification, TSubject, TResult, TItem> :
		ICountedLimit<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification, IChainableSpecification {}

	public class AtMost<TSpecification, TSubject, TResult, TItem> :
		CountedLimit<TSpecification, TSubject, TResult, TItem>,
		IAtMost<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification, IChainableSpecification
		where TResult : class, IEnumerable<TItem>
	{
		public AtMost([NotNull] IHasState<TSpecification, TSubject, TResult> hasState, int limit)
			: base(hasState, limit) {}

		public override void Accept(IExpectationVisitor visitor)
		{
			visitor.Visit4(this);
		}

		public override TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit4(this, data);
		}

		protected override bool Judge(int count)
		{
			return count <= Limit;
		}
	}
}
