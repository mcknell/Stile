#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Enumerable;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Tests.Prototypes.Specifications.SampleObjects;
using Iz = NUnit.Framework.Is;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Builders.OfExpectations
{
	[TestFixture]
	public class EnumerableExpectationBuilderExtensionsFixture
	{
		[Test]
		public void Contains()
		{
			var foo = new Foo<int> {7};
			IEvaluation<Foo<int>, Foo<int>> evaluation = Specify.That(() => foo).Contains(6).Evaluate();
			Assert.That(evaluation.Outcome == Outcome.Failed);
			Assert.That(evaluation.ToPastTense(), Iz.EqualTo(@"foo should contain 6
but was Foo<int> {7}"));

			foo.Add(6);

			IEvaluation<Foo<int>, Foo<int>> reEvaluate = evaluation.ReEvaluate();
			Assert.That(reEvaluate.Outcome == Outcome.Succeeded);
			Assert.That(reEvaluate.ToPastTense(), Iz.EqualTo(@"foo should contain 6"));
		}
	}
}
