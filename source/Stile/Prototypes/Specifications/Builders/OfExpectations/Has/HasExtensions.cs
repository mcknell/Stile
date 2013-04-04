#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Diagnostics.Contracts;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
using Stile.Prototypes.Specifications.SemanticModel.Expectations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Builders.OfExpectations.Has
{
	public static class HasExtensions
	{
		[Pure]
		[RuleExpansion(Nonterminal.Has)]
		public static TSpecification HashCode<TSpecification, TSubject, TResult>(
			this IHas<TSpecification, TSubject, TResult> has, int hashCode)
			where TSpecification : class, ISpecification<TSubject, TResult>, IChainableSpecification
		{
			var hashcode = new Hashcode<TSpecification, TSubject, TResult>(has.Xray, hashCode);
			var expectation = new Expectation<TSubject, TResult>(has.Xray.ExpectationBuilder.Instrument,
				x => x.GetHashCode() == hashCode,
				hashcode);
			TSpecification specification = has.Xray.ExpectationBuilder.Make(expectation);
			return specification;
		}
	}
}
