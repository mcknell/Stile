#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates
{
	public interface IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem> :
		IEnumerableExpectationBuilder
			<ISpecification<TSubject, TResult, IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>>, TSubject,
				TResult, TItem>
		where TResult : class, IEnumerable<TItem> {}

	public class FluentEnumerableExpectationBuilder<TSubject, TResult, TItem> :
		EnumerableExpectationBuilder
			<ISpecification<TSubject, TResult, IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>>, TSubject,
				TResult, TItem, IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>>,
		IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>
		where TResult : class, IEnumerable<TItem>
	{
		public FluentEnumerableExpectationBuilder(IInstrument<TSubject, TResult> instrument,
			[NotNull] SpecificationFactory
				<TSubject, TResult, IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>,
					ISpecification<TSubject, TResult, IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>>>
				specificationFactory,
			ISource<TSubject> source = null)
			: base(instrument, specificationFactory, source) {}
	}
}
