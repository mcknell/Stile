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
		IHides<IExceptionFilterBuilderState<TSpecification, TSubject>>
		where TSpecification : class, ISpecification<TSubject>
	{
		TSpecification Throws<TException>() where TException : Exception;
	}

	public interface IExceptionFilterBuilderState
	{
		[NotNull]
		object ChainFrom(object specification);
	}

	public interface IExceptionFilterBuilderState<out TSpecification, TSubject> : IExceptionFilterBuilderState,
		IChainingConjuctionState<TSpecification, IProcedure<TSubject>>
		where TSpecification : class, ISpecification<TSubject>
	{
		Func<IExceptionFilter<TSubject>, TSpecification> Factory { get; }
		[CanBeNull]
		ISource<TSubject> Source { get; }
	}

	public abstract class ExceptionFilterBuilder<TSpecification, TSubject, TBuilder> :
		ChainingConjunction
			<TSpecification, TSubject, IExceptionFilterBuilderState<TSpecification, TSubject>, IProcedure<TSubject>,
				ExceptionFilter<TSubject>>,
		IExceptionFilterBuilder<TSpecification, TSubject>,
		IExceptionFilterBuilderState<TSpecification, TSubject>
		where TSpecification : class, IFaultSpecification<TSubject>
		where TBuilder : class, IExceptionFilterBuilder, IHides<IExceptionFilterBuilderState>
	{
		protected ExceptionFilterBuilder([NotNull] IProcedure<TSubject> procedure,
			[CanBeNull] TSpecification prior,
			ISource<TSubject> source = null)
			: base(procedure, prior)
		{
			Source = source;
		}

		public abstract Func<IExceptionFilter<TSubject>, TSpecification> Factory { get; }

		public ISource<TSubject> Source { get; private set; }
		public override IExceptionFilterBuilderState<TSpecification, TSubject> Xray
		{
			get { return this; }
		}

		public abstract object ChainFrom(object specification);
		protected abstract TBuilder Builder { get; }

		protected IBoundFaultSpecification<TSubject, TBuilder> MakeSpecification(
			IExceptionFilter<TSubject> exceptionFilter)
		{
			return new FaultSpecification<TSubject, TBuilder>(Inspection, exceptionFilter, Builder, Prior);
		}

		protected override TSpecification SpecFactor(Predicate<Exception> predicate,
			IProcedure<TSubject> inspection,
			Lazy<string> description)
		{
			var exceptionFilter = new ExceptionFilter<TSubject>(predicate, inspection, description);
			return Factory.Invoke(exceptionFilter);
		}
	}
}
