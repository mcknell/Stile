#region License info...
// Propter for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Propter project on GitHub
#endregion

#region using...
using System;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations
{
	public interface IFluentBoundExpectationBuilder<TSubject, TResult> :
		IBoundExpectationBuilder
			<IBoundSpecification<TSubject, TResult, IFluentBoundExpectationBuilder<TSubject, TResult>>, TSubject,
				TResult> {}

	public class FluentBoundExpectationBuilder<TSubject, TResult> :
		ExpectationBuilder
			<IBoundSpecification<TSubject, TResult, IFluentBoundExpectationBuilder<TSubject, TResult>>, TSubject,
				TResult, IFluentBoundExpectationBuilder<TSubject, TResult>>,
		IFluentBoundExpectationBuilder<TSubject, TResult>
	{
		public FluentBoundExpectationBuilder(IInstrument<TSubject, TResult> instrument,
			ISource<TSubject> source = null)
			: base(instrument, source) {}

		protected override IFluentBoundExpectationBuilder<TSubject, TResult> Builder
		{
			get { return this; }
		}
		protected override
			Func
				<ICriterion<TResult>, IExceptionFilter<TSubject, TResult>,
					IBoundSpecification<TSubject, TResult, IFluentBoundExpectationBuilder<TSubject, TResult>>> SpecFactory
		{
			get { return MakeBoundSpecification; }
		}
	}
}
