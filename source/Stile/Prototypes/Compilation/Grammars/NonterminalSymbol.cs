#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Compilation.Grammars
{
	public class NonterminalSymbol : Symbol
	{
		public static readonly Symbol Start = new NonterminalSymbol("S");

		protected NonterminalSymbol([NotNull] string token)
			: base(token) {}
	}
}
