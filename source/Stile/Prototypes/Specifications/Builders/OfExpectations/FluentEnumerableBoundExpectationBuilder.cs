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

namespace Stile.Prototypes.Specifications.Builders.OfExpectations
{
	public interface IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem> :
		IEnumerableExpectationBuilder
			<IBoundSpecification<TSubject, TResult, IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem>>
				, TSubject, TResult, TItem>
		where TResult : class, IEnumerable<TItem> {}

	public class FluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem> :
		EnumerableExpectationBuilder
			<IBoundSpecification<TSubject, TResult, IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem>>
				, TSubject, TResult, TItem, IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem>>,
		IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem>
		where TResult : class, IEnumerable<TItem>
	{
		public FluentEnumerableBoundExpectationBuilder(IInstrument<TSubject, TResult> instrument,
			ISource<TSubject> source = null)
			: base(instrument, source) {}

		public FluentEnumerableBoundExpectationBuilder(
			IExpectationBuilderState
				<
					IBoundSpecification
						<TSubject, TResult, IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem>>, TSubject,
					TResult> state,
			ISource<TSubject> source = null)
			: base(state.Instrument, source) {}

		protected override IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem> Builder
		{
			get { return this; }
		}
		protected override
			Func
				<ICriterion<TResult>, IExceptionFilter<TSubject, TResult>,
					IBoundSpecification
						<TSubject, TResult, IFluentEnumerableBoundExpectationBuilder<TSubject, TResult, TItem>>> SpecFactory
		{
			get { return MakeBoundSpecification; }
		}
	}
}
