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

	public class BoundExpectationBuilder<TSpecification, TSubject, TResult, TBuilder> :
		ExpectationBuilder
			<TSpecification, TSubject, TResult, BoundExpectationBuilder<TSpecification, TSubject, TResult, TBuilder>>,
		IBoundExpectationBuilder<TSpecification, TSubject, TResult>
		where TSpecification : class, IBoundSpecification<TSubject, TResult, TBuilder>,
			IChainableSpecification<BoundExpectationBuilder<TSpecification, TSubject, TResult, TBuilder>>
		where TBuilder : class, IExpectationBuilder<TSpecification, TSubject, TResult>
	{
		public BoundExpectationBuilder(IInstrument<TSubject, TResult> instrument,
			[NotNull] SpecificationFactory
				<TSubject, TResult, BoundExpectationBuilder<TSpecification, TSubject, TResult, TBuilder>, TSpecification>
				specificationFactory,
			ISource<TSubject> source = null)
			: base(instrument, specificationFactory, source) {}
	}
}
