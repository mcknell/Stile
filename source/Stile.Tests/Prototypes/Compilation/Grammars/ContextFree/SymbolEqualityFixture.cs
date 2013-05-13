#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.NUnit;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree
{
	[TestFixture]
	public class SymbolEqualityFixture : EqualityFixture<Symbol>
	{
		[Test]
		public void AliasDoesNotCount()
		{
			const string token = "token";
			var aliased = new Nonterminal(token, "alias");
			var plain = new Nonterminal(token);
			Assert.That(aliased.Alias, Is.Not.EqualTo(plain.Alias), "precondition");
			Assert.That(aliased.Token, Is.EqualTo(plain.Token), "precondition");

			Assert.That(aliased.Equals(plain), Is.True);
			Assert.That(plain.Equals(aliased), Is.True);
			Assert.That(plain.GetHashCode(), Is.EqualTo(aliased.GetHashCode()));
		}

		protected override Symbol OtherFactory()
		{
			return Nonterminal.Specification;
		}

		protected override Symbol TestSubjectFactory()
		{
			return Nonterminal.Deadline;
		}
	}
}
