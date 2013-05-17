#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations
{
	public interface IExpectationBuilder : IChainingConjuction {}

	public interface IExpectationBuilder<out TSpecification, TSubject, TResult, out THas, out TIs> :
		IExpectationBuilder
		where TSpecification : class, IChainableSpecification
		where THas : class, IHas<TSpecification, TSubject, TResult>
		where TIs : class, IIs<TSpecification, TSubject, TResult>
	{
		[System.Diagnostics.Contracts.Pure]
		[Rule(Nonterminal.Enum.Expectation, CanBeInlined = false)]
		THas Has { get; }
		[System.Diagnostics.Contracts.Pure]
		[Rule(Nonterminal.Enum.Expectation, "Is Not?")]
		TIs Is { get; }
	}

	public interface IExpectationBuilder<out TSpecification, TSubject, TResult> :
		IExpectationBuilder
			<TSpecification, TSubject, TResult, IHas<TSpecification, TSubject, TResult>,
				INegatableIs<TSpecification, TSubject, TResult, IIs<TSpecification, TSubject, TResult>>>,
		IChainingConjuction
			<TSpecification, TSubject, IExpectationBuilderState<TSpecification, TSubject, TResult>,
				IInstrument<TSubject, TResult>>
		where TSpecification : class, ISpecification<TSubject> {}

	public interface IExpectationBuilderState : IChainingConjuctionState {}

	public interface IExpectationBuilderState<out TSpecification, TSubject, TResult> : IExpectationBuilderState,
		IChainingConjuctionState<TSpecification, IInstrument<TSubject, TResult>>
		where TSpecification : class, IChainableSpecification
	{
		TSpecification Make([NotNull] Expression<Predicate<TResult>> expression,
			[NotNull] IAcceptExpectationVisitors lastTerm,
			Negated negated,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null);

		TSpecification Make([NotNull] Predicate<TResult> predicate,
			[NotNull] IAcceptExpectationVisitors lastTerm,
			Negated negated,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null);
	}

	public abstract class ExpectationBuilder<TSpecification, TSubject, TResult, THas, TIs, TBuilder> :
		ChainingConjunction
			<TSpecification, TSubject, IExpectationBuilderState<TSpecification, TSubject, TResult>,
				IInstrument<TSubject, TResult>, ExceptionFilter<TSubject, TResult>>,
		IExpectationBuilder<TSpecification, TSubject, TResult, THas, TIs>,
		IExpectationBuilderState<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification<TBuilder>
		where THas : class, IHas, IHas<TSpecification, TSubject, TResult>
		where TIs : class, IIs<TSpecification, TSubject, TResult>
		where TBuilder : class, IExpectationBuilder

	{
		private readonly Lazy<THas> _lazyHas;
		private readonly Lazy<TIs> _lazyIs;

		protected ExpectationBuilder([NotNull] IInstrument<TSubject, TResult> instrument,
			[CanBeNull] TSpecification prior)
			: base(instrument, prior)
		{
			_lazyHas = new Lazy<THas>(MakeHas);
			_lazyIs = new Lazy<TIs>(MakeIs);
		}

		public THas Has
		{
			get
			{
				THas value = _lazyHas.Value;
				return value;
			}
		}
		public TIs Is
		{
			get
			{
				TIs value = _lazyIs.Value;
				return value;
			}
		}

		public TSpecification Make(Expression<Predicate<TResult>> expression,
			IAcceptExpectationVisitors lastTerm,
			Negated negated,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null)
		{
			var expectation = new Expectation<TSubject, TResult>(Inspection, expression, lastTerm, negated);
			return SpecFactory.Invoke(expectation, exceptionFilter);
		}

		public TSpecification Make(Predicate<TResult> predicate,
			IAcceptExpectationVisitors lastTerm,
			Negated negated,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null)
		{
			var expectation = new Expectation<TSubject, TResult>(Inspection, predicate, negated, lastTerm);
			return SpecFactory.Invoke(expectation, exceptionFilter);
		}

		public TSpecification Make(IExpectation<TSubject, TResult> expectation,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null)
		{
			return SpecFactory.Invoke(expectation, exceptionFilter);
		}

		private TBuilder Builder
		{
			get { return this as TBuilder; }
		}
		protected abstract
			Func<IExpectation<TSubject, TResult>, IExceptionFilter<TSubject, TResult>, TSpecification> SpecFactory { get; }

		protected override TSpecification Factory(Predicate<Exception> predicate,
			IInstrument<TSubject, TResult> inspection,
			Lazy<string> description)
		{
			var exceptionFilter = new ExceptionFilter<TSubject, TResult>(predicate, Inspection, description);
			var expectation = new Expectation<TSubject, TResult>(inspection, x => true, exceptionFilter, Negated.False);
			return Make(expectation, exceptionFilter);
		}

		protected IBoundSpecification<TSubject, TResult, TBuilder> MakeBoundSpecification(
			IExpectation<TSubject, TResult> expectation, IExceptionFilter<TSubject, TResult> exceptionFilter = null)
		{
			return new Specification<TSubject, TResult, TBuilder>(expectation,
				Builder,
				expectation.Xray,
				Prior,
				exceptionFilter);
		}

		protected abstract THas MakeHas();
		protected abstract TIs MakeIs();

		protected ISpecification<TSubject, TResult, TBuilder> MakeUnboundSpecification(
			IExpectation<TSubject, TResult> expectation, IExceptionFilter<TSubject, TResult> exceptionFilter = null)
		{
			return new Specification<TSubject, TResult, TBuilder>(expectation,
				Builder,
				expectation.Xray,
				Prior,
				exceptionFilter);
		}
	}

	public abstract class ExpectationBuilder<TSpecification, TSubject, TResult, TBuilder> :
		ExpectationBuilder
			<TSpecification, TSubject, TResult, IHas<TSpecification, TSubject, TResult>,
				INegatableIs<TSpecification, TSubject, TResult, IIs<TSpecification, TSubject, TResult>>, TBuilder>,
		IExpectationBuilder<TSpecification, TSubject, TResult>
		where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification<TBuilder>
		where TBuilder : class, IExpectationBuilder
	{
		protected ExpectationBuilder([NotNull] IInstrument<TSubject, TResult> instrument,
			[CanBeNull] TSpecification prior)
			: base(instrument, prior) {}

		protected override IHas<TSpecification, TSubject, TResult> MakeHas()
		{
			var has = new Has<TSpecification, TSubject, TResult>(this);
			return has;
		}

		protected override INegatableIs<TSpecification, TSubject, TResult, IIs<TSpecification, TSubject, TResult>>
			MakeIs()
		{
			return new Is<TSpecification, TSubject, TResult>(this, Negated.False);
		}
	}
}
