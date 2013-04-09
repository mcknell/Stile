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
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree
{
	[TestFixture]
	public class ContextFreeGrammarBuilderFixture
	{
		[Test]
		public void AddsLink()
		{
			var rules = new IProductionRule[0];
			var testSubject = new ContextFreeGrammarBuilder(rules);
			Assert.That(testSubject.Links, Is.Empty, "precondition");
			Assert.That(testSubject.Symbols, Is.Empty, "precondition");
			Symbol has = Nonterminal.Has;
			Symbol hashcode = Nonterminal.HashCode;

			// act
			testSubject.Add(has, hashcode);

			Assert.That(testSubject.Symbols.Any(x => x.Token == has));
			Assert.That(testSubject.Symbols.Any(x => x.Token == hashcode));
			Assert.That(testSubject.Symbols.Count, Is.EqualTo(2));

			Assert.That(testSubject.Links.Count, Is.EqualTo(1));
			IFollower link = testSubject.Links.FirstOrDefault();
			Assert.NotNull(link);
			Assert.That(link.Prior.Symbols.First(), Is.EqualTo(has));
			Assert.That(link.Current.Symbols.First(), Is.EqualTo(hashcode));
		}

		[Test]
		public void BuildsGrammar()
		{
			IProductionRule inspection = ProductionRuleLibrary.Inspection;
			IProductionRule specification = ProductionRuleLibrary.Specification;
			IProductionRule andLater = ProductionRuleLibrary.AndLater;
			IProductionRule expectationHas = ProductionRuleLibrary.ExpectationHas;
			var testSubject = new ContextFreeGrammarBuilder(inspection, specification, andLater, expectationHas);
			testSubject.Add(Nonterminal.Enum.Has.ToString(), Nonterminal.Enum.HashCode.ToString());

			// act
			ContextFreeGrammar grammar = testSubject.Build();

			Assert.NotNull(grammar);
			AssertRuleIsInGrammar(grammar, specification);
			AssertRuleIsInGrammar(grammar, inspection);
			AssertRuleIsInGrammar(grammar, andLater);
			var joinedRule = new ProductionRule(Nonterminal.Expectation,
				new Clause(new Clause(Nonterminal.Has), new Clause(Nonterminal.HashCode)));
			AssertRuleIsInGrammar(grammar, joinedRule);
			Assert.That(grammar.ProductionRules, Has.Count.EqualTo(4));
		}

		[Test]
		public void ConstructsFromRules()
		{
			IProductionRule expectation = ProductionRuleLibrary.ExpectationBefore;

			// act
			var testSubject = new ContextFreeGrammarBuilder(expectation);

			Assert.That(testSubject.Symbols, Has.Member(Nonterminal.Expectation));
			Assert.That(testSubject.Symbols, Has.Member(Nonterminal.Before));
			Assert.That(testSubject.Links.Count, Is.EqualTo(0));
		}

		[Test]
		[Explicit("WIP")]
		public void DetectsLinkCycle()
		{
			Assert.Fail("wip");
		}

		[Test]
		public void EmitsEBNF()
		{
			IProductionRule inspection = ProductionRuleLibrary.Inspection;
			IProductionRule specification = ProductionRuleLibrary.Specification;
			IProductionRule andLater = ProductionRuleLibrary.AndLater;
			var testSubject = new ContextFreeGrammarBuilder(inspection, specification, andLater);

			string ebnf = testSubject.ToEBNF();

			StringAssert.Contains("Inspection ::= (Instrument Action)", ebnf);
			StringAssert.Contains("Specification ::= (Source? Inspection Deadline? Reason? | (Specification AndLater))", ebnf);
		}

		[Test]
		[Explicit("WIP")]
		public void TopologicallySortsProductionRules()
		{
			Assert.Fail("wip");
		}

		private static void AssertRuleIsInGrammar(ContextFreeGrammar grammar, IProductionRule rule)
		{
			Assert.That(grammar.Nonterminals.Any(x => x.Token == rule.Left));
			foreach (Symbol right in rule.Right.Flatten())
			{
				Assert.That(grammar.Nonterminals, Has.Member(right), right);
			}
			Assert.That(grammar.ProductionRules, Has.Member(rule));
		}
	}
}
