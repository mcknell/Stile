#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree
{
	[TestFixture]
	public class SymbolLinkFixture
	{
		[Test]
		public void HashCode()
		{
			Follower fooLink = MakeSymbolLinkWithMatchingSymbols("foo");
			Assert.That(fooLink.GetHashCode(),
				Is.Not.EqualTo(0),
				"Matching symbols should not degenerate to a zero hashcode");
			Follower barLink = MakeSymbolLinkWithMatchingSymbols("bar");
			Assert.That(fooLink.GetHashCode(),
				Is.Not.EqualTo(barLink.GetHashCode()),
				"Two links whose symbols are internal matches should not match each other");
		}

		private static Follower MakeSymbolLinkWithMatchingSymbols(string token)
		{
			var symbol = new Nonterminal(token);
			var symbolLink = new Follower(symbol, symbol);
			return symbolLink;
		}
	}
}
