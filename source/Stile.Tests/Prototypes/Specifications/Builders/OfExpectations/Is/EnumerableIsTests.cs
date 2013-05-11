#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Enumerable;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Tests.Prototypes.Specifications.SampleObjects;
using Iz = NUnit.Framework.Is;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Builders.OfExpectations.Is
{
	[TestFixture]
	public class EnumerableIsTests
	{
		[Test]
		public void Empty()
		{
			var subject = new Foo<int>();

			IEvaluation<Foo<int>, Foo<int>> evaluationOfNew = Specify.ThatAny<Foo<int>>().OfItemsLike(0).Is.Empty //
				.AndThen.Is.Not.Empty.Evaluate(() => subject);

			Assert.That(evaluationOfNew.Outcome, Iz.EqualTo(Outcome.Succeeded));

			subject.Add(1);
			IEvaluation<Foo<int>, Foo<int>> evaluationOfOne =
				Specify.ThatAny<Foo<int>>().OfItemsLike(0).Is.Not.Empty.Evaluate(() => subject);

			Assert.That(evaluationOfOne.Outcome, Iz.EqualTo(Outcome.Succeeded));
		}

		[Test]
		public void SequenceEquals()
		{
			var ints = new[] {1, 2, 3};
			IBoundSpecification<int[], int[], IFluentEnumerableBoundExpectationBuilder<int[], int[], int>>
				specification = Specify.That(() => ints).OfItemsLike(0).Is.Not.SequenceEqualTo(ints);
			IEvaluation<int[], int[]> evaluation = specification.Evaluate();

			Assert.That(evaluation.Outcome == Outcome.Failed);
			Assert.That(evaluation.ToPastTense(), Contains.Substring("ints should not be sequence equal to int[3] {1, 2, 3}"));
		}
	}
}
