#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Linq;
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	[TestFixture]
	public class GrammarBuilderAcceptanceTests
	{
		[Test]
		public void Builds()
		{
			var builder = new GrammarBuilder();
			new Reflector(builder).Find();

			// act
			IGrammar grammar = builder.Build();

			Assert.NotNull(grammar);
			IProduction expectation = grammar.Productions.FirstOrDefault(x => x.Left == Nonterminal.Expectation);
			Assert.NotNull(expectation);
			Assert.That(expectation.Right.Sequences.Count, Is.EqualTo(5));
		}

		[Test]
		public void Prints()
		{
			var builder = new GrammarBuilder();
			new Reflector(builder).Find();
			IGrammar grammar = builder.Build();

			// act
			string ebnf = GrammarDescriber.Describe(grammar);

			Assert.NotNull(ebnf);
		}
	}
}
