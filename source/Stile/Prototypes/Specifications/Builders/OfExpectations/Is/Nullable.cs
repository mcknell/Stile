#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Is
{
	public interface INullableState<out TSpecification, TSubject, TResult> :
		IExpectationTerm<IIsState<TSpecification, TSubject, TResult?>>
		where TSpecification : class, IChainableSpecification
		where TResult : struct
	{
		TSpecification Build();
	}

	public class Nullable<TSpecification, TSubject, TResult> :
		ExpectationTerm<IIsState<TSpecification, TSubject, TResult?>>,
		INullableState<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult?>, IChainableSpecification
		where TResult : struct
	{
		[RuleExpansion(Nonterminal.Enum.Is)]
		public Nullable([NotNull] IIsState<TSpecification, TSubject, TResult?> prior)
			: base(prior) {}

		public override void Accept(IExpectationVisitor visitor)
		{
			visitor.Visit3(this);
		}

		public override TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit3(this, data);
		}

		public TSpecification Build()
		{
			Expectation<TSubject, TResult?> expectation = Expectation<TSubject>.From(x => x.HasValue == false,
				Prior.Negated,
				Prior.BuilderState.Inspection,
				this);
			return Prior.BuilderState.Make(expectation);
		}
	}
}
