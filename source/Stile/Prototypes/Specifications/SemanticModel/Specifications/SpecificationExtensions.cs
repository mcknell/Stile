#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Specifications
{
	public static class SpecificationExtensions
	{
		[System.Diagnostics.Contracts.Pure]
		public static TSpecification Before<TSpecification>([NotNull] this TSpecification specification,
			TimeSpan timeout) where TSpecification : class, ISpecification
		{
			return specification.Before(new Deadline(timeout, false));
		}

		[System.Diagnostics.Contracts.Pure]
		public static TSpecification Before<TSpecification>([NotNull] this TSpecification specification,
			IDeadline deadline) where TSpecification : class, ISpecification
		{
			var specificationState = specification as ISpecificationState;
			object clone = specificationState.Clone(deadline);
			return (TSpecification) clone; // hackish dodge around strong chain-of-custody of type
		}

		[System.Diagnostics.Contracts.Pure]
		public static bool IsTrue<TSubject, TResult>(this IBoundSpecification<TSubject, TResult> boundSpecification)
		{
			return boundSpecification.Evaluate().Outcome == Outcome.Succeeded;
		}

		[System.Diagnostics.Contracts.Pure]
		public static bool IsTrueFor<TSubject, TResult>(this ISpecification<TSubject, TResult> boundSpecification,
			ISource<TSubject> source)
		{
			return boundSpecification.Evaluate(source).Outcome == Outcome.Succeeded;
		}
	}
}
