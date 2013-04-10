#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Is
{
	public interface IComparablyEquivalentTo<out TSpecification, TSubject, TResult> :
		IExpectationTerm<IIsState<TSpecification, TSubject, TResult>>
		where TSpecification : class, IChainableSpecification
	{
		TResult Expected { get; }
	}

	public class ComparablyEquivalentTo<TSpecification, TSubject, TResult> :
		ExpectationTerm<IIsState<TSpecification, TSubject, TResult>>,
		IComparablyEquivalentTo<TSpecification, TSubject, TResult>
		where TSpecification : class, IChainableSpecification
	{
		[RuleExpansion(Nonterminal.Enum.Is)]
		public ComparablyEquivalentTo([NotNull] IIsState<TSpecification, TSubject, TResult> prior, TResult expected)
			: base(prior)
		{
			Expected = expected;
		}

		public TResult Expected { get; private set; }

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
