#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Visitors
{
	public interface IAcceptExpectationVisitors
	{
		void Accept(IExpectationVisitor visitor);
		TData Accept<TData>(IExpectationVisitor<TData> visitor, TData data);
	}

	public interface IExpectationVisitor
	{
		void Visit2<TSubject, TResult>([NotNull] IExpectation<TSubject, TResult> expectation);

		void Visit3<TSpecification, TSubject, TResult>([NotNull] IHas<TSpecification, TSubject, TResult> expectation)
			where TSpecification : class, IChainableSpecification;
	}

	public interface IExpectationVisitor<TData> : IExpectationVisitor
	{
		TData Visit2<TSubject, TResult>([NotNull] IExpectation<TSubject, TResult> expectation, TData data);

		TData Visit3<TSpecification, TSubject, TResult>([NotNull] IHas<TSpecification, TSubject, TResult> target,
			TData data) where TSpecification : class, IChainableSpecification;
	}
}
