#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Stile.Prototypes.Collections;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree
{
	[TestFixture]
	public class GrammarBuilderFixture
	{
		[Test]
		public void AddsLink()
		{
			var rules = new IProductionRule[0];
			var testSubject = new GrammarBuilder(rules);
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
			Assert.That(link.Prior.Members.First(), Is.EqualTo(has));
			Assert.That(link.Current.Members.First(), Is.EqualTo(hashcode));
		}

		[Test]
		public void BuildsGrammar()
		{
			IProductionRule inspection = ProductionRuleLibrary.Inspection;
			IProductionRule specification = ProductionRuleLibrary.Specification;
			IProductionRule andLater = ProductionRuleLibrary.AndLater;
			IProductionRule expectationHas = ProductionRuleLibrary.ExpectationHas;
			var testSubject = new GrammarBuilder(inspection, specification, andLater, expectationHas);
			testSubject.Add(Nonterminal.Enum.Has.ToString(), Nonterminal.Enum.HashCode.ToString());

			// act
			IGrammar grammar = testSubject.Build(false);

			Assert.NotNull(grammar);
			AssertRuleIsInGrammar(grammar, specification);
			AssertRuleIsInGrammar(grammar, inspection);
			AssertRuleIsInGrammar(grammar, andLater);
			var joinedRule = new ProductionRule(Nonterminal.Expectation,
				Clause.Make(Clause.Make(Nonterminal.Has), Clause.Make(Nonterminal.HashCode)));
			AssertRuleIsInGrammar(grammar, joinedRule);
			Assert.That(grammar.ProductionRules.Count, Is.EqualTo(4));
		}

		[Test]
		public void Consolidate()
		{
			var hashBucket = new HashBucket<Symbol, IClause>();
			var source = Clause.Make(Cardinality.ZeroOrOne, Nonterminal.Source);
			var deadline = Clause.Make(Cardinality.ZeroOrOne, Nonterminal.Deadline);
			var reason = Clause.Make(Cardinality.ZeroOrOne, Nonterminal.Reason);
			Symbol procedure = Nonterminal.Procedure;
			hashBucket.Add(Nonterminal.Specification, Clause.Make(source, procedure, deadline, reason));
			Symbol instrument = Nonterminal.Instrument;
			Symbol expectation = Nonterminal.Expectation;
			hashBucket.Add(Nonterminal.Specification, Clause.Make(source, instrument, expectation, deadline, reason));

			var middle = Clause.Make(Clause.Make(procedure),
				TerminalSymbol.EBNFAlternation,
				Clause.Make(instrument, expectation));
			var right = Clause.Make(source, middle, deadline, reason);
			var expected = new ProductionRule(Nonterminal.Specification, right);

			// act
			IList<IProductionRule> rules = GrammarBuilder.Consolidate(hashBucket);

			Assert.NotNull(rules);
			IProductionRule rule = rules.FirstOrDefault();
			Assert.That(rule, Is.EqualTo(expected));
		}

		[Test]
		public void ConsolidateTrivial()
		{
			AssertTrivialConsolidation(ProductionRuleLibrary.ExpectationHas, 1);
			AssertTrivialConsolidation(ProductionRuleLibrary.ExpectationBefore, 2);
		}

		[Test]
		public void ConstructsFromRules()
		{
			IProductionRule expectation = ProductionRuleLibrary.ExpectationBefore;

			// act
			var testSubject = new GrammarBuilder(expectation);

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
		[Explicit("WIP")]
		public void TopologicallySortsProductionRules()
		{
			Assert.Fail("wip");
		}

		private static void AssertRuleIsInGrammar(IGrammar grammar, IProductionRule rule)
		{
			Assert.That(grammar.Nonterminals.Any(x => x.Token == rule.Left));
			foreach (Symbol right in Flatten(rule.Right))
			{
				Assert.That(grammar.Nonterminals, Has.Member(right), right);
			}
			Assert.That(grammar.ProductionRules, Has.Member(rule));
		}

		private static void AssertTrivialConsolidation(IProductionRule productionRule, int expected)
		{
			IClause single = productionRule.Right;
			Assert.That(single.Members.Count, Is.EqualTo(expected), "prec");

			// act
			IList<IProductionRule> rules =
				GrammarBuilder.Consolidate(new HashBucket<Symbol, IClause> {{productionRule.Left, single}});

			Assert.NotNull(rules);
			IProductionRule rule = rules.FirstOrDefault();
			Assert.That(rule, Is.EqualTo(productionRule));
		}

		private static IEnumerable<Symbol> Flatten(IClause clause)
		{
			if (clause == null)
			{
				yield break;
			}
			foreach (IClauseMember member in clause.Members)
			{
				var symbol1 = member as Symbol;
				if (symbol1 != null)
				{
					yield return symbol1;
				}
				else
				{
					foreach (Symbol symbol in Flatten(member as Clause))
					{
						yield return symbol;
					}
				}
			}
		}
	}
}
