#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public class NonterminalSymbol : Symbol,
		IPrimary
	{
		public const string IfComparable = "\"(if comparable)\"";
		public const string IfEnumerable = "\"(if enumerable)\"";

		protected NonterminalSymbol([NotNull] string token, string alias = null)
			: base(ToTitleCase(token), alias) {}
	}
}
