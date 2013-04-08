#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Compilation.Grammars.CodeMetadata;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.CodeMetadata
{
	[TestFixture]
	public class SymbolLinkFixture
	{
		[Test]
		public void HashCode()
		{
			SymbolLink fooLink = MakeSymbolLinkWithMatchingSymbols("foo");
			Assert.That(fooLink.GetHashCode(),
				Is.Not.EqualTo(0),
				"Matching symbols should not degenerate to a zero hashcode");
			SymbolLink barLink = MakeSymbolLinkWithMatchingSymbols("bar");
			Assert.That(fooLink.GetHashCode(),
				Is.Not.EqualTo(barLink.GetHashCode()),
				"Two links whose symbols are internal matches should not match each other");
		}

		private static SymbolLink MakeSymbolLinkWithMatchingSymbols(string token)
		{
			var symbol = new Symbol(token);
			var symbolLink = new SymbolLink(symbol, symbol);
			return symbolLink;
		}
	}
}
