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

		public static IEvaluation<TSubject, TResult> EvaluateNextWith<TSubject, TResult>(
			[NotNull] this IEvaluation<TSubject, TResult> evaluation,
			[NotNull] Expression<Func<TSubject>> source,
			IDeadline deadline = null)
		{
			return evaluation.EvaluateNextWith(new Source<TSubject>(source), deadline);
		}
	}
}
