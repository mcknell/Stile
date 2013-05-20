#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Is
{
	public interface ISequenceEqual<out TItem> : IAcceptExpectationVisitors
	{
		IEnumerable<TItem> Expected { get; }
	}

	public class SequenceEqual<TSpecification, TSubject, TResult, TItem> :
		ExpectationTerm<IIsState<TSpecification, TSubject, TResult>>,
		ISequenceEqual<TItem>
		where TSpecification : class, IChainableSpecification
	{
		public SequenceEqual(IIsState<TSpecification, TSubject, TResult> prior,
			IEnumerable<TItem> sequence)
			: base(prior)
		{
			Expected = sequence;
		}

		public IEnumerable<TItem> Expected { get; private set; }

		public override void Accept(IExpectationVisitor visitor)
		{
			visitor.Visit1(this);
		}

		public override TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit1(this, data);
		}
	}
}
