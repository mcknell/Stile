#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Collections;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Compilation.Grammars.CodeMetadata;
using Stile.Prototypes.Specifications.Grammar;
using System.Linq;
#endregion

namespace Stile.DocumentationGeneration.Tests
{
	[TestFixture]
	public class GrammarBuilderFixture
	{
		[Test]
		public void Constructs()
		{
			var rules = new HashBucket<string, ProductionRule>();
			string specification = Nonterminal.Specification.ToString();
			string expectation = Nonterminal.Expectation.ToString();
			string deadline = Nonterminal.Deadline.ToString();
			rules.Add(specification, new ProductionRule(specification, expectation, deadline));
			var grammarBuilder = new GrammarBuilder(rules);
			Assert.That(grammarBuilder.Symbols, Has.Member(Symbol.Specification));
			Assert.That(grammarBuilder.Symbols, Has.Member(Symbol.Expectation));
			Assert.That(grammarBuilder.Symbols, Has.Member(Symbol.Deadline));
			Assert.That(grammarBuilder.Links.Count, Is.EqualTo(1));
			SymbolLink link = grammarBuilder.Links.FirstOrDefault();
			Assert.NotNull(link);
			Assert.That(link.Prior, Is.EqualTo(Symbol.Expectation));
			Assert.That(link.Next, Is.EqualTo(Symbol.Deadline));

			Assert.Fail("wip");
		}
	}
}
