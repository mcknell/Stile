#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Globalization;
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfInstruments;
using Stile.Prototypes.Specifications.Builders.OfPredicates.Is;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Tests.Prototypes.Specifications
{
	[TestFixture]
	public class SubjunctiveSpecificationVisitorFixture
	{
		[Test]
		public void ScalarIsEqualTo()
		{
			ISpecification<int, string> specification =
				Specify.ForAny<int>().That(x => x.ToString(CultureInfo.InvariantCulture)).Is.EqualTo("6");
			IEvaluation<string> evaluation = specification.Evaluate(6);
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(specification.Evaluate(7).Outcome, Is.EqualTo(Outcome.Failed));
		}
	}
}
