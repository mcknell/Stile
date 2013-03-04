#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Text;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Resources;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Past
{
	public static class PastEvaluations
	{
		public static string ToPastTense<TSubject, TResult>([NotNull] this IEvaluation<TSubject, TResult> evaluation)
		{
			IEvaluationState<TSubject, TResult> evaluationState = evaluation.ValidateArgumentIsNotNull().Xray;

			var sb = new StringBuilder();
			sb.Append(PastTenseEvaluations.Expected + " ");
			if (evaluationState.Source != null)
			{
				sb.Append(PastTenseEvaluations.That + " " + evaluationState.Instrument.Xray.Description);
			}
			return sb.ToString();
		}
	}
}
