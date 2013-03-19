#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
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
			[CanBeNull] IBoundSpecification<TSubject, TResult, IFluentBoundExpectationBuilder<TSubject, TResult>> prior)
			: base(instrument, prior) {}

		public override object CloneFor(
			object specification)
		{
			return new FluentBoundExpectationBuilder<TSubject, TResult>(Instrument, specification as IBoundSpecification<TSubject, TResult, IFluentBoundExpectationBuilder<TSubject, TResult>>);
		}

		protected override IFluentBoundExpectationBuilder<TSubject, TResult> Builder
		{
			get { return this; }
		}
		protected override
			Func
				<IExpectation<TSubject, TResult>, IExceptionFilter<TSubject, TResult>,
					IBoundSpecification<TSubject, TResult, IFluentBoundExpectationBuilder<TSubject, TResult>>> SpecFactory
		{
			get { return MakeBoundSpecification; }
		}
	}
}
