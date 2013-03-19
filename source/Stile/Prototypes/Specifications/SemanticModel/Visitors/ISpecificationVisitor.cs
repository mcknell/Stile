#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Builders.OfProcedures;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Visitors
{
	public interface ISpecificationVisitor
	{
		void Visit1<TSubject>([NotNull] IExceptionFilter<TSubject> target);
		void Visit1<TSubject>([NotNull] IFaultSpecification<TSubject> target);
		void Visit1<TSubject>([NotNull] IProcedure<TSubject> target);
		void Visit1<TSubject>([NotNull] IProcedureBuilder<TSubject> target);
		void Visit1<TSubject>([NotNull] ISource<TSubject> target);

		void Visit2<TSubject, TResult>([NotNull] IExceptionFilter<TSubject, TResult> target);
		void Visit2<TSubject, TResult>([NotNull] IExpectation<TSubject, TResult> target);
		void Visit2<TSubject, TResult>([NotNull] IInstrument<TSubject, TResult> target);
		void Visit2<TSubject, TResult>([NotNull] ISpecification<TSubject, TResult> target);

		void Visit3<TSpecification, TSubject, TResult>([NotNull] IHas<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification;

		void Visit3<TSpecification, TSubject, TResult>([NotNull] IIs<TSpecification, TSubject, TResult> target)
			where TSpecification : class, IChainableSpecification;

		void Visit3<TSpecification, TSubject, TResult>(
			[NotNull] IExpectationBuilder<TSpecification, TSubject, TResult> target)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification;

		void Visit3<TSubject, TResult, TExpectationBuilder>(
			[NotNull] ISpecification<TSubject, TResult, TExpectationBuilder> target)
			where TExpectationBuilder : class, IExpectationBuilder;

		void Visit4<TSpecification, TSubject, TResult, TItem>(
			IEnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem> target)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>;
	}

	public interface ISpecificationVisitor<TData> : ISpecificationVisitor
	{
		TData Visit1<TSubject>([NotNull] IExceptionFilter<TSubject> target, TData data);
		TData Visit1<TSubject>([NotNull] IFaultSpecification<TSubject> target, TData data);
		TData Visit1<TSubject>([NotNull] IProcedure<TSubject> target, TData data);
		TData Visit1<TSubject>([NotNull] IProcedureBuilder<TSubject> target, TData data);
		TData Visit1<TSubject>([NotNull] ISource<TSubject> target, TData data);

		TData Visit2<TSubject, TResult>([NotNull] IExceptionFilter<TSubject, TResult> target, TData data);
		TData Visit2<TSubject, TResult>([NotNull] IExpectation<TSubject, TResult> target, TData data);
		TData Visit2<TSubject, TResult>([NotNull] IInstrument<TSubject, TResult> target, TData data);
		TData Visit2<TSubject, TResult>([NotNull] ISpecification<TSubject, TResult> target, TData data);

		TData Visit3<TSpecification, TSubject, TResult>([NotNull] IHas<TSpecification, TSubject, TResult> target,
			TData data) where TSpecification : class, IChainableSpecification;

		TData Visit3<TSpecification, TSubject, TResult>([NotNull] IIs<TSpecification, TSubject, TResult> target,
			TData data) where TSpecification : class, IChainableSpecification;

		TData Visit3<TSpecification, TSubject, TResult>(
			[NotNull] IExpectationBuilder<TSpecification, TSubject, TResult> target, TData data)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification;

		TData Visit3<TSubject, TResult, TExpectationBuilder>(
			[NotNull] ISpecification<TSubject, TResult, TExpectationBuilder> target, TData data)
			where TExpectationBuilder : class, IExpectationBuilder;

		TData Visit4<TSpecification, TSubject, TResult, TItem>(
			IEnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem> target, TData data)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>;
	}
}
