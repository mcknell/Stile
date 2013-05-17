#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Builders.OfProcedures;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Tests.Prototypes.Specifications.SampleObjects;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Construction
{
	[TestFixture]
	public class ChangeSpecificationAcceptanceTests
	{
		[Test]
		public void WillChangeTo()
		{
			IEvaluation<Foo<int>, int> evaluation =
				Specify.ForAny<Foo<int>>().That(x => x.Bumps).WillChangeTo(1).Evaluate(() => new Foo<int>());
			Assert.NotNull(evaluation);
			Assert.That(evaluation.Outcome == Outcome.Succeeded);
			Assert.That(evaluation.ToPastTense(), Is.EqualTo("new Foo<int>().Bumps should not be 1"));
			IEvaluation<Foo<int>, int> next = evaluation.EvaluateNext();
			Assert.NotNull(next);
			Assert.That(next.Outcome == Outcome.Failed);
			Assert.That(next.ToPastTense(), Is.EqualTo(@"new Foo<int>().Bumps should be 1
but was 0"));
		}
	}
}
