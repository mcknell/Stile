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
	public interface IBoundPredicateBuilder : IPredicateBuilder {}

	public interface IBoundPredicateBuilder<TSpecification, TSubject, TResult> : IBoundPredicateBuilder,
		IPredicateBuilder<TSpecification, TSubject, TResult>
		where TSpecification : class, IBoundSpecification<TSubject, TResult> {}

	public class BoundPredicateBuilder<TSpecification, TSubject, TResult> :
		PredicateBuilder<TSpecification, TSubject, TResult>,
		IBoundPredicateBuilder<TSpecification, TSubject, TResult>
		where TSpecification : class, IBoundSpecification<TSubject, TResult>
	{
		public BoundPredicateBuilder(Instrument<TSubject, TResult> instrument,
			[NotNull] Specification.Factory<TSpecification, TSubject, TResult> specificationFactory,
			ISource<TSubject> source = null)
			: base(instrument, specificationFactory, source) {}
	}
}
