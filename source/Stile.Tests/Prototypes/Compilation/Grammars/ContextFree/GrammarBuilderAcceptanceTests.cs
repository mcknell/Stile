#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Specifications.Grammar.Metadata;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree
{
	[TestFixture]
	public class GrammarBuilderAcceptanceTests
	{
		[Test]
		public void Builds()
		{
			IEnumerable<IProductionRule> rules = new Reflector().FindRules();
			var builder = new GrammarBuilder(rules);

			// act
			IGrammar grammar = builder.Build();

			Assert.NotNull(grammar);
		}

		[Test]
		public void Prints()
		{
			IEnumerable<IProductionRule> rules = new Reflector().FindRules();
			var builder = new GrammarBuilder(rules);

			// act
			string ebnf = builder.ToEBNF();

			Assert.NotNull(ebnf);
		}
	}
}
