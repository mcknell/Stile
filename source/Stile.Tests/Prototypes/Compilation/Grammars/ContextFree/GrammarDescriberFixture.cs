#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree
{
	[TestFixture]
	public class GrammarDescriberFixture
	{
		[Test]
		[Ignore("wip")]
		public void Describes()
		{
			IProductionRule inspection = ProductionRuleLibrary.Inspection;
			IProductionRule specification = ProductionRuleLibrary.Specification;
			IProductionRule andLater = ProductionRuleLibrary.AndLater;
			var grammarBuilder = new GrammarBuilder();
			grammarBuilder.Add(inspection, specification, andLater);
			IGrammar grammar = grammarBuilder.Build();

			string ebnf = GrammarDescriber.Describe(grammar);

			StringAssert.Contains("Inspection ::= (Instrument Action)", ebnf);
			StringAssert.Contains(
				"Specification ::= ((Source? Inspection Deadline? Reason?) | (Specification AndLater))", ebnf);
		}
	}
}
