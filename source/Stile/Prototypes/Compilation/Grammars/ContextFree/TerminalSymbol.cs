#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public class TerminalSymbol : Symbol
	{
		public static readonly TerminalSymbol EBNFAlternation = new TerminalSymbol("Or", "|");
		public static readonly TerminalSymbol EBNFAssignment = new TerminalSymbol("::=");
		public static readonly TerminalSymbol UseReflection = new TerminalSymbol(">|");

		public TerminalSymbol([NotNull] string token, string alias = null)
			: base(token, alias ?? token) {}
	}
}
