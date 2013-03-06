#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Past
{
	public static class PastEvaluations
	{
		public static string ToPastTense<TSubject, TResult>([NotNull] this IEvaluation<TSubject, TResult> evaluation)
		{
			return PastTenseEvaluationDescriber.Describe(evaluation);
		}
	}
}
