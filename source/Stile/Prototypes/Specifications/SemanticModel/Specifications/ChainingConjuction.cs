#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Readability;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface IChainingConjuction {}

	public interface IChainingConjuction<out TSpecification, TSubject, out TState, out TInspection> :
		IChainingConjuction,
		IHides<TState>
		where TSpecification : class, ISpecification<TSubject>
		where TState : class, IChainingConjuctionState<TSpecification, TInspection>
		where TInspection : class, IProcedure
	{
		[System.Diagnostics.Contracts.Pure]
		[RuleExpansion(Nonterminal.Enum.Instrument, Nonterminal.Enum.ExceptionFilter)]
		TSpecification Throws<TException>() where TException : Exception;
	}

	public interface IChainingConjuctionState
	{
		[NotNull]
		object ChainFrom(object specification);
	}

	public interface IChainingConjuctionState<out TSpecification, out TInspection> : IChainingConjuctionState
		where TSpecification : class, IChainableSpecification
		where TInspection : class, IProcedure
	{
		TInspection Inspection { get; }
		[CanBeNull]
		TSpecification Prior { get; }
	}

	public abstract class ChainingConjunction<TSpecification, TSubject, TState, TInspection, TFilter> :
		IChainingConjuction<TSpecification, TSubject, TState, TInspection>,
		IChainingConjuctionState<TSpecification, TInspection>
		where TSpecification : class, ISpecification<TSubject>
		where TState : class, IChainingConjuctionState<TSpecification, TInspection>
		where TInspection : class, IProcedure<TSubject>
		where TFilter : class, IExceptionFilter
	{
		protected ChainingConjunction(TInspection inspection, TSpecification prior)
		{
			Inspection = inspection.ValidateArgumentIsNotNull();
			Prior = prior;
		}

		public TInspection Inspection { get; private set; }
		public TSpecification Prior { get; private set; }
		public TState Xray
		{
			get { return this as TState; }
		}

		public TSpecification Throws<TException>() where TException : Exception
		{
			return Factory(x => x is TException, Inspection, typeof(TException).ToLazyDebugString());
		}

		public abstract object ChainFrom(object specification);

		protected abstract TSpecification Factory(Predicate<Exception> predicate,
			TInspection inspection,
			Lazy<string> description);
	}
}
