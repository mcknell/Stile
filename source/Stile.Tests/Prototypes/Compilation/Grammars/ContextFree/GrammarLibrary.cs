#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Linq;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree
{
	public static class GrammarLibrary
	{
		public static IGrammar Simplest
		{
			get
			{
				return new Grammar(Nonterminal.All.Cast<NonterminalSymbol>().ToHashSet(),
					new HashSet<TerminalSymbol> {TerminalSymbol.EBNFAlternation, TerminalSymbol.EBNFAssignment},
					new[] {ProductionRuleLibrary.Specification},
					Nonterminal.Specification);
			}
		}
	}
}
