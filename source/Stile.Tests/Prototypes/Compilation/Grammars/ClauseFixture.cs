#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars
{
	[TestFixture]
	public class ClauseFixture
	{
		[Test]
		public void Flatten()
		{
			IClause testSubject = ProductionRuleLibrary.Specification.Right;
			Assert.That(testSubject.Symbols, Is.Empty, "precondition");

			// act
			List<Symbol> symbols = testSubject.Flatten().ToList();

			string joined = string.Join(" ", symbols);
			StringAssert.Contains(Nonterminal.Source.Token, joined);
			StringAssert.Contains(Nonterminal.Inspection.Token, joined);
			StringAssert.Contains(Nonterminal.Deadline.Token, joined);
			StringAssert.Contains(Nonterminal.Reason.Token, joined);
		}

		[Test]
		public void Equal()
		{
			var clause = new Clause(Nonterminal.Specification);
			Assert.That(clause.Equals(clause));
			Assert.That(clause.Equals(new Clause(Nonterminal.Deadline)), Is.False);
		}
	}
}
