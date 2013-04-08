#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

namespace Stile.Prototypes.Compilation.Grammars
{
	public class Term
	{
		public Term(Symbol symbol, Cardinality? cardinality = null)
		{
			Symbol = symbol;
			Cardinality = cardinality ?? Grammars.Cardinality.One;
		}

		public Cardinality Cardinality { get; private set; }
		public Symbol Symbol { get; private set; }
	}
}
