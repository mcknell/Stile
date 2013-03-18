#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Visitors
{
	public interface IEvaluationVisitor
	{
		void Visit1<TSubject>([NotNull] IFaultEvaluation<TSubject> target);
		void Visit1<TSubject>([NotNull] IFaultSpecification<TSubject> target);
		void Visit2<TSubject, TResult>([NotNull] IEvaluation<TSubject, TResult> target);

		void Visit3<TSubject, TResult, TExpectationBuilder>(
			[NotNull] ISpecification<TSubject, TResult, TExpectationBuilder> target)
			where TExpectationBuilder : class, IExpectationBuilder;
	}

	public interface IEvaluationVisitor<TData>
	{
		TData Visit1<TSubject>([NotNull] IFaultEvaluation<TSubject> target, TData data);
		TData Visit1<TSubject>([NotNull] IFaultSpecification<TSubject> target, TData data);

		TData Visit2<TSubject, TResult>(IEvaluation<TSubject, TResult> target, TData data);

		TData Visit3<TSubject, TResult, TExpectationBuilder>(
			ISpecification<TSubject, TResult, TExpectationBuilder> target, TData data)
			where TExpectationBuilder : class, IExpectationBuilder;
	}
}
