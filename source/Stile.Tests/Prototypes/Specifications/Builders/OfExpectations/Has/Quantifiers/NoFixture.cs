#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Tests.Prototypes.Specifications.SampleObjects;
using Iz = NUnit.Framework.Is;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Builders.OfExpectations.Has.Quantifiers
{
	[TestFixture]
	public class NoFixture
	{
		[Test]
		public void No()
		{
			var baz = new Baz<int>();
			IEvaluation<Baz<int>, Baz<int>> evaluation =
				Specify.ThatAny<Baz<int>>().OfItemsLike(0).Has.No.ItemsSatisfying(x => x < 0).Evaluate(() => baz);

			Assert.That(evaluation.Outcome == Outcome.Succeeded);
			Assert.That(evaluation.ToPastTense(), Iz.EqualTo("baz should have no items < 0"));
		}
	}
}
