#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Patterns.Structural.Hierarchy;
using Stile.Prototypes.Specifications.Builders.Lifecycle;
using Stile.Prototypes.Specifications.Builders.OfExceptionFilters;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
using Stile.Types.Primitives;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface IFaultSpecification : IChainableSpecification {}

	public interface IFaultSpecification<TSubject> : IFaultSpecification,
		ISpecification<TSubject>,
		IHides<IFaultSpecificationState<TSubject>> {}

	public interface IFaultSpecification<TSubject, out TExceptionFilterBuilder> : IFaultSpecification<TSubject>,
		IChainableSpecification<TExceptionFilterBuilder>
		where TExceptionFilterBuilder : class, IExceptionFilterBuilder {}

	public interface IFaultSpecificationState {}

	public interface IFaultSpecificationState<TSubject> : IFaultSpecificationState,
		ISpecificationState<TSubject>,
		IHasProcedure<TSubject>,
		ICanGetPredecessors<IFaultSpecification<TSubject>>
	{
		[NotNull]
		IExceptionFilter<TSubject> ExceptionFilter { get; }
		[CanBeNull]
		IFaultSpecification<TSubject> Prior { get; }

		[NotNull]
		[System.Diagnostics.Contracts.Pure]
		IFaultEvaluation<TSubject> Evaluate([NotNull] ISource<TSubject> source,
			[CanBeNull] IFaultEvaluation<TSubject> priorEvaluation,
			[NotNull] IFaultSpecificationState<TSubject> tailSpecification,
			IDeadline deadline = null);
	}

	public class FaultSpecification<TSubject, TExceptionFilterBuilder> :
		Specification<TSubject, IExceptionFilter<TSubject>>,
		IBoundFaultSpecification<TSubject, TExceptionFilterBuilder>,
		IFaultSpecificationState<TSubject>
		where TExceptionFilterBuilder : class, IExceptionFilterBuilder, IHides<IExceptionFilterBuilderState>
	{
		private readonly TExceptionFilterBuilder _filterBuilder;
		private readonly IExceptionFilterBuilderState _filterBuilderState;
		private readonly ISource<TSubject> _source;

		public FaultSpecification([NotNull] IProcedure<TSubject> procedure,
			[NotNull] IExceptionFilter<TSubject> exceptionFilter,
			[NotNull] TExceptionFilterBuilder filterBuilder,
			[CanBeNull] IFaultSpecification<TSubject> prior,
			IDeadline deadline = null,
			string reason = null)
			: this(procedure.Xray.Source, procedure, exceptionFilter, filterBuilder, prior, deadline, reason) {}

		[Rule(Nonterminal.Enum.Specification)]
		private FaultSpecification([NonterminalSymbol] [CanBeNull] ISource<TSubject> source,
			[NonterminalSymbol] [NotNull] IProcedure<TSubject> procedure,
			[NonterminalSymbol(Nonterminal.Enum.Exception)] [NotNull] IExceptionFilter<TSubject> exceptionFilter,
			[NotNull] TExceptionFilterBuilder filterBuilder,
			[CanBeNull] IFaultSpecification<TSubject> prior,
			[NonterminalSymbol] IDeadline deadline = null,
			[NonterminalSymbol] string reason = null)
			: base(exceptionFilter, exceptionFilter, deadline, reason)
		{
			Procedure = procedure.ValidateArgumentIsNotNull();
			_filterBuilder = filterBuilder.ValidateArgumentIsNotNull();
			_filterBuilderState = _filterBuilder.Xray;
			if (_filterBuilderState == null)
			{
				throw new ArgumentException(
					ErrorMessages.FaultSpecification_ctor_filterBuilder.InvariantFormat(filterBuilder.GetType().Name,
						typeof(IExceptionFilterBuilderState).Name));
			}
			Prior = prior;
			_source = source;
		}

		public TExceptionFilterBuilder AndThen
		{
			get { return (TExceptionFilterBuilder) _filterBuilderState.ChainFrom(this); }
		}

		public IAcceptEvaluationVisitors Parent
		{
			get { return null; }
		}

		public IFaultSpecification<TSubject> Prior { get; private set; }
		public IProcedure<TSubject> Procedure { get; private set; }

		public IFaultSpecificationState<TSubject> Xray
		{
			get { return this; }
		}

		public IFaultEvaluation<TSubject> Evaluate(IDeadline deadline = null)
		{
			IFaultSpecification<TSubject> specification = GetPredecessors().LastOrDefault() ?? this;
			return specification.Xray.Evaluate(_source, null, this, deadline);
		}

		public void Accept(ISpecificationVisitor visitor)
		{
			visitor.Visit1(this);
		}

		public TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit1(this, data);
		}

		public TData Accept<TData>(IEvaluationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit1(this, data);
		}

		public void Accept(IEvaluationVisitor visitor)
		{
			visitor.Visit1(this);
		}

		public override ISpecification Clone(IDeadline deadline)
		{
			IDeadline validated = deadline.ValidateArgumentIsNotNull();
			return new FaultSpecification<TSubject, TExceptionFilterBuilder>(Procedure,
				ExceptionFilter,
				_filterBuilder,
				Prior,
				validated,
				Reason);
		}

		public override ISpecification Clone(string reason)
		{
			string because = reason.ValidateArgumentIsNotNull();
			return new FaultSpecification<TSubject, TExceptionFilterBuilder>(Procedure,
				ExceptionFilter,
				_filterBuilder,
				Prior,
				Deadline,
				because);
		}

		public IFaultEvaluation<TSubject> Evaluate(ISource<TSubject> source,
			IFaultEvaluation<TSubject> priorEvaluation,
			IFaultSpecificationState<TSubject> tailSpecification,
			IDeadline deadline = null)
		{
			IObservation<TSubject> observation = Procedure.Observe(source, deadline ?? Deadline);
			observation = ExceptionFilter.Filter(observation);
			Outcome outcome = observation.Evaluate(true);
			return new FaultEvaluation<TSubject>(observation, outcome, tailSpecification, priorEvaluation, this);
		}

		public IEnumerable<IFaultSpecification<TSubject>> GetPredecessors(bool includeSelf = false)
		{
			IFaultSpecification<TSubject> prior = this;
			if (includeSelf)
			{
				yield return prior;
			}
			while (prior.Xray.Prior != null)
			{
				prior = prior.Xray.Prior;
				yield return prior;
			}
		}

		IAcceptSpecificationVisitors IHasParent<IAcceptSpecificationVisitors>.Parent
		{
			get { return ExceptionFilter; }
		}
	}
}
