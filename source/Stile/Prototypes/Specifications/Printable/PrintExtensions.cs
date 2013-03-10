﻿#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Printable.Past;
using Stile.Prototypes.Specifications.Printable.Specifications.Should;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Printable
{
	public static class PrintExtensions
	{
		public static string ToPastTense<TSubject, TResult>([NotNull] this IEvaluation<TSubject, TResult> evaluation)
		{
			return PastEvaluationDescriber.Describe(evaluation);
		}

		public static string ToShould<TSubject, TResult, TExpectationBuilder>(
			[NotNull] this ISpecification<TSubject, TResult, TExpectationBuilder> specification)
			where TExpectationBuilder : class, IExpectationBuilder
		{
			return ShouldSpecificationDescriber.Describe(specification);
		}
	}
}