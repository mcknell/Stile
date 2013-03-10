#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has
{
	public interface IHashcodeState<out TSpecification, TSubject, TResult> :
		IExpectationTerm<IHasState<TSpecification, TSubject, TResult>>
		where TSpecification : class, IChainableSpecification
	{
		int Expected { get; }
	}

	public class Hashcode<TSpecification, TSubject, TResult> :
		ExpectationTerm<IHasState<TSpecification, TSubject, TResult>>,
		IHashcodeState<TSpecification, TSubject, TResult>
		where TSpecification : class, IChainableSpecification
	{
		public Hashcode([NotNull] IHasState<TSpecification, TSubject, TResult> prior, int expected)
			: base(prior)
		{
			Expected = expected;
		}

		public int Expected { get; private set; }

		public override void Accept(IExpectationVisitor visitor)
		{
			visitor.Visit3(this);
		}

		public override TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit3(this, data);
		}
	}
}
