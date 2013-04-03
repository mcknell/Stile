#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
#endregion

namespace Stile.DocumentationGeneration.Tests
{
	[TestFixture]
	public class GeneratorAcceptanceTests
	{
		[Test]
		public void Prints()
		{
			string generated = new Generator().Generate();
			Assert.That(generated, Is.Not.Null);
			//StringAssert.Contains("Expectation ::= Is", generated);
			//StringAssert.Contains("Expectation ::= Has", generated);
		}
	}
}
