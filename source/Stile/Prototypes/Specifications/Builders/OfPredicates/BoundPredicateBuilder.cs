#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates
{
	public interface IBoundExpectationBuilder : IExpectationBuilder {}

	public interface IBoundExpectationBuilder<out TSpecification, TSubject, TResult> : IBoundExpectationBuilder,
		IExpectationBuilder<TSpecification, TSubject, TResult>
		where TSpecification : class, IBoundSpecification<TSubject, TResult>, IChainableSpecification {}

	public class BoundExpectationBuilder<TSpecification, TSubject, TResult, TPredicateBuilder> :
		ExpectationBuilder
			<TSpecification, TSubject, TResult,
				BoundExpectationBuilder<TSpecification, TSubject, TResult, TPredicateBuilder>>,
		IBoundExpectationBuilder<TSpecification, TSubject, TResult>
		where TSpecification : class, IBoundSpecification<TSubject, TResult, TPredicateBuilder>
		where TPredicateBuilder : class, IExpectationBuilder<TSpecification, TSubject, TResult>
	{
		public BoundExpectationBuilder(IInstrument<TSubject, TResult> instrument,
			[NotNull] Specification.Factory
				<TSpecification, TSubject, TResult,
					BoundExpectationBuilder<TSpecification, TSubject, TResult, TPredicateBuilder>> specificationFactory,
			ISource<TSubject> source = null)
			: base(instrument, specificationFactory, source) {}
	}
}
