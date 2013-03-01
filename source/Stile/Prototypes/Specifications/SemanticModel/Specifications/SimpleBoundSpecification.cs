#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Builders.OfPredicates;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface ISimpleBoundSpecification<TSubject, TResult> :
		IBoundSpecification<TSubject, TResult, ISimpleBoundExpectationBuilder<TSubject, TResult>> {}

	public class SimpleBoundSpecification<TSubject, TResult> :
		Specification<TSubject, TResult, ISimpleBoundExpectationBuilder<TSubject, TResult>>,
		ISimpleBoundSpecification<TSubject, TResult>
	{
		public SimpleBoundSpecification(IInstrument<TSubject, TResult> instrument,
			[NotNull] ICriterion<TResult> criterion,
			[NotNull] ISimpleBoundExpectationBuilder<TSubject, TResult> expectationBuilder,
			ISource<TSubject> source,
			string because = null,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null)
			: base(instrument, criterion, expectationBuilder, source, because, exceptionFilter) {}

		public static ISimpleBoundSpecification<TSubject, TResult> MakeBound([NotNull] ISource<TSubject> source,
			[NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] ICriterion<TResult> criterion,
			[NotNull] ISimpleBoundExpectationBuilder<TSubject, TResult> expectationBuilder,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null)
		{
			return new SimpleBoundSpecification<TSubject, TResult>(instrument,
				criterion,
				expectationBuilder,
				source,
				exceptionFilter : exceptionFilter);
		}
	}
}
