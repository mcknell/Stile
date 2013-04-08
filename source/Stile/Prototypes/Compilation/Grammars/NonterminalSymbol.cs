#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Prototypes.Compilation.Grammars
{
	public class NonterminalSymbol : Symbol
	{
		public NonterminalSymbol([NotNull] string token)
			: base(token) {}

		public static readonly Symbol Action = new Symbol(Nonterminal.Action.ToString());
		public static readonly Symbol Before = new Symbol(Nonterminal.Before.ToString());
		public static readonly Symbol Deadline = new Symbol(Nonterminal.Deadline.ToString());
		public static readonly Symbol Expectation = new Symbol(Nonterminal.Expectation.ToString());
		public static readonly Symbol Inspection = new Symbol(Nonterminal.Inspection.ToString());
		public static readonly Symbol Instrument = new Symbol(Nonterminal.Instrument.ToString());
		public static readonly Symbol Source = new Symbol(Nonterminal.Source.ToString());
		public static readonly Symbol Specification = new Symbol(Nonterminal.Specification.ToString());
		public static readonly Symbol Start = new Symbol(Nonterminal.Start.ToString());
	}
}
