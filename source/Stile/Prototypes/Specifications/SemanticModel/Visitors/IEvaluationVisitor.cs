#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Visitor;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Visitors
{
	public interface IAcceptEvaluationVisitors : IAcceptVisitors<IEvaluationVisitor> {}

	public interface IEvaluationVisitor : ISpecificationVisitor
	{
		void Visit2<TSubject, TResult>([NotNull] IEvaluation<TSubject, TResult> evaluation);

		TData Visit2<TSubject, TResult, TData>([NotNull] IEvaluation<TSubject, TResult> evaluation,
			TData data = default(TData));
	}
}
