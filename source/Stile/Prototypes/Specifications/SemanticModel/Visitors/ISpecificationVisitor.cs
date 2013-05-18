#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
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
		void Visit1<TSubject>([NotNull] ISource<TSubject> target);
		void Visit2<TSubject, TResult>([NotNull] IExceptionFilter<TSubject, TResult> target);
		void Visit2<TSubject, TResult>([NotNull] IExpectation<TSubject, TResult> target);
		void Visit2<TSubject, TResult>([NotNull] IInstrument<TSubject, TResult> target);

		void Visit3<TSubject, TResult, TExpectationBuilder>(
			[NotNull] ISpecification<TSubject, TResult, TExpectationBuilder> target)
			where TExpectationBuilder : class, IExpectationBuilder;
	}

	public interface ISpecificationVisitor<TData> : ISpecificationVisitor
	{
		TData Visit1<TSubject>([NotNull] IExceptionFilter<TSubject> target, TData data);
		TData Visit1<TSubject>([NotNull] IFaultSpecification<TSubject> target, TData data);
		TData Visit1<TSubject>([NotNull] IProcedure<TSubject> target, TData data);
		TData Visit1<TSubject>([NotNull] ISource<TSubject> target, TData data);
		TData Visit2<TSubject, TResult>([NotNull] IExceptionFilter<TSubject, TResult> target, TData data);
		TData Visit2<TSubject, TResult>([NotNull] IExpectation<TSubject, TResult> target, TData data);
		TData Visit2<TSubject, TResult>([NotNull] IInstrument<TSubject, TResult> target, TData data);

		TData Visit3<TSubject, TResult, TExpectationBuilder>(
			[NotNull] ISpecification<TSubject, TResult, TExpectationBuilder> target, TData data)
			where TExpectationBuilder : class, IExpectationBuilder;
	}
}
