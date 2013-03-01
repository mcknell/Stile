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
	public interface ISimpleExpectationBuilder<TSubject, TResult> :
		IExpectationBuilder<ISimpleSpecification<TSubject, TResult>, TSubject, TResult> {}

	public class SimpleExpectationBuilder<TSubject, TResult> :
		ExpectationBuilder
			<ISimpleSpecification<TSubject, TResult>, TSubject, TResult, SimpleExpectationBuilder<TSubject, TResult>>,
		ISimpleExpectationBuilder<TSubject, TResult>
	{
		public SimpleExpectationBuilder(IInstrument<TSubject, TResult> instrument,
			[NotNull] Specification.Factory<ISimpleSpecification<TSubject, TResult>, TSubject, TResult>
				specificationFactory,
			ISource<TSubject> source = null)
			: base(instrument, (source1, instrument1, criterion, builder, filter) => 
				specificationFactory.Invoke(source1,instrument1,criterion), source) {}
	}
}
