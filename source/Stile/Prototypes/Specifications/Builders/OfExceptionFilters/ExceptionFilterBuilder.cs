#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExceptionFilters
{
	public interface IExceptionFilterBuilder : IChainingConjuction {}

	public interface IExceptionFilterBuilder<out TSpecification, TSubject> : IExceptionFilterBuilder,
		IChainingConjuction
			<TSpecification, TSubject, IExceptionFilterBuilderState<TSpecification, TSubject>, IProcedure<TSubject>>
		where TSpecification : class, ISpecification<TSubject> {}

	public interface IExceptionFilterBuilderState : IChainingConjuctionState {}

	public interface IExceptionFilterBuilderState<out TSpecification, TSubject> : IExceptionFilterBuilderState,
		IChainingConjuctionState<TSpecification, IProcedure<TSubject>>
		where TSpecification : class, ISpecification<TSubject> {}

	public abstract class ExceptionFilterBuilder<TSpecification, TSubject, TBuilder> :
		ChainingConjunction
			<TSpecification, TSubject, IExceptionFilterBuilderState<TSpecification, TSubject>, IProcedure<TSubject>,
				ExceptionFilter<TSubject>>,
		IExceptionFilterBuilder<TSpecification, TSubject>,
		IExceptionFilterBuilderState<TSpecification, TSubject>
		where TSpecification : class, IFaultSpecification<TSubject>
		where TBuilder : class, IExceptionFilterBuilder, IHides<IExceptionFilterBuilderState>
	{
		protected ExceptionFilterBuilder([NotNull] IProcedure<TSubject> procedure, [CanBeNull] TSpecification prior)
			: base(procedure, prior) {}

		private TBuilder Builder
		{
			get { return this as TBuilder; }
		}
		protected abstract Func<IExceptionFilter<TSubject>, TSpecification> SpecFactory { get; }

		protected IBoundFaultSpecification<TSubject, TBuilder> Factory(IExceptionFilter<TSubject> exceptionFilter)
		{
			return new FaultSpecification<TSubject, TBuilder>(Inspection, exceptionFilter, Builder, Prior);
		}

		protected override TSpecification Factory(Predicate<Exception> predicate,
			IProcedure<TSubject> inspection,
			Lazy<string> description)
		{
			var exceptionFilter = new ExceptionFilter<TSubject>(predicate, inspection, description);
			return SpecFactory.Invoke(exceptionFilter);
		}
	}
}
