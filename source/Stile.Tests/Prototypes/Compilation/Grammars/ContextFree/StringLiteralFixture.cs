#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree
{
	[TestFixture]
	public class StringLiteralFixture : ContextFreeFixtureBase
	{
		[Test]
		public void AliasIsQuoted()
		{
			var stringLiteral = new StringLiteral("HazGoz");
			Assert.That(stringLiteral.Token, Is.EqualTo("HazGoz"));
			Assert.That(stringLiteral.Alias, Is.EqualTo("\"HazGoz\""));
		}

		[Test]
		public void Prints()
		{
			AssertPrints(new StringLiteral("HazGoz"), "\"HazGoz\"");
			AssertPrints(TerminalSymbol.EBNFAssignment, "::=");
		}

		[Test]
		public void QuoteIfNeeded()
		{
			Assert.That(StringLiteral.QuoteIfNeeded(" d "), Is.EqualTo("\"d\""));
			Assert.That(StringLiteral.QuoteIfNeeded(" \"d "), Is.EqualTo("\"d\""));
			Assert.That(StringLiteral.QuoteIfNeeded(" d\" "), Is.EqualTo("\"d\""));
		}

		[Test]
		public void ValuesAreTrimmed()
		{
			var stringLiteral = new StringLiteral(" haz Goz ", " is not ");
			Assert.That(stringLiteral.Token, Is.EqualTo("HazGoz"));
			Assert.That(stringLiteral.Alias, Is.EqualTo("\"is not\""));
		}
	}
}
