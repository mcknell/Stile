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
	public interface ISimpleSpecification<TSubject, TResult> :
		ISpecification<TSubject, TResult, ISimpleExpectationBuilder<TSubject, TResult>> {}

	public class SimpleSpecification<TSubject, TResult> :
		Specification<TSubject, TResult, ISimpleExpectationBuilder<TSubject, TResult>>,
		ISimpleSpecification<TSubject, TResult>
	{
		public SimpleSpecification(IInstrument<TSubject, TResult> instrument,
			[NotNull] ISimpleExpectationBuilder<TSubject, TResult> expectationBuilder,
			[NotNull] ICriterion<TResult> criterion,
			string because = null,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null)
			: base(instrument, criterion, expectationBuilder, because : because, exceptionFilter : exceptionFilter) {}

		public static ISimpleSpecification<TSubject, TResult> Make([CanBeNull] ISource<TSubject> source,
			[NotNull] IInstrument<TSubject, TResult> instrument,
			[NotNull] ICriterion<TResult> criterion,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null)
		{
			return new SimpleSpecification<TSubject, TResult>(instrument,
				new SimpleExpectationBuilder<TSubject, TResult>(instrument, Make, source),
				criterion,
				exceptionFilter : exceptionFilter);
		}
	}
}
