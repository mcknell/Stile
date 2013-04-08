#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Compilation.Grammars.CodeMetadata;
#endregion

namespace Stile.Prototypes.Compilation.Grammars
{
	public class Term
	{
		public Term(string token, SymbolCardinality? cardinality = null)
		{
			Token = token;
			Cardinality = cardinality ?? SymbolCardinality.One;
		}

		public SymbolCardinality Cardinality { get; private set; }
		public string Token { get; private set; }
	}
}
