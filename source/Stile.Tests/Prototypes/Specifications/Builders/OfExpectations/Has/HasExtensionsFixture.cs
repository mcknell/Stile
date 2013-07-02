#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has.Quantifiers;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Iz = NUnit.Framework.Is;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Builders.OfExpectations.Has
{
	[TestFixture]
	public class HasExtensionsFixture
	{
		private Expression<Func<int, bool>> _expression;
		private List<int> _list;
		private string _quantifier;

		[SetUp]
		public void Init()
		{
			_list = new List<int>();
			_expression = x => x == 1;
		}

		[Test]
		public void AtLeast()
		{
			_quantifier = "at least";
			IEvaluation<List<int>, List<int>> evaluation = MakeSatisfyingEvaluation(Has.AtLeast(1));
			AssertSuccessesProgressingFrom0To2(evaluation, Outcome.Failed, Outcome.Succeeded, Outcome.Succeeded);

			evaluation = MakeFailingEvaluation(Has.AtLeast(1));
			AssertFailuresProgressingFrom0To2(evaluation, Outcome.Failed, Outcome.Succeeded, Outcome.Succeeded);
		}

		[Test]
		public void AtMost()
		{
			_quantifier = "at most";
			IEvaluation<List<int>, List<int>> evaluation = MakeSatisfyingEvaluation(Has.AtMost(1));
			AssertSuccessesProgressingFrom0To2(evaluation, Outcome.Succeeded, Outcome.Succeeded, Outcome.Failed);

			evaluation = MakeFailingEvaluation(Has.AtMost(1));
			AssertFailuresProgressingFrom0To2(evaluation, Outcome.Succeeded, Outcome.Succeeded, Outcome.Failed);
		}

		[Test]
		public void Exactly()
		{
			_quantifier = "exactly";
			IEvaluation<List<int>, List<int>> evaluation = MakeSatisfyingEvaluation(Has.Exactly(1));
			AssertSuccessesProgressingFrom0To2(evaluation, Outcome.Failed, Outcome.Succeeded, Outcome.Failed);
			
			evaluation = MakeFailingEvaluation(Has.Exactly(1));
			AssertFailuresProgressingFrom0To2(evaluation, Outcome.Failed, Outcome.Succeeded, Outcome.Failed);
		}

		[Test]
		public void FewerThan()
		{
			_quantifier = "fewer than";
			IEvaluation<List<int>, List<int>> evaluation = MakeSatisfyingEvaluation(Has.FewerThan(1));
			AssertSuccessesProgressingFrom0To2(evaluation, Outcome.Succeeded, Outcome.Failed, Outcome.Failed);
			
			evaluation = MakeFailingEvaluation(Has.FewerThan(1));
			AssertFailuresProgressingFrom0To2(evaluation, Outcome.Succeeded, Outcome.Failed, Outcome.Failed);
		}

		[Test]
		public void MoreThan()
		{
			_quantifier = "more than";
			IEvaluation<List<int>, List<int>> evaluation = MakeSatisfyingEvaluation(Has.MoreThan(1));
			AssertSuccessesProgressingFrom0To2(evaluation, Outcome.Failed, Outcome.Failed, Outcome.Succeeded);
			
			evaluation = MakeFailingEvaluation(Has.MoreThan(1));
			AssertFailuresProgressingFrom0To2(evaluation, Outcome.Failed, Outcome.Failed, Outcome.Succeeded);
		}

		private IEnumerableHas<IBoundSpecification<List<int>, List<int>>, List<int>, List<int>, int> Has
		{
			get { return Specify.That(() => _list).OfItemsLike(0).Has; }
		}

// ReSharper disable UnusedParameter.Local

// ReSharper disable UnusedParameter.Local
		private void AssertFailuresProgressingFrom0To2(IEvaluation<List<int>, List<int>> evaluation,
			Outcome zero,
			Outcome one,
			Outcome two) //
// ReSharper restore UnusedParameter.Local
		{
			Func<int, bool> failingPredicate = x => _expression.Compile().Invoke(x) == false;
			_list.Clear();

			Assert.That(_list.Count(failingPredicate), Iz.EqualTo(0), "precondition");
			Assert.That(evaluation.Outcome, Iz.EqualTo(zero));

			_list.Add(0);
			Assert.That(_list.Count(failingPredicate), Iz.EqualTo(1), "precondition");
			Assert.That(evaluation.ReEvaluate().Outcome, Iz.EqualTo(one));

			_list.Add(0);
			Assert.That(_list.Count(failingPredicate), Iz.EqualTo(2), "precondition");
			Assert.That(evaluation.ReEvaluate().Outcome == two);

			AssertPastTenseContains(evaluation, "failing the test '== 1'");
		}

		private void AssertPastTenseContains(IEvaluation<List<int>, List<int>> evaluation, string criterion)
		{
			string expected = string.Format("_list should have {0} 1 item {1}", _quantifier, criterion);
			Assert.That(evaluation.ToPastTense(), Contains.Substring(expected));
		}

		private void AssertSuccessesProgressingFrom0To2(IEvaluation<List<int>, List<int>> evaluation,
			Outcome zero,
			Outcome one,
			Outcome two) //
// ReSharper restore UnusedParameter.Local
		{
			Func<int, bool> predicate = _expression.Compile();
			_list.Clear();

			Assert.That(_list.Count(predicate), Iz.EqualTo(0), "precondition");
			IEvaluation<List<int>, List<int>> reEvaluation = evaluation.ReEvaluate();
			Assert.That(reEvaluation.Outcome, Iz.EqualTo(zero));

			_list.Add(1);
			Assert.That(_list.Count(predicate), Iz.EqualTo(1), "precondition");
			reEvaluation = evaluation.ReEvaluate();
			Assert.That(reEvaluation.Outcome, Iz.EqualTo(one));

			_list.Add(1);
			Assert.That(_list.Count(predicate), Iz.EqualTo(2), "precondition");
			reEvaluation = evaluation.ReEvaluate();
			Assert.That(reEvaluation.Outcome == two);

			AssertPastTenseContains(evaluation, "== 1");
		}

		private IEvaluation<List<int>, List<int>> MakeFailingEvaluation(
			IQuantifier<IBoundSpecification<List<int>, List<int>>, int> quantifier)
		{
			return quantifier.ItemsFailing(_expression).Evaluate();
		}

		private IEvaluation<List<int>, List<int>> MakeSatisfyingEvaluation(
			IQuantifier<IBoundSpecification<List<int>, List<int>>, int> quantifier)
		{
			return quantifier.ItemsSatisfying(_expression).Evaluate();
		}
	}
}
