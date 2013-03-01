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
	public interface IEnumerableExpectationBuilder<out TSpecification, TSubject, TResult, TItem> :
		IExpectationBuilder
			<TSpecification, TSubject, TResult, IEnumerableHas<TSpecification, TSubject, TResult, TItem>,
				INegatableIs<TSpecification, TSubject, TResult, IEnumerableIs<TSpecification, TSubject, TResult, TItem>>>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		where TResult : class, IEnumerable<TItem> {}

	public class EnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem> :
		ExpectationBuilder
			<TSpecification, TSubject, TResult, IEnumerableHas<TSpecification, TSubject, TResult, TItem>,
				INegatableIs<TSpecification, TSubject, TResult, IEnumerableIs<TSpecification, TSubject, TResult, TItem>>,
				EnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem>>,
		IEnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		where TResult : class, IEnumerable<TItem>
	{
		public EnumerableExpectationBuilder([NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] Specification.Factory
				<TSpecification, TSubject, TResult,
					EnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem>> specificationFactory,
			ISource<TSubject> source = null)
			: base(instrument, specificationFactory, source) {}

		protected override IEnumerableHas<TSpecification, TSubject, TResult, TItem> MakeHas()
		{
			var has = new EnumerableHas<TSpecification, TSubject, TResult, TItem>(Instrument, Make, Source);
			return has;
		}

		protected override
			INegatableIs<TSpecification, TSubject, TResult, IEnumerableIs<TSpecification, TSubject, TResult, TItem>>
			MakeIs()
		{
			throw new NotImplementedException();
		}
	}
}
