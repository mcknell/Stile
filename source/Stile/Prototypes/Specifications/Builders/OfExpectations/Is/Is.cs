#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Is
{
	public interface IIs {}

	public interface IIs<out TSpecification, TSubject, TResult> : IIs,
		IHides<IIsState<TSpecification, TSubject, TResult>>
		where TSpecification : class, IChainableSpecification {}

	public interface INegatableIs : IIs {}

	public interface INegatableIs<out TSpecification, TSubject, TResult, out TNegated> : INegatableIs,
		IIs<TSpecification, TSubject, TResult>,
		INegatable<TNegated>
		where TSpecification : class, IChainableSpecification
		where TNegated : class, IIs<TSpecification, TSubject, TResult> {}

	public interface IIsState<out TSpecification, TSubject, TResult> : IAcceptExpectationVisitors
		where TSpecification : class, IChainableSpecification
	{
		IExpectationBuilderState<TSpecification, TSubject, TResult> BuilderState { get; }
		Negated Negated { get; }
	}

	public class Is<TSpecification, TSubject, TResult> :
		INegatableIs<TSpecification, TSubject, TResult, IIs<TSpecification, TSubject, TResult>>,
		IIsState<TSpecification, TSubject, TResult>
		where TSpecification : class, IChainableSpecification
	{
		public Is([NotNull] IExpectationBuilderState<TSpecification, TSubject, TResult> builderState,
			Negated negated)
		{
			BuilderState = builderState.ValidateArgumentIsNotNull();
			Negated = negated;
		}

		public IExpectationBuilderState<TSpecification, TSubject, TResult> BuilderState { get; private set; }
		public Negated Negated { get; private set; }

		public IIs<TSpecification, TSubject, TResult> Not
		{
			get { return new Is<TSpecification, TSubject, TResult>(BuilderState, Negated.True); }
		}

		public IAcceptExpectationVisitors Parent
		{
			get { return null; }
		}
		public IIsState<TSpecification, TSubject, TResult> Xray
		{
			get { return this; }
		}

		public void Accept(IExpectationVisitor visitor)
		{
			visitor.Visit3(this);
		}

		public TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data)
		{
			return visitor.Visit3(this, data);
		}
	}
}
