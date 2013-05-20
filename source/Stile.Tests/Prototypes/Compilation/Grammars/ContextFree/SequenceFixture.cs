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
	public class SequenceFixture
	{
		[Test]
		public void Prints()
		{
			var sequence = new Sequence(new Item(Nonterminal.Source, Cardinality.ZeroOrOne),
				new Item(Nonterminal.Inspection));

			Assert.That(sequence.ToString(), Is.EqualTo("Source? Inspection"));
		}
	}
}
