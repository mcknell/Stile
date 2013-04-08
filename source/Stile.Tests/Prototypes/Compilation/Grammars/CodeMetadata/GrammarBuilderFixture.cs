#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Linq;
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Compilation.Grammars.CodeMetadata;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.CodeMetadata
{
	[TestFixture]
	public class GrammarBuilderFixture
	{
		[Test]
		public void AddsLink()
		{
			var rules = new ProductionRule[0];
			var testSubject = new ContextFreeGrammarBuilder(rules);
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
		public void BuildsGrammar()
		{
			ProductionRule inspection = Inspection();
			ProductionRule specification = Specification();
			ProductionRule andLater = AndLater();
			var testSubject = new ContextFreeGrammarBuilder(inspection, specification, andLater);

			// act
			ContextFreeGrammar grammar = testSubject.Build();

			Assert.NotNull(grammar);
			AssertRuleIsInGrammar(grammar, specification);
			AssertRuleIsInGrammar(grammar, inspection);
			AssertRuleIsInGrammar(grammar, andLater);
			Assert.That(grammar.ProductionRules, Has.Count.EqualTo(3));
		}

		private static void AssertRuleIsInGrammar(ContextFreeGrammar grammar, ProductionRule rule)
		{
			Assert.That(grammar.Nonterminals.Any(x => x.Token == rule.Left));
			foreach (string right in rule.Right)
			{
				Assert.That(grammar.Nonterminals.Any(x => x.Token == right), right);
			}
			Assert.That(grammar.ProductionRules, Has.Member(rule));
		}

		[Test]
		public void ConstructsFromRules()
		{
			ProductionRule expectation = Expectation();

			// act
			var testSubject = new ContextFreeGrammarBuilder(expectation);

			Assert.That(testSubject.Symbols, Has.Member(NonterminalSymbol.Expectation));
			Assert.That(testSubject.Symbols, Has.Member(NonterminalSymbol.Before));
			Assert.That(testSubject.Links.Count, Is.EqualTo(1));
			SymbolLink link = testSubject.Links.FirstOrDefault();
			Assert.NotNull(link);
			Assert.That(link.Prior, Is.EqualTo(NonterminalSymbol.Expectation));
			Assert.That(link.Current, Is.EqualTo(NonterminalSymbol.Before));
		}

		[Test]
		[Explicit("WIP")]
		public void DetectsLinkCycle()
		{
			Assert.Fail("wip");
		}

		[Test]
		[Explicit("WIP")]
		public void EmitsEBNF()
		{
			Assert.Fail("wip");
		}

		[Test]
		[Explicit("WIP")]
		public void TopologicallySortsProductionRules()
		{
			Assert.Fail("wip");
		}

		private static ProductionRule AndLater()
		{
			return new ProductionRule(Nonterminal.Specification, Nonterminal.Specification, Nonterminal.AndLater);
		}

		private static ProductionRule Expectation()
		{
			return new ProductionRule(Nonterminal.Expectation, Nonterminal.Expectation, Nonterminal.Before);
		}

		private static ProductionRule Inspection()
		{
			return new ProductionRule(Nonterminal.Inspection, Nonterminal.Instrument, Nonterminal.Action);
		}

		private static ProductionRule Specification()
		{
			return new ProductionRule(Nonterminal.Specification,
				Nonterminal.Source,
				Nonterminal.Inspection,
				Nonterminal.Expectation,
				Nonterminal.Deadline);
		}
	}
}
