#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications.Grammar.Metadata;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree
{
	[TestFixture]
	public class NonterminalSymbolAttributeFixture
	{
		[Test]
		public void PublicSetterCannotSetTerminal()
		{
			var attribute = new NonterminalSymbolAttribute {Terminal = true};
			Assert.That(attribute.Terminal, Is.EqualTo(false));
		}
	}
}
