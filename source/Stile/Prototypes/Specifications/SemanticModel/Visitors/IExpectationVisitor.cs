﻿#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantifiers;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Visitors
{
	public interface IExpectationVisitor
	{
		void Visit2<TSubject, TResult>([NotNull] IExpectation<TSubject, TResult> target);

		void Visit3<TSpecification, TSubject, TResult>([NotNull] IEqualToState<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification;

		void Visit3<TSpecification, TSubject, TResult>([NotNull] IHas<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification;

		void Visit3<TSpecification, TSubject, TResult>([NotNull] IIs<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification;

		void Visit4<TSpecification, TSubject, TResult, TItem>(
			IHasAll<TSpecification, TSubject, TResult, TItem> target)
			where TSpecification : class, ISpecification, IChainableSpecification;
	}

	public interface IExpectationVisitor<TData> : IExpectationVisitor
	{
		TData Visit2<TSubject, TResult>([NotNull] IExpectation<TSubject, TResult> target, TData data);

		TData Visit3<TSpecification, TSubject, TResult>(
			[NotNull] IEqualToState<TSpecification, TSubject, TResult> target, TData data)
			where TSpecification : class, IChainableSpecification;

		TData Visit3<TSpecification, TSubject, TResult>([NotNull] IHas<TSpecification, TSubject, TResult> target,
			TData data) where TSpecification : class, IChainableSpecification;

		TData Visit3<TSpecification, TSubject, TResult>([NotNull] IIs<TSpecification, TSubject, TResult> target,
			TData data) where TSpecification : class, IChainableSpecification;

		TData Visit4<TSpecification, TSubject, TResult, TItem>(
			IHasAll<TSpecification, TSubject, TResult, TItem> target, TData data)
			where TSpecification : class, ISpecification, IChainableSpecification;
	}
}