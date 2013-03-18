#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Printable.Past;
using Stile.Prototypes.Specifications.Printable.Specifications.Should;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Printable
{
	public static class PrintExtensions
	{
		public static string ToPastTense<TSubject>([NotNull] this IFaultEvaluation<TSubject> evaluation)
		{
			return PastEvaluationDescriber.Describe(evaluation);
		}

		public static string ToPastTense<TSubject, TResult>([NotNull] this IEvaluation<TSubject, TResult> evaluation)
		{
			return PastEvaluationDescriber.Describe(evaluation);
		}

		public static string ToShould<TSubject>([NotNull] this IFaultSpecification<TSubject> specification)
		{
			var describer = new ShouldSpecificationDescriber(specification.Xray.Procedure.Xray.Source);
			describer.Visit1(specification);
			return describer.ToString();
		}

		public static string ToShould<TSubject, TResult>(
			[NotNull] this ISpecification<TSubject, TResult> specification)
		{
			return ShouldSpecificationDescriber.Describe(specification);
		}
	}
}
