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
using Stile.Prototypes.Specifications.Builders.OfInstruments;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public interface ISpecificationVisitor
	{
		void Visit1<TSubject>([NotNull] IProcedure<TSubject> procedure);
		void Visit1<TSubject>([NotNull] IProcedureBuilder<TSubject> builder);
		void Visit1<TSubject>([NotNull] ISource<TSubject> source);

		void Visit2<TSubject, TResult>([NotNull] IExpectation<TSubject, TResult> expectation);

		void Visit2<TSubject, TResult>([NotNull] IInstrument<TSubject, TResult> instrument);

		void Visit3<TSpecification, TSubject, TResult>([NotNull] IHas<TSpecification, TSubject, TResult> has)
			where TSpecification : class, IChainableSpecification;

		void Visit3<TSpecification, TSubject, TResult>([NotNull] IIs<TSpecification, TSubject, TResult> @is)
			where TSpecification : class, IChainableSpecification;

		void Visit3<TSpecification, TSubject, TResult>(
			[NotNull] IExpectationBuilder<TSpecification, TSubject, TResult> builder)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification;

		void Visit3<TSubject, TResult, TExpectationBuilder>(
			[NotNull] ISpecification<TSubject, TResult, TExpectationBuilder> specification)
			where TExpectationBuilder : class, IExpectationBuilder;

		void Visit4<TSpecification, TSubject, TResult, TItem>(
			IEnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem> builder)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>;
	}

	public interface ISpecificationVisitor<TData> : ISpecificationVisitor
	{
		TData Visit1<TSubject>([NotNull] IProcedure<TSubject> procedure, TData data);
		TData Visit1<TSubject>([NotNull] IProcedureBuilder<TSubject> builder, TData data);
		TData Visit1<TSubject>([NotNull] ISource<TSubject> source, TData data);

		TData Visit2<TSubject, TResult>([NotNull] IExpectation<TSubject, TResult> expectation, TData data);

		TData Visit2<TSubject, TResult>([NotNull] IInstrument<TSubject, TResult> instrument, TData data);

		TData Visit3<TSpecification, TSubject, TResult>([NotNull] IHas<TSpecification, TSubject, TResult> has,
			TData data) where TSpecification : class, IChainableSpecification;

		TData Visit3<TSpecification, TSubject, TResult>([NotNull] IIs<TSpecification, TSubject, TResult> @is,
			TData data) where TSpecification : class, IChainableSpecification;

		TData Visit3<TSpecification, TSubject, TResult>(
			[NotNull] IExpectationBuilder<TSpecification, TSubject, TResult> builder, TData data)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification;

		TData Visit3<TSubject, TResult, TExpectationBuilder>(
			[NotNull] ISpecification<TSubject, TResult, TExpectationBuilder> specification, TData data)
			where TExpectationBuilder : class, IExpectationBuilder;

		TData Visit4<TSpecification, TSubject, TResult, TItem>(
			IEnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem> builder, TData data)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>;
	}
}
