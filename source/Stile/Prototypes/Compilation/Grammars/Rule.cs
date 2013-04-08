#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars
{
	public class Rule
	{
		public Rule([NotNull] Symbol left, [NotNull] Term right, params Term[] rights)
		{
			Left = left;
			Rights = rights.Unshift(right).ToArray();
		}

		[NotNull]
		public Symbol Left { get; private set; }
		[NotNull]
		public IReadOnlyCollection<Term> Rights { get; set; }
	}
}
