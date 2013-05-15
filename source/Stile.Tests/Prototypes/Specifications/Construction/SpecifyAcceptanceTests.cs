#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Builders.OfProcedures;
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
		private static readonly TimeSpan _SaboteurFuse = TimeSpan.FromMilliseconds(15);
		private readonly TimeSpan _deadline = TimeSpan.FromMilliseconds(5);
		private Saboteur _saboteur;
		private bool _writeToConsole;

		[TestFixtureSetUp]
		public void FixtureInit()
		{
			bool b;
			_writeToConsole = bool.TryParse(ConfigurationManager.AppSettings["TraceThreadsToConsole"], out b) && b;
		}

		[SetUp]
		public void Init()
		{
			_saboteur = MakeSaboteur();
		}

		[Test]
		public void Before_WhenBoundToInstance()
		{
			IBoundSpecification<Foo<int>, int> specification =
				Specify.For(() => new Foo<int>()).That(x => x.Count).Is.Not.EqualTo(12).Before(TimeSpan.FromSeconds(1));
			IEvaluation<Foo<int>, int> evaluation = specification.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(evaluation.TimedOut, Is.False);
			Assert.That(evaluation.Errors.Count, Is.EqualTo(0));
			Assert.That(evaluation.ToPastTense(),
				Is.EqualTo(@"new Foo<int>().Count should not be 12, in runtime < 1 second"));
		}

		[Test]
		public void Before_WhenBoundToInstance_OnlyTimesOutOnAsync()
		{
			IFaultEvaluation<Saboteur> evaluation =
				Specify.For(() => _saboteur).That(x => x.Throw()).Throws<ArgumentException>().Before(_deadline).Evaluate();
			IFaultEvaluation<Saboteur> synchronousEvaluation = evaluation.ReEvaluate(Deadline.Synchronous);

			Before_OnlyTimesOutOnAsync(evaluation,
				synchronousEvaluation,
				evaluation.ToPastTense(),
				synchronousEvaluation.ToPastTense(),
				"Throw()");
		}

		[Test]
		public void Before_WhenBoundToInstrumentInstance_OnlyTimesOutOnAsync()
		{
			IEvaluation<Saboteur, Saboteur> evaluation =
				Specify.For(() => _saboteur)
					.That(x => x.SuicidalSideEffect)
					.Throws<ArgumentException>()
					.Before(_deadline)
					.Evaluate();
			IEvaluation<Saboteur, Saboteur> synchronousEvaluation = evaluation.ReEvaluate(Deadline.Synchronous);

			Before_OnlyTimesOutOnAsync(evaluation,
				synchronousEvaluation,
				evaluation.ToPastTense(),
				synchronousEvaluation.ToPastTense(),
				"SuicidalSideEffect");
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
				Is.EqualTo(@"new Foo<int>().Count should be neither greater nor less than 7
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
			string pastTense = evaluation.ToPastTense();
			Assert.That(pastTense, Is.EqualTo(@"new Foo<int>().Count should not be 12"));
			Assert.That(specification.ToShould(), Is.EqualTo(pastTense));
		}

		[Test]
		public void FailsToThrowBound()
		{
			IBoundFaultSpecification<Foo<string>> specification =
				Specify.For(() => new Foo<string>()).That(x => x.Clear()).Throws<ArgumentException>();
			IFaultEvaluation<Foo<string>> evaluation = specification.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Failed));
			Assert.That(evaluation.ToPastTense(), Is.EqualTo(@"new Foo<string>().Clear() should throw ArgumentException
but no exception was thrown"));
		}

		[Test]
		public void FailsToThrowBoundInstrument()
		{
			IBoundSpecification<Foo<int>, int, IFluentBoundExpectationBuilder<Foo<int>, int>> specification =
				Specify.For(() => new Foo<int>()).That(x => x.Count).Throws<ArgumentException>();
			IEvaluation<Foo<int>, int> evaluation = specification.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Failed));
			Assert.That(evaluation.ToPastTense(), Is.EqualTo(@"new Foo<int>().Count should throw ArgumentException
but was 0 and no exception was thrown"));
		}

		[Test]
		public void FailsToThrowUnbound()
		{
			IFaultSpecification<Foo<string>> specification =
				Specify.ForAny<Foo<string>>().That(x => x.Clear()).Throws<ArgumentException>();
			IFaultEvaluation<Foo<string>> evaluation = specification.Evaluate(() => new Foo<string>());
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Failed));
			Assert.That(evaluation.ToPastTense(), Is.EqualTo(@"new Foo<string>().Clear() should throw ArgumentException
but no exception was thrown"));
			Assert.That(specification.ToShould(), Is.EqualTo(@"Any Foo<string>.Clear() should throw ArgumentException"));
		}

		[Test]
		public void ThrowingBound()
		{
			var saboteur = new Saboteur();
			saboteur.Load(() => new ArgumentException());
			IBoundFaultSpecification<Saboteur> specification =
				Specify.For(() => saboteur).That(x => x.Throw()).Throws<ArgumentException>();
			IFaultEvaluation<Saboteur> evaluation = specification.Evaluate();
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(evaluation.Errors, Is.Not.Empty);
			Assert.That(saboteur.ThrowCalled);
			string pastTense = evaluation.ToPastTense();
			Assert.That(pastTense, Is.EqualTo(@"saboteur.Throw() should throw ArgumentException"));
			Assert.That(specification.ToShould(), Is.EqualTo(pastTense));
		}

		[Test]
		public void ThrowingBoundInstrument()
		{
			var saboteur = new Saboteur();
			saboteur.Load(() => new ArgumentException());
			var target = new SabotageTarget(saboteur);
			IBoundSpecification<SabotageTarget, Saboteur> specification =
				Specify.For(() => target).That(x => x.Saboteur.SuicidalSideEffect).Throws<ArgumentException>();
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
			IFaultSpecification<Saboteur> specification =
				Specify.ForAny<Saboteur>().That(x => x.Throw()).Throws<ArgumentException>();
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

		private void Before_OnlyTimesOutOnAsync(IEvaluation<Saboteur> evaluation,
			IEvaluation<Saboteur> synchronousEvaluation,
			string description,
			string synchronousDescription,
			string member)
		{
			Trace();
			Assert.NotNull(evaluation.Sample, "precondition");
			Assert.That(evaluation.Sample.Value.Fuse, Is.GreaterThan(_deadline), "precondition");
			Assert.That(evaluation.Outcome, Is.EqualTo(Outcome.Incomplete));
			Assert.That(evaluation.TimedOut, Is.True);
			Assert.That(evaluation.Errors.Count, Is.EqualTo(1));
			Trace(2);
			Assert.NotNull(synchronousEvaluation.Sample, "precondition");
			Assert.That(synchronousEvaluation.Sample.Value.Fuse, Is.GreaterThan(_deadline), "precondition");
			Assert.That(synchronousEvaluation.Outcome, Is.EqualTo(Outcome.Succeeded));
			Assert.That(synchronousEvaluation.TimedOut, Is.False);
			Assert.That(evaluation.Errors.Count, Is.GreaterThan(0));

			string expected = string.Format(@"_saboteur.{1} should throw ArgumentException, in runtime < {0}ms",
				_deadline.TotalMilliseconds,
				member);
			Assert.That(synchronousDescription, Is.EqualTo(expected));
			Assert.That(description, Is.EqualTo(string.Format(@"{0}
but timed out", expected)));
		}

		private static Saboteur MakeSaboteur()
		{
			var saboteur = new Saboteur();
			saboteur.Load(() => new ArgumentException());
			saboteur.Fuse = _SaboteurFuse;
			return saboteur;
		}

		private void Trace(int? number = null)
		{
			if (_writeToConsole)
			{
				Console.WriteLine("asserting{3} on thread {0}, background=={1}, pool=={2}",
					Thread.CurrentThread.ManagedThreadId,
					Thread.CurrentThread.IsBackground,
					Thread.CurrentThread.IsThreadPoolThread,
					number.HasValue ? number.ToString() : string.Empty);
			}
		}
	}
}
