#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree
{
	[TestFixture]
	public class ClauseFixture
	{
		[Test]
		public void Intersects()
		{
			IClause testSubject = ProductionRuleLibrary.Specification.Right;

			// act
			Assert.That(testSubject.Intersects(new HashSet<Symbol> {Nonterminal.Source}));
		}
	}
}
