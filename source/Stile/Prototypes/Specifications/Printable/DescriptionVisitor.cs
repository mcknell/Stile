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
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Printable
{
	public interface IDescriptionVisitor
	{
		void DescribeOverload1<TSubject>([NotNull] IProcedure<TSubject> procedure);
		void DescribeOverload1<TSubject>([NotNull] IProcedureBuilder<TSubject> builder);
		void DescribeOverload1<TSubject>([NotNull] ISource<TSubject> source);
		void DescribeOverload2<TSubject, TResult>([NotNull] IExpectation<TSubject, TResult> expectation);
		void DescribeOverload2<TSubject, TResult>([NotNull] IEvaluation<TSubject, TResult> evaluation);
		void DescribeOverload2<TSubject, TResult>([NotNull] IInstrument<TSubject, TResult> instrument);

		void DescribeOverload3<TSpecification, TSubject, TResult>(
			[NotNull] IHas<TSpecification, TSubject, TResult> has)
			where TSpecification : class, IChainableSpecification;

		void DescribeOverload3<TSpecification, TSubject, TResult>(
			[NotNull] IIs<TSpecification, TSubject, TResult> @is) where TSpecification : class, IChainableSpecification;

		void DescribeOverload3<TSpecification, TSubject, TResult>(
			[NotNull] IExpectationBuilder<TSpecification, TSubject, TResult> builder)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification;

		void DescribeOverload3<TSubject, TResult, TExpectationBuilder>(
			[NotNull] ISpecification<TSubject, TResult, TExpectationBuilder> specification)
			where TExpectationBuilder : class, IExpectationBuilder;

		void DescribeOverload4<TSpecification, TSubject, TResult, TItem>(
			IEnumerableExpectationBuilder<TSpecification, TSubject, TResult, TItem> builder)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
			where TResult : class, IEnumerable<TItem>;
	}

	public interface IExpectationFormatVisitor
	{
		string Format<TSubject, TResult>(IExpectation<TSubject, TResult> expectation);
	}
}
