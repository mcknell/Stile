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
	public interface ISimplePredicateBuilder<TSubject, TResult> :
		IPredicateBuilder
			<ISimpleSpecification<TSubject, TResult>, TSubject, TResult, ISimplePredicateBuilder<TSubject, TResult>> {}

	public class SimplePredicateBuilder<TSubject, TResult> :
		PredicateBuilder
			<ISimpleSpecification<TSubject, TResult>, TSubject, TResult, ISimplePredicateBuilder<TSubject, TResult>>,
		ISimplePredicateBuilder<TSubject, TResult>
	{
		public SimplePredicateBuilder(IInstrument<TSubject, TResult> instrument,
			[NotNull] Specification.Factory<ISimpleSpecification<TSubject, TResult>, TSubject, TResult>
				specificationFactory,
			ISource<TSubject> source = null)
			: base(instrument, specificationFactory, source) {}
	}
}