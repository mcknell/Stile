#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Builders.OfPredicates.Has;
using Stile.Prototypes.Specifications.Builders.OfPredicates.Is;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates
{
	public interface IEnumerablePredicateBuilder<TSpecification, TSubject, TResult, TItem> :
		IPredicateBuilder
			<TSpecification, TSubject, TResult, IEnumerableHas<TSpecification, TSubject, TResult, TItem>,
				IEnumerablePredicateIs<TSpecification, TSubject, TResult, TItem>>
		where TSpecification : class, ISpecification<TSubject, TResult>
		where TResult : class, IEnumerable<TItem> {}

	public interface IEnumerablePredicateIs<TSpecification, TSubject, out TResult, TItem> :
		IPredicateIs<TSpecification, TSubject, TResult>
		where TResult : class, IEnumerable<TItem>
		where TSpecification : class, ISpecification<TSubject, TResult> {}

	public class EnumerablePredicateBuilder<TSpecification, TSubject, TResult, TItem> :
		PredicateBuilder
			<TSpecification, TSubject, TResult, IEnumerableHas<TSpecification, TSubject, TResult, TItem>,
				IEnumerablePredicateIs<TSpecification, TSubject, TResult, TItem>>,
		IEnumerablePredicateBuilder<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification<TSubject, TResult>
		where TResult : class, IEnumerable<TItem>
	{
		public EnumerablePredicateBuilder([NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] Specification.Factory<TSpecification, TSubject, TResult> specificationFactory,
			ISource<TSubject> source = null)
			: base(instrument, specificationFactory, source) {}

		protected override IEnumerableHas<TSpecification, TSubject, TResult, TItem> MakeHas()
		{
			var has = new EnumerableHas<TSpecification, TSubject, TResult, TItem>(Instrument,
				_specificationFactory,
				Source);
			return has;
		}

		protected override IEnumerablePredicateIs<TSpecification, TSubject, TResult, TItem> MakeIs()
		{
			throw new NotImplementedException();
		}
	}
}
