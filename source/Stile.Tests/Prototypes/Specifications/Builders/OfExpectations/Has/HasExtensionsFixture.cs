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
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Iz = NUnit.Framework.Is;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Builders.OfExpectations.Has
{
	[TestFixture]
	public class HasExtensionsFixture
	{
		[Test]
		public void AtLeast()
		{
			var list = new List<int>();
			Expression<Func<int, bool>> expression = x => x == 1;
			Func<int, bool> predicate = expression.Compile();
			IEvaluation<List<int>, List<int>> evaluation =
				Specify.That(() => list).OfItemsLike(0).Has.AtLeast(1).ItemsSatisfying(expression).Evaluate();
			Assert.That(list.Count(predicate), Iz.EqualTo(0));
			Assert.That(evaluation.Outcome == Outcome.Failed);

			list.Add(1);
			Assert.That(list.Count(predicate), Iz.EqualTo(1));
			Assert.That(evaluation.ReEvaluate().Outcome == Outcome.Succeeded);

			list.Add(1);
			Assert.That(list.Count(predicate), Iz.EqualTo(2));
			Assert.That(evaluation.ReEvaluate().Outcome == Outcome.Succeeded);
		}
	}
}
