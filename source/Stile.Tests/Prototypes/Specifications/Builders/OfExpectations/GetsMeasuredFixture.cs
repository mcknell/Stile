#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Tests.Prototypes.Specifications.SampleObjects;
using Iz = NUnit.Framework.Is;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Builders.OfExpectations
{
	[TestFixture]
	public class GetsMeasuredFixture
	{
		[Test]
		public void Bound()
		{
			var subject = new Foo<int>();

			IFluentBoundExpectationBuilder<Foo<int>, Foo<int>> builder = Specify.That(() => subject);
			IEvaluation<Foo<int>, Foo<int>> evaluation = builder.GetsMeasured().Evaluate();
			Assert.That(evaluation.Outcome, Iz.EqualTo(Outcome.Succeeded));
			Assert.That(evaluation.Value, Iz.EqualTo(subject));
			Assert.That(evaluation.ToPastTense(), Iz.EqualTo(@"subject should get measured"));
		}

		[Test]
		public void Unbound()
		{
			var subject = new Foo<int>();

			IFluentExpectationBuilder<Foo<int>, Foo<int>> builder = Specify.ThatAny<Foo<int>>();
			IEvaluation<Foo<int>, Foo<int>> evaluation = builder.GetsMeasured().Evaluate(() => subject);
			Assert.That(evaluation.Outcome, Iz.EqualTo(Outcome.Succeeded));
			Assert.That(evaluation.Value, Iz.EqualTo(subject));
			Assert.That(evaluation.ToPastTense(), Iz.EqualTo(@"subject should get measured"));
		}
	}
}
