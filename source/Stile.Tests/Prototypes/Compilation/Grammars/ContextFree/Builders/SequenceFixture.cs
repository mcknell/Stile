#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	[TestFixture]
	public class SequenceFixture
	{
		private Nonterminal _first;

		[SetUp]
		public void Init()
		{
			_first = new Nonterminal("first");
		}

		[Test]
		public void FirstNestedSymbol()
		{
			var nestedSequence = new Sequence(new Item(_first), new Item(new Nonterminal("second")));
			var testSubject = new Sequence(new Item(new Choice(nestedSequence)));

			Symbol symbol = testSubject.FirstSymbol();

			Assert.That(symbol, Is.EqualTo(_first));
		}

		[Test]
		public void FirstSymbol()
		{
			var testSubject = new Sequence(new Item(_first), new Item(new Nonterminal("second")));

			Symbol symbol = testSubject.FirstSymbol();

			Assert.That(symbol, Is.EqualTo(_first));
		}
	}
}
