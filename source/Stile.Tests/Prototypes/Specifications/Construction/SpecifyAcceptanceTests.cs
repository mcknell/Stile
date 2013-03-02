﻿#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Builders.OfInstruments;
using Stile.Prototypes.Specifications.Builders.OfSpecifications;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Tests.Prototypes.Specifications.SampleObjects;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Construction
{
	[TestFixture]
	public class SpecifyAcceptanceTests
	{
		[Test]
		public void BoundToExpression()
		{
			IBoundSpecification<Foo<int>, int> specification =
				Specify.For(() => new Foo<int>()).That(x => x.Count).Is.ComparablyEquivalentTo(7);
			Assert.That(specification, Is.Not.Null);
			IEvaluation<int> evaluation = specification.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Failed));
			Assert.That(evaluation.Value, Is.EqualTo(0));
		}

		[Test]
		public void BoundToInstance()
		{
			IBoundSpecification<Foo<int>, int> specification =
				Specify.For(new Foo<int>()).That(x => x.Count).Is.Not.EqualTo(12);
			Assert.That(specification, Is.Not.Null);
			IEvaluation<int> evaluation = specification.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(evaluation.Value, Is.EqualTo(0));
		}

		[Test]
		public void ThrowingBound()
		{
			var saboteur = new Saboteur();
			saboteur.Load(() => new ArgumentException());
			IThrowingSpecificationBuilder<IVoidBoundSpecification<Saboteur>, Saboteur> specificationBuilder =
				Specify.For(saboteur).That(x => x.Throw()).Throws<ArgumentException>();
			Assert.That(specificationBuilder, Is.Not.Null);
			IVoidBoundSpecification<Saboteur> specification = specificationBuilder.Build();
			IEvaluation evaluation = specification.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(saboteur.ThrowCalled);
		}

		[Test]
		public void ThrowingBoundInstrument()
		{
			var saboteur = new Saboteur();

			saboteur.Load(() => new ArgumentException());
			var target = new SabotageTarget(saboteur);
			IThrowingSpecificationBuilder<IBoundSpecification<SabotageTarget, Saboteur>, SabotageTarget>
				specificationBuilder =
					Specify.For(target).That(x => x.Saboteur.SuicidalSideEffect).Throws<ArgumentException>();
			Assert.That(specificationBuilder, Is.Not.Null);
			IBoundSpecification<SabotageTarget, Saboteur> specification = specificationBuilder.Build();
			IEvaluation<SabotageTarget, Saboteur> evaluation = specification.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(saboteur.ThrowCalled);
		}

		[Test]
		public void ThrowingUnbound()
		{
			var saboteur = new Saboteur();
			saboteur.Load(() => new ArgumentException());
			IThrowingSpecificationBuilder<IVoidSpecification<Saboteur>, Saboteur> specificationBuilder =
				Specify.ForAny<Saboteur>().That(x => x.Throw()).Throws<ArgumentException>();
			Assert.That(specificationBuilder, Is.Not.Null);
			IVoidSpecification<Saboteur> specification = specificationBuilder.Build();
			IEvaluation evaluation = specification.Evaluate(saboteur);
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(saboteur.ThrowCalled);
		}

		[Test]
		public void Unbound()
		{
			ISpecification<Foo<int>, string> specification =
				Specify.ForAny<Foo<int>>().That(x => x.ToString()).Has.HashCode(45);
			Assert.That(specification, Is.Not.Null);
			IEvaluation<string> evaluation = specification.Evaluate(new Foo<int>());
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Failed));
			Assert.That(evaluation.Value, Is.Not.EqualTo(45));
		}
	}
}
