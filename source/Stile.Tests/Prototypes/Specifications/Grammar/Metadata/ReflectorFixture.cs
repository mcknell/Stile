#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
using Stile.Prototypes.Specifications.Grammar.Metadata;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Grammar.Metadata
{
	[TestFixture]
	public class ReflectorFixture
	{
		private const int Prior = 8;

		[Test]
		public void NoSymbolOrAlias()
		{
			Action<int> action = NoSymbolOrAliasTarget;
			MethodInfo methodBase = action.Method;
			RuleExpansionAttribute attribute = methodBase.GetCustomAttributes<RuleExpansionAttribute>(false).First();

			// act
			List<ILink> links = Reflector.GetLinks(methodBase, attribute).ToList();

			Assert.NotNull(links);
			Assert.That(links, Has.Count.EqualTo(1));
			ILink first = links.First();
			Assert.NotNull(first);
			Assert.NotNull(first.Prior);
			Assert.That(first.Prior.Token, Is.EqualTo(Prior.ToString(CultureInfo.InvariantCulture)));
			Assert.That(first.Symbol.Token, Is.EqualTo(methodBase.Name).IgnoreCase);
			string parameterName = methodBase.GetParameters()[0].Name;
			string titleCase = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(parameterName);
			string expected = string.Format("({0} {1} aka \"{2}\")", methodBase.Name, titleCase, parameterName);
			Assert.That(first.Clause.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void RuleWithNonterminalSymbol()
		{
			Action<int> action = RuleWithNonterminalSymbolTarget;
			MethodInfo methodBase = action.Method;
			RuleAttribute attribute = methodBase.GetCustomAttributes<RuleAttribute>(false).First();

			// act
			IProductionRule rule = Reflector.GetRule(methodBase, attribute);

			Assert.NotNull(rule);
			Assert.That(rule.Left.Token, Is.EqualTo(Prior.ToString(CultureInfo.InvariantCulture)));
			Assert.That(rule.Right.Cardinality, Is.EqualTo(Cardinality.One));
			Assert.That(rule.Right.GetFirstNonterminal().Token, Is.EqualTo(methodBase.Name).IgnoreCase);
			Assert.That(rule.Right.Members.Count, Is.EqualTo(2));
			string parameterName = methodBase.GetParameters()[0].Name;
			string titleCase = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(parameterName);
			string expected = string.Format("{0} aka \"{1}\"", titleCase, parameterName);
			var member = rule.Right.Members.ElementAt(1) as IClause;
			Assert.NotNull(member);
			Assert.That(member.Cardinality, Is.EqualTo(Cardinality.ZeroOrOne));
			Assert.That(member.Members.Single().ToString(), Is.EqualTo(expected));
		}

		[RuleExpansion(Prior)]
		private void NoSymbolOrAliasTarget([Symbol] int foo) {}

		[Rule(Prior, UseMethodNameAsSymbol = true)]
		private void RuleWithNonterminalSymbolTarget([Symbol] int bar = 0) {}
	}
}
