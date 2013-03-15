#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Builders.OfInstruments;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Tests.Prototypes.Specifications.SampleObjects;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Construction
{
	[TestFixture]
	public class ChainedSpecificationAcceptanceTests
	{
		[Test]
		[Explicit("WIP")]
		public void BoundToExpression()
		{
			var foo = new Saboteur(2);
			foo.Load(() => new ArgumentException());

			IBoundEvaluation<Saboteur, int?> evaluation =
				Specify.For(() => foo).That(x => x.SuicidalSideEffect.MisfiresRemaining) //
					.Is.EqualTo(2) //
					.AndThen.Is.EqualTo(1) //
					.AndThen.Throws<ArgumentException>().Build() //
					.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(evaluation.Value, Is.EqualTo(2));

			IBoundEvaluation<Saboteur, int?> secondEvaluation = evaluation.Evaluate();
			Assert.That(secondEvaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(secondEvaluation.Value, Is.EqualTo(0));

			/* this should only pass if indeed the third specification is being evaluated;
		    * the first two would fail to throw */
			IBoundEvaluation<Saboteur, int?> thirdEvaluation = secondEvaluation.Evaluate();
			Assert.That(thirdEvaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
		}

		[Test]
		public void BoundToInstance()
		{
			IBoundSpecification<Foo<int>, int, IFluentBoundExpectationBuilder<Foo<int>, int>> specification = //
				Specify.For(() => new Foo<int>()).That(x => x.Count) //
					.Is.Not.EqualTo(12) //
					.AndThen.Is.Not.EqualTo(12);
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
			ISpecification<Foo<int>, string, IFluentExpectationBuilder<Foo<int>, string>> specification = //
				Specify.ForAny<Foo<int>>().That(x => x.ToString()).Has.HashCode(45) //
					.AndThen.Has.HashCode(45);
			IEvaluation<Foo<int>, string> evaluation = specification.Evaluate(() => new Foo<int>());
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Failed));
			Assert.That(evaluation.Value, Is.Not.EqualTo(45));

			IEvaluation<Foo<int>, string> secondEvaluation = evaluation.Evaluate(() => new Foo<int>());
			Assert.That(secondEvaluation.Outcome, Is.EqualTo(Outcome.Failed));
			Assert.That(secondEvaluation.Value, Is.Not.EqualTo(45));
		}
	}
}
