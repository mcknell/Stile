#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.CodeMetadata
{
	public class SymbolNode
	{
		public SymbolNode([NotNull] string symbol, [NotNull] Type type, params SymbolNode[] children)
		{
			Symbol = symbol.Validate().EnumerableOf<char>().IsNotNullOrEmpty();
			Type = type.ValidateArgumentIsNotNull();
			Children = children.ToReadOnly();
		}

		[NotNull]
		public IReadOnlyCollection<SymbolNode> Children { get; private set; }

		public string Symbol { get; private set; }

		public Type Type { get; private set; }
	}
}
