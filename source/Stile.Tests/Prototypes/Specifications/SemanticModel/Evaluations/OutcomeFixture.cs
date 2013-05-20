#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Tests.Prototypes.Specifications.SemanticModel.Evaluations
{
	[TestFixture]
	public class OutcomeFixture
	{
		[Test]
		public void Covers()
		{
			IEnumerable<Outcome> valuesBesidesTimedOut = Outcome.Values.Where(x => x != Outcome.TimedOut);
			foreach (Outcome outcome in valuesBesidesTimedOut)
			{
				Outcome copy = outcome;
				IEnumerable<Outcome> remainingValues = valuesBesidesTimedOut.Where(x => x != copy && x != Outcome.Failed);
				foreach (Outcome remainingValue in remainingValues)
				{
					Assert.That(outcome.Covers(remainingValue),
						Is.False,
						string.Format("{0} should not cover {1}", outcome, remainingValue));
				}
			}
			Assert.That(Outcome.TimedOut.Covers(Outcome.Incomplete));
			Assert.That(Outcome.Incomplete.IsCoveredBy(Outcome.TimedOut));
		}

		[Test]
		public void Distinct()
		{
			ReadOnlyCollection<Outcome> values = Outcome.Values;
			foreach (Outcome outcome in values)
			{
				Outcome copy = outcome;
				IEnumerable<Outcome> remainingValues = values.Where(x => x != copy);
				foreach (Outcome remainingValue in remainingValues)
				{
					Assert.That(outcome.Equals(remainingValue),
						Is.False,
						string.Format("{0}.Equals({1}) should fail", outcome, remainingValue));
					Assert.That(outcome == remainingValue,
						Is.False,
						string.Format("{0} == {1} should fail", outcome, remainingValue));
					Assert.That(outcome != remainingValue,
						Is.True,
						string.Format("{0} != {1} should succeed", outcome, remainingValue));
				}
			}
		}
	}
}
