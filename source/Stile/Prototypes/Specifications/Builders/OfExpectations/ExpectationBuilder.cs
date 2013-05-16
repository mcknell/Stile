#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations
{
	public interface IExpectationBuilder : IChainingConjuction {}

	public interface IExpectationBuilder<out TSpecification> : IExpectationBuilder
		where TSpecification : class, IChainableSpecification
	{
		[NotNull]
		[System.Diagnostics.Contracts.Pure]
		[RuleExpansion(Nonterminal.Enum.Instrument, Nonterminal.Enum.ExceptionFilter)]
		TSpecification Throws<TException>() where TException : Exception;
	}

	public interface IExpectationBuilder<out TSpecification, TSubject, TResult, out THas, out TIs> :
		IExpectationBuilder<TSpecification>
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
		IHides<IExpectationBuilderState<TSpecification, TSubject, TResult>>
		where TSpecification : class, IChainableSpecification {}

	public interface IExpectationBuilderState
	{
		[NotNull]
		object CloneFor(object specification);
	}

	public interface IExpectationBuilderState<out TSpecification, TSubject, TResult> : IExpectationBuilderState,
		IChainingConjuctionState<TSpecification, IInstrument<TSubject, TResult>>
		where TSpecification : class, IChainableSpecification
	{
		[NotNull]
		TSpecification Make([NotNull] IExpectation<TSubject, TResult> expectation,
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

		public override IExpectationBuilderState<TSpecification, TSubject, TResult> Xray
		{
			get { return this; }
		}

		public abstract object CloneFor(object specification);

		public TSpecification Make(IExpectation<TSubject, TResult> expectation,
			IExceptionFilter<TSubject, TResult> exceptionFilter = null)
		{
			return SpecFactory.Invoke(expectation, exceptionFilter);
		}

		protected abstract TBuilder Builder { get; }
		protected abstract
			Func<IExpectation<TSubject, TResult>, IExceptionFilter<TSubject, TResult>, TSpecification> SpecFactory { get; }

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

		protected override TSpecification SpecFactor(Predicate<Exception> predicate,
			IInstrument<TSubject, TResult> inspection,
			Lazy<string> description)
		{
			var exceptionFilter = new ExceptionFilter<TSubject, TResult>(predicate, Inspection, description);
			var expectation = new Expectation<TSubject, TResult>(inspection, x => true, exceptionFilter, Negated.False);
			return Make(expectation, exceptionFilter);
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
