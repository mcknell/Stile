#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Builders.OfProcedures;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Tests.Prototypes.Specifications.SampleObjects;
using NUnitFramework = NUnit.Framework;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Builders.OfExpectations.Is
{
	[NUnitFramework.TestFixtureAttribute]
	public class NullFixture
	{
		[NUnitFramework.TestAttribute]
		public void Null()
		{
			var subject = new Foo<int>();

			IEvaluation<Foo<int>, Foo<int>> evaluation =
				Specify.For(() => subject).That(x => x).Is.Not.Null().Evaluate();
			NUnitFramework.Assert.That(evaluation.Outcome, NUnitFramework.Is.EqualTo(Outcome.Succeeded));

			subject = null;
			IEvaluation<Foo<int>, Foo<int>> secondEvaluation = evaluation.ReEvaluate();

			NUnitFramework.Assert.That(secondEvaluation.Outcome, NUnitFramework.Is.EqualTo(Outcome.Failed));
			NUnitFramework.Assert.AreEqual(@"subject should not be null
but was <null>", secondEvaluation.ToPastTense());
		}
	}
}
