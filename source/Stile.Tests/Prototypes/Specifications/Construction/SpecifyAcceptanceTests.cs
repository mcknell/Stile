﻿#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq;
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Builders.OfInstruments;
using Stile.Prototypes.Specifications.Builders.OfSpecifications;
using Stile.Prototypes.Specifications.Printable;
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
		public void Before_WhenBoundToInstance()
		{
			IBoundSpecification<Foo<int>, int> specification =
				Specify.For(() => new Foo<int>()).That(x => x.Count).Is.Not.EqualTo(12).Before(TimeSpan.FromSeconds(1));
			IEvaluation<Foo<int>, int> evaluation = specification.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(evaluation.TimedOut, Is.False);
			Assert.That(evaluation.Errors.Length, Is.EqualTo(0));
			Assert.That(evaluation.ToPastTense(), Is.EqualTo(@"new Foo<int>().Count was not 12, in runtime < 1 second"));
			Assert.That(specification.ToShould(), Is.EqualTo(@"new Foo<int>().Count should not be 12, in runtime < 1 second"));
		}

		[Test]
		public void Before_WhenBoundToInstance_OnlyTimesOutOnAsync()
		{
			var saboteur = new Saboteur();
			saboteur.Load(() => new ArgumentException());
			saboteur.Fuse = TimeSpan.FromMilliseconds(1000);
			IBoundFaultSpecification<Saboteur> boundSpecification =
				Specify.For(() => saboteur)
					.That(x => x.Throw())
					.Throws<ArgumentException>()
					.Before(TimeSpan.FromMilliseconds(40));
			IEvaluation evaluation = boundSpecification.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Incomplete));
			Assert.That(evaluation.TimedOut, Is.True);
			Assert.That(evaluation.Errors.Length, Is.EqualTo(0));

			IEvaluation synchronousEvaluation = boundSpecification.Evaluate(Deadline.Synchronous);
			Assert.That(synchronousEvaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(synchronousEvaluation.TimedOut, Is.False);
		}

		[Test]
		public void BoundToExpression()
		{
			IBoundSpecification<Foo<int>, int> specification =
				Specify.For(() => new Foo<int>()).That(x => x.Count).Is.ComparablyEquivalentTo(7);
			IEvaluation<Foo<int>, int> evaluation = specification.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Failed));
			Assert.That(evaluation.Value, Is.EqualTo(0));
			Assert.That(evaluation.ToPastTense(),
				Is.EqualTo(@"Expected that new Foo<int>().Count would be neither greater nor less than 7
but was 0"));
		}

		[Test]
		public void BoundToInstance()
		{
			IBoundSpecification<Foo<int>, int> specification =
				Specify.For(() => new Foo<int>()).That(x => x.Count).Is.Not.EqualTo(12);
			IEvaluation<Foo<int>, int> evaluation = specification.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(evaluation.Value, Is.EqualTo(0));
			Assert.That(evaluation.ToPastTense(), Is.EqualTo(@"new Foo<int>().Count was not 12"));
			Assert.That(specification.ToShould(), Is.EqualTo(@"new Foo<int>().Count should not be 12"));
		}

		[Test]
		public void FailsToThrowBound()
		{
			IBoundFaultSpecification<Foo<string>> specification =
				Specify.For(() => new Foo<string>()).That(x => x.Clear()).Throws<ArgumentException>().Build();
			IEvaluation evaluation = specification.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Failed));
		}

		[Test]
		public void FailsToThrowBoundInstrument()
		{
			IBoundSpecification<Foo<int>, int, IFluentBoundExpectationBuilder<Foo<int>, int>> specification =
				Specify.For(() => new Foo<int>()).That(x => x.Count).Throws<ArgumentException>().Build();
			IEvaluation<Foo<int>, int> evaluation = specification.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Failed));
		}

		[Test]
		public void FailsToThrowUnbound()
		{
			IFaultSpecification<Foo<string>> specification =
				Specify.ForAny<Foo<string>>().That(x => x.Clear()).Throws<ArgumentException>().Build();
			IEvaluation evaluation = specification.Evaluate(() => new Foo<string>());
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Failed));
		}

		[Test]
		public void ThrowingBound()
		{
			var saboteur = new Saboteur();
			saboteur.Load(() => new ArgumentException());
			IFaultSpecificationBuilder<IBoundFaultSpecification<Saboteur>, Saboteur, ArgumentException>
				specificationBuilder = Specify.For(() => saboteur).That(x => x.Throw()).Throws<ArgumentException>();
			IBoundFaultSpecification<Saboteur> specification = specificationBuilder.Build();
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
			IFaultSpecificationBuilder
				<IBoundSpecification<SabotageTarget, Saboteur, IFluentBoundExpectationBuilder<SabotageTarget, Saboteur>>,
					SabotageTarget, ArgumentException> specificationBuilder =
						Specify.For(() => target).That(x => x.Saboteur.SuicidalSideEffect).Throws<ArgumentException>();
			IBoundSpecification<SabotageTarget, Saboteur> specification = specificationBuilder.Build();
			IEvaluation<SabotageTarget, Saboteur> evaluation = specification.Evaluate();
			Assert.That(evaluation.Errors.Any(), Is.True);
			Assert.That(evaluation.Errors.First().Exception, Is.InstanceOf<ArgumentException>());
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(saboteur.ThrowCalled);
		}

		[Test]
		public void ThrowingUnbound()
		{
			var saboteur = new Saboteur();
			saboteur.Load(() => new ArgumentException());
			IFaultSpecificationBuilder<IFaultSpecification<Saboteur>, Saboteur, ArgumentException> specificationBuilder
				= Specify.ForAny<Saboteur>().That(x => x.Throw()).Throws<ArgumentException>();
			IFaultSpecification<Saboteur> specification = specificationBuilder.Build();
			IEvaluation evaluation = specification.Evaluate(saboteur);
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(saboteur.ThrowCalled);
		}

		[Test]
		public void Unbound()
		{
			ISpecification<Foo<int>, string> specification =
				Specify.ForAny<Foo<int>>().That(x => x.ToString()).Has.HashCode(45);
			IEvaluation<Foo<int>, string> evaluation = specification.Evaluate(new Foo<int>());
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Failed));
			Assert.That(evaluation.Value, Is.Not.EqualTo(45));
		}
	}
}
