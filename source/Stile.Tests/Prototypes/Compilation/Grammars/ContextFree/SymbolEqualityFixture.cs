#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.NUnit;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree
{
	[TestFixture]
	public class SymbolEqualityFixture : EqualityFixture<Symbol>
	{
		protected override Symbol GetOther()
		{
			return Nonterminal.Specification;
		}

		protected override Symbol GetTestSubject()
		{
			return Nonterminal.Deadline;
		}
	}
}
