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
	public interface IFluentExpectationBuilder<TSubject, TResult> :
		IExpectationBuilder
			<ISpecification<TSubject, TResult, IFluentExpectationBuilder<TSubject, TResult>>, TSubject, TResult> {}

	public class FluentExpectationBuilder<TSubject, TResult> :
		ExpectationBuilder
			<ISpecification<TSubject, TResult, IFluentExpectationBuilder<TSubject, TResult>>, TSubject, TResult,
				IFluentExpectationBuilder<TSubject, TResult>>,
		IFluentExpectationBuilder<TSubject, TResult>
	{
		public FluentExpectationBuilder(IInstrument<TSubject, TResult> instrument,
			[NotNull] SpecificationFactory
				<TSubject, TResult, IFluentExpectationBuilder<TSubject, TResult>,
					ISpecification<TSubject, TResult, IFluentExpectationBuilder<TSubject, TResult>>> specificationFactory,
			ISource<TSubject> source = null)
			: base(instrument, specificationFactory, source) {}

		protected override IFluentExpectationBuilder<TSubject, TResult> Builder
		{
			get { return this; }
		}
		protected override Func<ICriterion<TResult>, IExceptionFilter<TSubject, TResult>, ISpecification<TSubject, TResult, IFluentExpectationBuilder<TSubject, TResult>>> SpecFactory
		{
			get { return MakeUnboundSpecification; }
		}
	}
}
