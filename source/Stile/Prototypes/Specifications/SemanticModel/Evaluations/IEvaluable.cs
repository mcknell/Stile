#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
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

		/*[NotNull]
		[System.Diagnostics.Contracts.Pure]
		IEvaluation Evaluate(Expression<Func<TSubject>> source, IDeadline deadline = null);*/
	}

	public interface IEvaluable<TSubject, TResult> : IEvaluable
	{
		[NotNull]
		[System.Diagnostics.Contracts.Pure]
		IEvaluation<TSubject, TResult> Evaluate([NotNull] ISource<TSubject> source, IDeadline deadline = null);

/*
		[NotNull]
		[System.Diagnostics.Contracts.Pure]
		IEvaluation<TSubject, TResult> Evaluate(Expression<Func<TSubject>> source, IDeadline deadline = null);
*/
	}

	public static class EvaluableExtensions
	{
		public static IEvaluation Evaluate<TSubject>([NotNull] this IEvaluable<TSubject> evaluable,
			[CanBeNull] TSubject subject,
			IDeadline deadline = null)
		{
			return evaluable.Evaluate(() => subject, deadline);
		}

		public static IEvaluation Evaluate<TSubject>([NotNull] this IEvaluable<TSubject> evaluable,
			[NotNull] Expression<Func<TSubject>> source,
			IDeadline deadline = null)
		{
			return evaluable.Evaluate(new Source<TSubject>(source), deadline);
		}

		public static IEvaluation<TSubject, TResult> Evaluate<TSubject, TResult>(
			[NotNull] this IEvaluable<TSubject, TResult> evaluable,
			[CanBeNull] TSubject source,
			IDeadline deadline = null)
		{
			return evaluable.Evaluate(() => source, deadline);
		}

		public static IEvaluation<TSubject, TResult> Evaluate<TSubject, TResult>(
			[NotNull] this IEvaluable<TSubject, TResult> evaluable,
			[NotNull] Expression<Func<TSubject>> source,
			IDeadline deadline = null)
		{
			return evaluable.Evaluate(new Source<TSubject>(source), deadline);
		}
	}
}
