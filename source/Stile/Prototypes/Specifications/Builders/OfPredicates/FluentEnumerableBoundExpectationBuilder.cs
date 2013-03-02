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
	public interface IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem> :
		IEnumerableExpectationBuilder
			<IBoundSpecification<TSubject, TResult, IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem>>,
				TSubject, TResult, TItem>
		where TResult : class, IEnumerable<TItem> {}

	public class FluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem> :
		EnumerableExpectationBuilder
			<IBoundSpecification<TSubject, TResult, IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem>>,
				TSubject, TResult, TItem, IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem>>,
		IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem>
		where TResult : class, IEnumerable<TItem>
	{
		public FluentEnumerableBoundExpectationBuilder(IInstrument<TSubject, TResult> instrument,
			[NotNull] SpecificationFactory
				<TSubject, TResult, IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem>,
					IBoundSpecification<TSubject, TResult, IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem>>>
				specificationFactory,
			ISource<TSubject> source = null)
			: base(instrument, specificationFactory, source) {}
	}
}
