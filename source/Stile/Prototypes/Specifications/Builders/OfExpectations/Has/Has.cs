#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Structural.FluentInterface;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Prototypes.Specifications.SemanticModel.Visitors;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has
{
	public interface IHas {}

	public interface IHas<out TSpecification, TSubject, TResult> : IHas,
		IHides<IHasState<TSpecification, TSubject, TResult>>
		where TSpecification : class, IChainableSpecification {}

	public interface IHasState<out TSpecification, TSubject, TResult> : IAcceptExpectationVisitors
		where TSpecification : class, IChainableSpecification
	{
		IExpectationBuilderState<TSpecification, TSubject, TResult> ExpectationBuilder { get; }
	}

	public class Has<TSpecification, TSubject, TResult> : IHas<TSpecification, TSubject, TResult>,
		IHasState<TSpecification, TSubject, TResult>
		where TSpecification : class, IChainableSpecification
	{
		public Has([NotNull] IExpectationBuilderState<TSpecification, TSubject, TResult> builderState)
		{
			ExpectationBuilder = builderState;
		}

		public IExpectationBuilderState<TSpecification, TSubject, TResult> ExpectationBuilder { get; private set; }

		public IHasState<TSpecification, TSubject, TResult> Xray
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
