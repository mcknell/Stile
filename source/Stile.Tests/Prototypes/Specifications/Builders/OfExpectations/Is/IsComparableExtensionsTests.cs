#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Builders.OfExpectations.Is
{
	[TestFixture]
	public class IsComparableExtensionsTests
	{
		[Test]
		public void ComparablyEquivalentTo()
		{
			Assert.That(Specify.That(1).Is.ComparablyEquivalentTo(0).Evaluate().Outcome == Outcome.Failed);
			Assert.That(Specify.That(1).Is.ComparablyEquivalentTo(1).Evaluate().Outcome == Outcome.Succeeded);
			Assert.That(Specify.That(0).Is.ComparablyEquivalentTo(1).Evaluate().Outcome == Outcome.Failed);
		}

		[Test]
		public void GreaterThan()
		{
			Assert.That(Specify.That(1).Is.GreaterThan(0).Evaluate().Outcome == Outcome.Succeeded);
			Assert.That(Specify.That(1).Is.GreaterThan(1).Evaluate().Outcome == Outcome.Failed);
			Assert.That(Specify.That(0).Is.GreaterThan(1).Evaluate().Outcome == Outcome.Failed);
		}
	}
}
