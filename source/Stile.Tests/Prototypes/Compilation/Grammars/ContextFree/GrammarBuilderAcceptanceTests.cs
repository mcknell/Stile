#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Linq;
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree
{
	[TestFixture]
	public class GrammarBuilderAcceptanceTests
	{
		[Test]
		public void Builds()
		{
			var builder = new GrammarBuilder();
			new Reflector(builder).FindRules();

			// act
			IGrammar grammar = builder.Build();

			Assert.NotNull(grammar);
			IProductionRule expectation = grammar.ProductionRules.FirstOrDefault(x => x.Left == Nonterminal.Expectation);
			Assert.NotNull(expectation);
			Assert.That(expectation.Right.Members, Has.Member(TerminalSymbol.EBNFAlternation));
			Assert.That(expectation.Right.Members.Count, Is.EqualTo(9));
		}

		[Test]
		public void Prints()
		{
			var builder = new GrammarBuilder();
			new Reflector(builder).FindRules();
			IGrammar grammar = builder.Build();

			// act
			string ebnf = GrammarDescriber.Describe(grammar);

			Assert.NotNull(ebnf);
		}
	}
}
