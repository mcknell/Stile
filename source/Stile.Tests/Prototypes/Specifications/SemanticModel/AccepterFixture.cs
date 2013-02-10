#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Tests.Prototypes.Specifications.SemanticModel
{
	[TestFixture]
	public class AccepterFixture
	{
		[Test]
		public void FromExpression()
		{
			var source = new Criterion<int>(x => x == 4 ? Outcome.Succeeded : Outcome.Failed);
			Assert.That(source.Accept(4), Is.EqualTo(true));
			Assert.That(source.Accept(5), Is.Not.EqualTo(true));
			Assert.That(source.Description.Value, Is.EqualTo("x => x == 4"));
		}
	}
}
