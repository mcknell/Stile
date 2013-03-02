#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfInstruments;
using Stile.Prototypes.Specifications.Builders.OfPredicates.Has;
using Stile.Prototypes.Specifications.Builders.OfPredicates.Is;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Tests.Prototypes.Specifications.SampleObjects;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Construction
{
	[TestFixture]
	public class ChainedSpecificationAcceptanceTests
	{
		[Test]
		public void BoundToExpression()
		{
			var foo = new Foo<int>();
			var specification = //
				Specify.For(() => foo).That(x => x.Count) //
					.Is.Not.EqualTo(12) //
					.AndThen.Is.Not.EqualTo(12);
			Assert.That(specification, Is.Not.Null);
			IBoundEvaluation<Foo<int>, int> evaluation = specification.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(evaluation.Value, Is.EqualTo(0));

			IBoundEvaluation<Foo<int>, int> secondEvaluation = evaluation.Evaluate();
			Assert.That(secondEvaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(secondEvaluation.Value, Is.Not.EqualTo(12));
		}

		[Test]
		public void BoundToInstance()
		{
			var specification = //
				Specify.For(new Foo<int>()).That(x => x.Count) //
					.Is.Not.EqualTo(12) //
					.AndThen.Is.Not.EqualTo(12);
			Assert.That(specification, Is.Not.Null);
			IBoundEvaluation<Foo<int>, int> evaluation = specification.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(evaluation.Value, Is.EqualTo(0));

			IBoundEvaluation<Foo<int>, int> secondEvaluation = evaluation.Evaluate();
			Assert.That(secondEvaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(secondEvaluation.Value, Is.Not.EqualTo(12));
		}

		[Test]
		public void Unbound()
		{
			var specification = //
				Specify.ForAny<Foo<int>>().That(x => x.ToString()).Has.HashCode(45) //
					.AndThen.Has.HashCode(45);
			Assert.That(specification, Is.Not.Null);
			IEvaluation<Foo<int>, string> evaluation = specification.Evaluate(new Foo<int>());
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Failed));
			Assert.That(evaluation.Value, Is.Not.EqualTo(45));

			IEvaluation<string> secondEvaluation = evaluation.Evaluate(new Foo<int>());
			Assert.That(secondEvaluation.Outcome, Is.EqualTo(Outcome.Failed));
			Assert.That(secondEvaluation.Value, Is.Not.EqualTo(45));
		}
	}
}
