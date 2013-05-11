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
			IEvaluation<int, int> evaluation = Is.ComparablyEquivalentTo(1).Evaluate();
			AssertFrom0To2(evaluation, Outcome.Failed, Outcome.Succeeded, Outcome.Failed);
			AssertPastTenseContains(evaluation, "_int should be neither greater nor less than 1");
		}

		[Test]
		public void GreaterThan()
		{
			IEvaluation<int, int> evaluation = Is.GreaterThan(1).Evaluate();
			AssertFrom0To2(evaluation, Outcome.Failed, Outcome.Failed, Outcome.Succeeded);
			AssertPastTenseContains(evaluation, "_int should be > 1");
		}

		[Test]
		public void GreaterThanOrEqualTo()
		{
			IEvaluation<int, int> evaluation = Is.GreaterThanOrEqualTo(1).Evaluate();
			AssertFrom0To2(evaluation, Outcome.Failed, Outcome.Succeeded, Outcome.Succeeded);
			AssertPastTenseContains(evaluation, "_int should be >= 1");
		}

		[Test]
		public void LessThan()
		{
			IEvaluation<int, int> evaluation = Is.LessThan(1).Evaluate();
			AssertFrom0To2(evaluation, Outcome.Succeeded, Outcome.Failed, Outcome.Failed);
			AssertPastTenseContains(evaluation, "_int should be < 1");
		}

		[Test]
		public void LessThanOrEqualTo()
		{
			IEvaluation<int, int> evaluation = Is.LessThanOrEqualTo(1).Evaluate();
			AssertFrom0To2(evaluation, Outcome.Succeeded, Outcome.Succeeded, Outcome.Failed);
			AssertPastTenseContains(evaluation, "_int should be <= 1");
		}

		private
			INegatableIs
				<IBoundSpecification<int, int, IFluentBoundExpectationBuilder<int, int>>, int, int,
					IIs<IBoundSpecification<int, int, IFluentBoundExpectationBuilder<int, int>>, int, int>> Is
		{
			get { return Specify.That(() => _int).Is; }
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

		private static void AssertPastTenseContains(IEvaluation<int, int> evaluation, string substring)
		{
			Assert.That(evaluation.ToPastTense(), Contains.Substring(substring));
		}
	}
}
