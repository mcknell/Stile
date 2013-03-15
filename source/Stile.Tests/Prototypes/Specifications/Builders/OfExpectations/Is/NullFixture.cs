#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Builders.OfInstruments;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Tests.Prototypes.Specifications.SampleObjects;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Builders.OfExpectations.Is
{
	[TestFixture]
	public class NullFixture
	{
		[Test]
		public void Null()
		{
			var subject = new Foo<int>();

			IEvaluation<Foo<int>, Foo<int>> evaluation =
				Specify.For(() => subject).That(x => x).Is.Not.Null().Evaluate();
			Assert.That(evaluation.Outcome, NUnit.Framework.Is.EqualTo(Outcome.Succeeded));

			subject = null;
			IEvaluation<Foo<int>, Foo<int>> secondEvaluation = evaluation.ReEvaluate();

			Assert.That(secondEvaluation.Outcome, NUnit.Framework.Is.EqualTo(Outcome.Failed));
			Assert.AreEqual(@"Expected that subject would be not null
but was <null>", secondEvaluation.ToPastTense());
		}
	}
}
