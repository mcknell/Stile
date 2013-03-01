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
	public interface ISimpleBoundExpectationBuilder<TSubject, TResult> :
		IBoundExpectationBuilder<ISimpleBoundSpecification<TSubject, TResult>, TSubject, TResult> { }

	public class SimpleBoundExpectationBuilder<TSubject, TResult> :
		ExpectationBuilder
			<ISimpleBoundSpecification<TSubject, TResult>, TSubject, TResult,
				SimpleBoundExpectationBuilder<TSubject, TResult>>,
		ISimpleBoundExpectationBuilder<TSubject, TResult>
	{
		public SimpleBoundExpectationBuilder(IInstrument<TSubject, TResult> instrument,
			[NotNull] Specification.Factory
				<ISimpleBoundSpecification<TSubject, TResult>, TSubject, TResult,
					ISimpleBoundExpectationBuilder<TSubject, TResult>> specificationFactory,
			ISource<TSubject> source = null)
			: base(instrument, specificationFactory, source) {}
	}
}
