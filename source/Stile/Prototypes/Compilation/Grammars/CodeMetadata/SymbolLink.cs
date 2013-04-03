#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.CodeMetadata
{
	public class SymbolLink
	{
		public SymbolLink([NotNull] SymbolNode prior, [NotNull] SymbolNode next)
		{
			Prior = prior;
			Next = next;
		}

		public SymbolNode Next { get; set; }
		public SymbolNode Prior { get; set; }
	}
}
