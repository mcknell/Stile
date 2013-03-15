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
using Stile.Prototypes.Specifications.Printable;
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
		public void BoundToExpression()
		{
			var saboteur = new Saboteur(2);
			saboteur.Load(() => new ArgumentException());

			IEvaluation<Saboteur, int?> evaluation =
				Specify.For(() => saboteur).That(x => x.SuicidalSideEffect.MisfiresRemaining) //
					.Is.EqualTo(1) //
					.AndThen.Is.EqualTo(0) //
					.AndThen.Throws<ArgumentException>().Build() //
					.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(evaluation.Value, Is.EqualTo(1));
			Assert.That(evaluation.Errors.Length, Is.EqualTo(0));

			IEvaluation<Saboteur, int?> secondEvaluation = evaluation.EvaluateNext();
			Assert.That(secondEvaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(secondEvaluation.Value, Is.EqualTo(0));
			Assert.That(secondEvaluation.Xray.TailSpecification, Is.EqualTo(evaluation.Xray.TailSpecification));
			Assert.That(secondEvaluation.Errors.Length, Is.EqualTo(0));

			/* this should only pass if indeed the third specification is being evaluated;
		    * the first two would fail to throw */
			IEvaluation<Saboteur, int?> thirdEvaluation = secondEvaluation.EvaluateNext();
			Assert.That(thirdEvaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(thirdEvaluation.Errors.Length, Is.EqualTo(1));
			Assert.That(thirdEvaluation.Xray.TailSpecification, Is.EqualTo(secondEvaluation.Xray.TailSpecification));

			Assert.That(thirdEvaluation.EvaluateNext(), Is.Null);
		}

		[Test]
		public void BoundToInstance()
		{
			IBoundSpecification<Foo<int>, int, IFluentBoundExpectationBuilder<Foo<int>, int>> specification = //
				Specify.For(() => new Foo<int>()).That(x => x.Count) //
					.Is.Not.EqualTo(12) //
					.AndThen.Is.Not.EqualTo(12);
			IEvaluation<Foo<int>, int> evaluation = specification.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(evaluation.Value, Is.EqualTo(0));

			IEvaluation<Foo<int>, int> secondEvaluation = evaluation.ReEvaluate();
			Assert.That(secondEvaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(secondEvaluation.Value, Is.Not.EqualTo(12));
		}

		[Test]
		public void FirstEvaluationRefersToFirstSpecification()
		{
			var foo = new Foo<int>();
			IBoundSpecification<Foo<int>, int, IFluentBoundExpectationBuilder<Foo<int>, int>> first =
				Specify.For(() => foo).That(x => x.Bump()).Is.EqualTo(1);
			IBoundSpecification<Foo<int>, int, IFluentBoundExpectationBuilder<Foo<int>, int>> chained =
				first.AndThen.Is.EqualTo(2);

			IEvaluation<Foo<int>, int> firstEvaluation = chained.Evaluate();

			Assert.That(ReferenceEquals(firstEvaluation.Xray.Specification, first));

			IEvaluation<Foo<int>, int> reEvaluated = firstEvaluation.ReEvaluate();
			Assert.That(ReferenceEquals(reEvaluated.Xray.Specification, first));
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

			IEvaluation<Foo<int>, string> secondEvaluation = evaluation.EvaluateNext();
			Assert.That(secondEvaluation.Outcome, Is.EqualTo(Outcome.Failed));
			Assert.That(secondEvaluation.Value, Is.Not.EqualTo(45));
		}
	}
}
