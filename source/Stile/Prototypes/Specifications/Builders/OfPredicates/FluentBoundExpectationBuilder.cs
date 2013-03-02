#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfPredicates
{
	public interface IFluentBoundExpectationBuilder<TSubject, TResult> :
		IBoundExpectationBuilder
			<IBoundSpecification<TSubject, TResult, IFluentBoundExpectationBuilder<TSubject, TResult>>, TSubject, TResult> {}

	public class FluentBoundExpectationBuilder<TSubject, TResult> :
		ExpectationBuilder
			<IBoundSpecification<TSubject, TResult, IFluentBoundExpectationBuilder<TSubject, TResult>>, TSubject, TResult,
				IFluentBoundExpectationBuilder<TSubject, TResult>>,
		IFluentBoundExpectationBuilder<TSubject, TResult>
	{
		public FluentBoundExpectationBuilder(IInstrument<TSubject, TResult> instrument,
			[NotNull] SpecificationFactory
				<TSubject, TResult, IFluentBoundExpectationBuilder<TSubject, TResult>,
					IBoundSpecification<TSubject, TResult, IFluentBoundExpectationBuilder<TSubject, TResult>>>
				specificationFactory,
			ISource<TSubject> source = null)
			: base(instrument, specificationFactory, source) {}

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
