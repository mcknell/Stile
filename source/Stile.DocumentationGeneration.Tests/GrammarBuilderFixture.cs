#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Linq;
using NUnit.Framework;
using Stile.Prototypes.Collections;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Compilation.Grammars.CodeMetadata;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.DocumentationGeneration.Tests
{
	[TestFixture]
	public class GrammarBuilderFixture
	{
		[Test]
		public void AddsLink()
		{
			var rules = new HashBucket<string, ProductionRule>();
			var testSubject = new GrammarBuilder(rules);
			Assert.That(testSubject.Links, Is.Empty, "precondition");
			Assert.That(testSubject.Symbols, Is.Empty, "precondition");
			string has = Nonterminal.Has.ToString();
			string hashcode = Nonterminal.HashCode.ToString();

			// act
			testSubject.AddLink(has, hashcode);

			Assert.That(testSubject.Symbols.Any(x => x.Token == has));
			Assert.That(testSubject.Symbols.Any(x => x.Token == hashcode));
			Assert.That(testSubject.Symbols.Count, Is.EqualTo(2));

			Assert.That(testSubject.Links.Count, Is.EqualTo(1));
			SymbolLink link = testSubject.Links.FirstOrDefault();
			Assert.NotNull(link);
			Assert.That(link.Prior.Token, Is.EqualTo(has));
			Assert.That(link.Current.Token, Is.EqualTo(hashcode));
		}

		[Test]
		public void ConstructsFromRules()
		{
			string specification = Nonterminal.Specification.ToString();
			string expectation = Nonterminal.Expectation.ToString();
			string deadline = Nonterminal.Deadline.ToString();
			var productionRule = new ProductionRule(specification, expectation, deadline);
			var rules = new HashBucket<string, ProductionRule> {{specification, productionRule}};

			// act
			var testSubject = new GrammarBuilder(rules);

			Assert.That(testSubject.Symbols, Has.Member(Symbol.Specification));
			Assert.That(testSubject.Symbols, Has.Member(Symbol.Expectation));
			Assert.That(testSubject.Symbols, Has.Member(Symbol.Deadline));
			Assert.That(testSubject.Links.Count, Is.EqualTo(1));
			SymbolLink link = testSubject.Links.FirstOrDefault();
			Assert.NotNull(link);
			Assert.That(link.Prior, Is.EqualTo(Symbol.Expectation));
			Assert.That(link.Current, Is.EqualTo(Symbol.Deadline));
		}
	}
}
