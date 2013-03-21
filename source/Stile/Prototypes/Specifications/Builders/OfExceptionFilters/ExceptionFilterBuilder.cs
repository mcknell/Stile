#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExceptionFilters
{
	public interface IExceptionFilterBuilder : IChainingConjuction {}

	public interface IExceptionFilterBuilder<out TSpecification, TSubject> : IExceptionFilterBuilder,
		IHides<IExceptionFilterBuilderState<TSpecification, TSubject>>
		where TSpecification : class, IFaultSpecification<TSubject>
	{
		TSpecification Throws<TException>() where TException : Exception;
	}

	public interface IExceptionFilterBuilderState
	{
		[NotNull]
		object CloneFor(object specification);
	}

	public interface IExceptionFilterBuilderState<out TSpecification, TSubject> : IExceptionFilterBuilderState,
		IChainingConjuctionState<TSpecification>
		where TSpecification : class, IFaultSpecification<TSubject>
	{
		Func<IExceptionFilter<TSubject>, TSpecification> Factory { get; }
		[NotNull]
		IProcedure<TSubject> Procedure { get; }
		[CanBeNull]
		ISource<TSubject> Source { get; }
	}

	public abstract class ExceptionFilterBuilder<TSpecification, TSubject, TBuilder> :
		IExceptionFilterBuilder<TSpecification, TSubject>,
		IExceptionFilterBuilderState<TSpecification, TSubject>
		where TSpecification : class, IFaultSpecification<TSubject>
		where TBuilder : class, IExceptionFilterBuilder, IHides<IExceptionFilterBuilderState>
	{
		protected ExceptionFilterBuilder([NotNull] IProcedure<TSubject> procedure,
			[CanBeNull] TSpecification prior,
			ISource<TSubject> source = null)
		{
			Procedure = procedure.ValidateArgumentIsNotNull();
			Prior = prior;
			Source = source;
		}

		public abstract Func<IExceptionFilter<TSubject>, TSpecification> Factory { get; }

		public TSpecification Prior { get; private set; }
		public IProcedure<TSubject> Procedure { get; private set; }
		public ISource<TSubject> Source { get; private set; }
		public IExceptionFilterBuilderState<TSpecification, TSubject> Xray
		{
			get { return this; }
		}

		public TSpecification Throws<TException>() where TException : Exception
		{
			var exceptionFilter = new ExceptionFilter<TSubject>(x => x is TException,
				Procedure,
				typeof(TException).ToLazyDebugString());
			return Factory.Invoke(exceptionFilter);
		}

		public abstract object CloneFor(object specification);
		protected abstract TBuilder Builder { get; }

		protected IBoundFaultSpecification<TSubject, TBuilder> MakeSpecification(
			IExceptionFilter<TSubject> exceptionFilter)
		{
			return new FaultSpecification<TSubject, TBuilder>(Procedure, exceptionFilter, Builder, Prior);
		}
	}
}
