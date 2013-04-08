#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars
{
	public static class ProductionRuleLibrary
	{
		public static ProductionRule AndLater
		{
			get { return new ProductionRule(Nonterminal.Specification, Nonterminal.Specification, Nonterminal.AndLater); }
		}

		public static ProductionRule Expectation
		{
			get { return new ProductionRule(Nonterminal.Expectation, Nonterminal.Expectation, Nonterminal.Before); }
		}

		public static ProductionRule Inspection
		{
			get { return new ProductionRule(Nonterminal.Inspection, Nonterminal.Instrument, Nonterminal.Action); }
		}

		public static ProductionRule Specification
		{
			get
			{
				return new ProductionRule(Nonterminal.Specification,
					Nonterminal.Source,
					Nonterminal.Inspection,
					Nonterminal.Expectation,
					Nonterminal.Deadline);
			}
		}
	}
}
