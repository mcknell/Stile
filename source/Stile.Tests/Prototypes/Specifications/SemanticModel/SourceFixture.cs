#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications.SemanticModel;
#endregion

namespace Stile.Tests.Prototypes.Specifications.SemanticModel
{
	[TestFixture]
	public class SourceFixture
	{
		[Test]
		public void FromExpression()
		{
			var source = new Source<int>(() => 4);
			Assert.That(source.Get(), Is.EqualTo(4));
		}

		[Test]
		public void FromInstance()
		{
			var source = new Source<int>(4);
			Assert.That(source.Get(), Is.EqualTo(4));
		}
	}
}
