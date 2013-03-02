#region License info...
// Propter for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Propter project on GitHub
#endregion

#region using...
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
				INegatableEnumerableIs
					<TSpecification, TSubject, TResult, IEnumerableIs<TSpecification, TSubject, TResult, TItem>, TItem>>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		where TResult : class, IEnumerable<TItem> {}

	public abstract class EnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem, TBuilder> :
		ExpectationBuilder
			<TSpecification, TSubject, TResult, IEnumerableHas<TSpecification, TSubject, TResult, TItem>,
				INegatableEnumerableIs
					<TSpecification, TSubject, TResult, IEnumerableIs<TSpecification, TSubject, TResult, TItem>, TItem>,
				TBuilder>,
		IEnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification<TBuilder>
		where TResult : class, IEnumerable<TItem>
		where TBuilder : class, IExpectationBuilder
	{
		protected EnumerableExpectationBuilder([NotNull] IInstrument<TSubject, TResult> instrument,
			ISource<TSubject> source = null)
			: base(instrument, source) {}

		protected override IEnumerableHas<TSpecification, TSubject, TResult, TItem> MakeHas()
		{
			var has = new EnumerableHas<TSpecification, TSubject, TResult, TItem>(Instrument, Make, Source);
			return has;
		}

		protected override
			INegatableEnumerableIs
				<TSpecification, TSubject, TResult, IEnumerableIs<TSpecification, TSubject, TResult, TItem>, TItem> MakeIs
			()
		{
			return new EnumerableIs<TSpecification, TSubject, TResult, TItem>(Instrument,
				Negated.False,
				criterion => Make(criterion),
				Source);
		}
	}
}
