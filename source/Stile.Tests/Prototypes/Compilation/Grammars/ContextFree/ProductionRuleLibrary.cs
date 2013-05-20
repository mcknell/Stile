#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree
{
	public static class ProductionRuleLibrary
	{
		public static IProductionRule AndLater
		{
			get { return new ProductionRule(Nonterminal.Specification, Nonterminal.Specification, Nonterminal.AndLater); }
		}

		public static IProductionRule ExpectationBefore
		{
			get { return new ProductionRule(Nonterminal.Expectation, Nonterminal.Expectation, Nonterminal.Before); }
		}

		public static IProductionRule ExpectationHas
		{
			get { return new ProductionRule(Nonterminal.Expectation, Nonterminal.Has); }
		}

		public static IProductionRule Inspection
		{
			get { return new ProductionRule(Nonterminal.Inspection, Nonterminal.Instrument, Nonterminal.Action); }
		}

		public static IProductionRule Specification
		{
			get
			{
				Clause clause =
					Clause.Make(new IClause[]
					{
						Clause.Make(Cardinality.ZeroOrOne, Nonterminal.Source),
						Clause.Make(Cardinality.One, Nonterminal.Inspection),
						Clause.Make(Cardinality.ZeroOrOne, Nonterminal.Deadline),
						Clause.Make(Cardinality.ZeroOrOne, Nonterminal.Reason)
					});
				return new ProductionRule(Nonterminal.Specification, clause);
			}
		}
	}
}
