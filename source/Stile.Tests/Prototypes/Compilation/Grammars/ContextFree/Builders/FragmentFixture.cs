#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public class FragmentLibrary
	{
		public readonly Fragment A_B = new Fragment("a", new Nonterminal("b"));
		public readonly Fragment B_C = new Fragment("b", new Nonterminal("c"));
		public readonly Fragment B_D = new Fragment("b", new Nonterminal("d"));
		public readonly Fragment C_D = new Fragment("c", new Nonterminal("d"));
	}

	[TestFixture]
	public class FragmentFixture
	{
		[Test]
		public void CompareTo()
		{
			var library = new FragmentLibrary();
			Assert.That(library.A_B.CompareTo(library.B_C), Is.EqualTo(-1));
			Assert.That(library.B_C.CompareTo(library.A_B), Is.EqualTo(1));
			Assert.That(library.A_B.CompareTo(library.C_D), Is.EqualTo(0));
		}
	}
}
