#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
using Stile.Prototypes.Specifications.Grammar.Metadata;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	[TestFixture]
	public class ProductionBuilderFromMethodFixture
	{
		private const int Prior = 8;

		[Test]
		public void GetRuleFromMethodWithNonterminalSymbol()
		{
			Action<int> action = RuleWithNonterminalSymbol;
			AssertRuleMethod(action, false, 1, "Foo");
		}

		[Test]
		public void GetRuleFromMethodWithTerminalSymbol()
		{
			Action<int> action = RuleWithTerminalSymbol;
			string methodName = action.Method.Name;
			string firstParameterName = action.Method.GetParameters()[0].Name;
			string firstToken = string.Format("{0} \"{1}\"", methodName, firstParameterName);
			AssertRuleMethod(action, true, 1, methodName, firstToken);
		}

		private void AssertRuleMethod(Action<int> action,
			bool nameIsSymbol,
			int items,
			string firstToken,
			string firstAlias = null)
		{
			MethodInfo methodBase = action.Method;
			RuleAttribute attribute = methodBase.GetCustomAttributes<RuleAttribute>(false).First();
			Assert.That(attribute.NameIsSymbol, Is.EqualTo(nameIsSymbol), "precondition");

			// act
			ProductionBuilder builder = new ProductionBuilderFromMethod(methodBase, attribute).Build();

			Assert.NotNull(builder);
			Assert.That(builder.Left.Token, Is.EqualTo(Prior.ToString(CultureInfo.InvariantCulture)));
			Assert.NotNull(builder.Right);
			Assert.That(builder.Right.Sequences.Count, Is.EqualTo(1));
			ISequence sequence = builder.Right.Sequences[0];
			Assert.That(sequence.Items.Count, Is.EqualTo(items));
			IItem firstItem = sequence.Items[0];
			Assert.That(firstItem.Cardinality, Is.EqualTo(Cardinality.One));
			Assert.That(firstItem.Primary, Is.InstanceOf<NonterminalSymbol>());
			var nonterminalSymbol = (NonterminalSymbol) firstItem.Primary;
			Assert.That(nonterminalSymbol.Token, Is.EqualTo(firstToken));
			Assert.That(nonterminalSymbol.Alias, Is.EqualTo(firstAlias));

			string parameterName = methodBase.GetParameters()[0].Name;
			string titleCase = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(parameterName);
			if (items > 1)
			{
				IItem secondItem = sequence.Items[1];
				Assert.That(secondItem.Cardinality, Is.EqualTo(Cardinality.One));
				var terminalSymbol = secondItem.Primary as TerminalSymbol;
				Assert.NotNull(terminalSymbol);
				Assert.That(terminalSymbol.Token, Is.EqualTo(titleCase));
				Assert.That(terminalSymbol.Alias,
					Is.EqualTo(StringLiteral.DoubleQuote + parameterName + StringLiteral.DoubleQuote));
			}
		}

		[Rule(Prior)]
		private void RuleWithNonterminalSymbol([NonterminalSymbol] int foo = 4) {}

		[Rule(Prior, NameIsSymbol = true)]
		private void RuleWithTerminalSymbol([Symbol] int foo) {}
	}
}
