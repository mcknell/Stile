#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.NUnit;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree
{
	[TestFixture]
	public class ClauseEqualityFixture : EqualityFixtureWithClone<IClause>
	{
		protected override Func<IClause, IClause> GetCloner()
		{
			return x => x.Clone(y => y);
		}

		protected override IClause GetOther()
		{
			return new Clause(Nonterminal.Specification);
		}

		protected override IClause GetTestSubject()
		{
			return new Clause(Nonterminal.Deadline);
		}
	}
}