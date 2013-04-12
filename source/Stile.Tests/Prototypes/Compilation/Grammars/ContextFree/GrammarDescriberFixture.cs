#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree
{
	[TestFixture]
	public class GrammarDescriberFixture
	{
		[Test]
		public void Describes()
		{
			IProductionRule inspection = ProductionRuleLibrary.Inspection;
			IProductionRule specification = ProductionRuleLibrary.Specification;
			IProductionRule andLater = ProductionRuleLibrary.AndLater;
			IGrammar grammar = new GrammarBuilder(inspection, specification, andLater).Build();

			string ebnf = GrammarDescriber.Describe(grammar);

			StringAssert.Contains("Inspection ::= (Instrument Action)", ebnf);
			StringAssert.Contains(
				"Specification ::= ((Source? Inspection Deadline? Reason?) | (Specification AndLater))", ebnf);
		}
	}
}