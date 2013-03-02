#region License info...
// Propter for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Propter project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates
{
	public interface IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem> :
		IEnumerableExpectationBuilder
			<ISpecification<TSubject, TResult, IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>>, TSubject
				, TResult, TItem>
		where TResult : class, IEnumerable<TItem> {}

	public class FluentEnumerableExpectationBuilder<TSubject, TResult, TItem> :
		EnumerableExpectationBuilder
			<ISpecification<TSubject, TResult, IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>>, TSubject
				, TResult, TItem, IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>>,
		IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>
		where TResult : class, IEnumerable<TItem>
	{
		public FluentEnumerableExpectationBuilder(IInstrument<TSubject, TResult> instrument,
			ISource<TSubject> source = null)
			: base(instrument, source) {}

		protected override IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem> Builder
		{
			get { return this; }
		}
		protected override
			Func
				<ICriterion<TResult>, IExceptionFilter<TSubject, TResult>,
					ISpecification<TSubject, TResult, IFluentEnumerableExpectationBuilder<TSubject, TResult, TItem>>>
			SpecFactory
		{
			get { return MakeUnboundSpecification; }
		}
	}
}
