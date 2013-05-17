#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Builders.OfExpectations.Is
{
	[TestFixture]
	public class IsComparableExtensionsTests
	{
		private int _int;

		[SetUp]
		public void Init()
		{
			_int = 0;
		}

		[Test]
		public void ComparablyEquivalentTo()
		{
			AssertFrom0To2((x, y) => x.ComparablyEquivalentTo(y),
				Outcome.Failed,
				Outcome.Succeeded,
				Outcome.Failed,
				"neither greater nor less than",
				"either greater or less than");
		}

		[Test]
		public void GreaterThan()
		{
			AssertFrom0To2((x, y) => x.GreaterThan(y), Outcome.Failed, Outcome.Failed, Outcome.Succeeded, ">");
		}

		[Test]
		public void GreaterThanOrEqualTo()
		{
			AssertFrom0To2((x, y) => x.GreaterThanOrEqualTo(y),
				Outcome.Failed,
				Outcome.Succeeded,
				Outcome.Succeeded,
				">=");
		}

		[Test]
		public void LessThan()
		{
			AssertFrom0To2((x, y) => x.LessThan(y), Outcome.Succeeded, Outcome.Failed, Outcome.Failed, "<");
		}

		[Test]
		public void LessThanOrEqualTo()
		{
			AssertFrom0To2((x, y) => x.LessThanOrEqualTo(y), Outcome.Succeeded, Outcome.Succeeded, Outcome.Failed, "<=");
		}

// ReSharper disable UnusedParameter.Local
		private void AssertFrom0To2(IEvaluation<int, int> evaluation, Outcome zero, Outcome one, Outcome two) //
// ReSharper restore UnusedParameter.Local
		{
			_int = 0;
			Assert.That(evaluation.ReEvaluate().Outcome == zero);
			_int = 1;
			Assert.That(evaluation.ReEvaluate().Outcome == one);
			_int = 2;
			Assert.That(evaluation.ReEvaluate().Outcome == two);
		}

		private void AssertFrom0To2(Extension extension,
			Outcome zero,
			Outcome one,
			Outcome two,
			string shouldBe,
			string shouldNot = null)
		{
			IEvaluation<int, int> evaluation = extension.Invoke(Specify.That(() => _int).Is, 1).Evaluate();
			AssertFrom0To2(evaluation, zero, one, two);
			IEvaluation<int, int> negative = extension.Invoke(Specify.That(() => _int).Is.Not, 1).Evaluate();
			AssertFrom0To2(negative, Invert(zero), Invert(one), Invert(two));
			AssertPastTenseContains(evaluation, string.Format("_int should be {0} 1", shouldBe));
			AssertPastTenseContains(negative, string.Format("_int should not be {0} 1", shouldNot ?? shouldBe));
		}

		private static void AssertPastTenseContains(IEvaluation<int, int> evaluation, string substring)
		{
			Assert.That(evaluation.ToPastTense(), Contains.Substring(substring));
		}

		private Outcome Invert(Outcome outcome)
		{
			return outcome == Outcome.Succeeded ? Outcome.Failed : Outcome.Succeeded;
		}

		private delegate IBoundSpecification<int, int, IFluentBoundExpectationBuilder<int, int>> Extension(
			IIs<IBoundSpecification<int, int, IFluentBoundExpectationBuilder<int, int>>, int, int> negatableIs, int i);
	}
}
