#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Tests.Prototypes.Specifications.SemanticModel
{
	[TestFixture]
	public class OutcomeFixture
	{
		[Test]
		public void Make()
		{
			Assert.That(Outcome.Values, Is.Not.Null);
		}
	}
}
