#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Evaluations
{
	public interface IEvaluable {}

	public interface IEvaluable<TSubject> : IEvaluable
	{
		[NotNull]
		[System.Diagnostics.Contracts.Pure]
		IEvaluation<TSubject> Evaluate([NotNull] ISource<TSubject> source, IDeadline deadline = null);
	}

	public interface IEvaluable<TSubject, TResult> : IEvaluable
	{
		[NotNull]
		[System.Diagnostics.Contracts.Pure]
		IEvaluation<TSubject, TResult> Evaluate([NotNull] ISource<TSubject> source, IDeadline deadline = null);
	}
}
