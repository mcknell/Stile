#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Specifications.Grammar.Metadata;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	[TestFixture]
	public class ProductionExtractorFromMethodFixture : ExtractorFixtureBase
	{
		public ProductionExtractorFromMethodFixture() {}

		[Rule(Prior, NonterminalSymbol.IfEnumerable, NonterminalSymbol.IfEnumerable)]
		public ProductionExtractorFromMethodFixture([Symbol(alias: Alias)] int foo) { }

		[Test]
		public void GetProductionFromCtorWithAliasedTerminalSymbol()
		{
			ConstructorInfo constructorInfo = GetType().GetConstructor(new[] {typeof(int)});
			AssertRuleFromMember(constructorInfo, true, new SymbolMetadata("Foo", StringLiteral.QuoteIfNeeded(Alias)));
		}

		[Test]
		public void GetProductionFromMethodWithNonterminalSymbol()
		{
			Action<int, int> action = RuleWithNonterminalSymbol;
			AssertNameIsSymbol(false, action.Method);
			AssertRuleFromMember(action.Method,
				true,
				new SymbolMetadata("Foo", "Foo?"),
				new SymbolMetadata("Bar", "Bar?"));
		}

		[Test]
		public void GetProductionFromMethodWithTerminalSymbol()
		{
			Action<int> action = RuleWithTerminalSymbol;
			AssertNameIsSymbol(true, action.Method);
			string methodName = action.Method.Name;
			AssertRuleFromMember(action.Method,
				true,
				new SymbolMetadata(methodName, string.Format("{0} {1}", methodName, StringLiteral.QuoteIfNeeded(Alias))));
		}

		private static void AssertNameIsSymbol(bool nameIsSymbol, MethodInfo methodBase)
		{
			Assert.That(GetAttribute(methodBase).NameIsSymbol, Is.EqualTo(nameIsSymbol), "precondition");
		}

		private static RuleAttribute GetAttribute(MethodInfo methodBase)
		{
			RuleAttribute attribute = methodBase.GetCustomAttributes<RuleAttribute>(false).First();
			return attribute;
		}

		[Rule(Prior)]
		private void RuleWithNonterminalSymbol([NonterminalSymbol] int foo = 4, [NonterminalSymbol] int bar = 4) {}

		[Rule(Prior, NameIsSymbol = true)]
		private void RuleWithTerminalSymbol([Symbol(alias : Alias)] int foo) {}
	}
}
