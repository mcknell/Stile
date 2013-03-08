#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Is
{
	public interface IIs {}

	public interface IResultIs<out TSpecification, out TResult> : IIs
		where TSpecification : class, IChainableSpecification {}

	public interface IIs<out TSpecification, TSubject, TResult> : IResultIs<TSpecification, TResult>,
		IHides<IIsState<TSpecification, TSubject, TResult>>
		where TSpecification : class, IChainableSpecification {}

	public interface INegatableIs : IIs {}

	public interface INegatableIs<out TSpecification, TSubject, TResult, out TNegated> : INegatableIs,
		IIs<TSpecification, TSubject, TResult>,
		INegatable<TNegated>
		where TSpecification : class, IChainableSpecification
		where TNegated : class, IIs<TSpecification, TSubject, TResult> {}

	public interface IIsState<out TSpecification, TSubject, TResult> :
		IExpectationBuilderState<TSpecification, TSubject, TResult>
		where TSpecification : class, IChainableSpecification
	{
		Negated Negated { get; }
	}

	public class Is<TSpecification, TSubject, TResult> :
		INegatableIs<TSpecification, TSubject, TResult, IIs<TSpecification, TSubject, TResult>>,
		IIsState<TSpecification, TSubject, TResult>
		where TSpecification : class, IChainableSpecification
	{
		protected readonly IExpectationBuilderState<TSpecification, TSubject, TResult> _builderState;

		public Is([NotNull] IExpectationBuilderState<TSpecification, TSubject, TResult> builderState,
			Negated negated)
		{
			_builderState = builderState.ValidateArgumentIsNotNull();
			Instrument = builderState.Instrument.ValidateArgumentIsNotNull();
			Negated = negated;
			SpecificationFactory = builderState.Make;
		}

		public IInstrument<TSubject, TResult> Instrument { get; private set; }
		public Negated Negated { get; private set; }

		public IIs<TSpecification, TSubject, TResult> Not
		{
			get { return new Is<TSpecification, TSubject, TResult>(_builderState, Negated.True); }
		}

		public ExpectationBuilder.SpecificationFactory<TSubject, TResult, TSpecification> SpecificationFactory { get; private set; }

		public IIsState<TSpecification, TSubject, TResult> Xray
		{
			get { return this; }
		}

		public void Accept(ISpecificationVisitor visitor)
		{
			visitor.Visit3(this);
		}

		public TData Accept<TData>(ISpecificationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit3(this, data);
		}

		public TSpecification Make(IExpectation<TSubject, TResult> expectation,
			IExceptionFilter<TSubject, TResult> filter = null)
		{
			return SpecificationFactory.Invoke(expectation, filter);
		}
	}
}
