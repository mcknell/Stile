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
			[CanBeNull] ISpecification<TSubject, TResult, IFluentExpectationBuilder<TSubject, TResult>> prior)
			: base(instrument, prior) {}

		public override object CloneFor(object specification)
		{
			return new FluentExpectationBuilder<TSubject, TResult>(Inspection,
				specification as ISpecification<TSubject, TResult, IFluentExpectationBuilder<TSubject, TResult>>);
		}

		protected override IFluentExpectationBuilder<TSubject, TResult> Builder
		{
			get { return this; }
		}
		protected override
			Func
				<IExpectation<TSubject, TResult>, IExceptionFilter<TSubject, TResult>,
					ISpecification<TSubject, TResult, IFluentExpectationBuilder<TSubject, TResult>>> SpecFactory
		{
			get { return MakeUnboundSpecification; }
		}
	}
}
