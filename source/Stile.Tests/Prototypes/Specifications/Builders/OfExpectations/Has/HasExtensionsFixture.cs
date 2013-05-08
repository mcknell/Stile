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

		[SetUp]
		public void Init()
		{
			_list = new List<int>();
			_expression = x => x == 1;
		}

		[Test]
		public void AtLeast()
		{
			IEvaluation<List<int>, List<int>> evaluation = MakeEvaluation(Has.AtLeast(1));
			AssertCountProgressingFrom0To2(evaluation, Outcome.Failed, Outcome.Succeeded, Outcome.Succeeded);
		}

		[Test]
		public void Exactly()
		{
			IEvaluation<List<int>, List<int>> evaluation = MakeEvaluation(Has.Exactly(1));
			AssertCountProgressingFrom0To2(evaluation, Outcome.Failed, Outcome.Succeeded, Outcome.Failed);
		}

		private IEnumerableHas<IBoundSpecification<List<int>, List<int>>, List<int>, List<int>, int> Has
		{
			get { return Specify.That(() => _list).OfItemsLike(0).Has; }
		}

// ReSharper disable UnusedParameter.Local
		private void AssertCountProgressingFrom0To2(IEvaluation<List<int>, List<int>> evaluation,
			Outcome zero,
			Outcome one,
			Outcome two) // ReSharper restore UnusedParameter.Local
		{
			Func<int, bool> predicate = _expression.Compile();

			Assert.That(_list.Count(predicate), Iz.EqualTo(0));
			Assert.That(evaluation.Outcome == zero);

			_list.Add(1);
			Assert.That(_list.Count(predicate), Iz.EqualTo(1));
			Assert.That(evaluation.ReEvaluate().Outcome == one);

			_list.Add(1);
			Assert.That(_list.Count(predicate), Iz.EqualTo(2));
			Assert.That(evaluation.ReEvaluate().Outcome == two);
		}

		private IEvaluation<List<int>, List<int>> MakeEvaluation(
			IQuantifier<IBoundSpecification<List<int>, List<int>>, int> quantifier)
		{
			return quantifier.ItemsSatisfying(_expression).Evaluate();
		}
	}
}
