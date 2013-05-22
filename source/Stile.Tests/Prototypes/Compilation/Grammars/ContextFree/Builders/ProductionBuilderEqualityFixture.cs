#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.NUnit;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	[TestFixture]
	public class ProductionBuilderEqualityFixture : EqualityFixture<IProductionBuilder>
	{
		protected override IProductionBuilder OtherFactory()
		{
			return new ProductionBuilder(Nonterminal.Expectation, null);
		}

		protected override IProductionBuilder TestSubjectFactory()
		{
			return new ProductionBuilder(Nonterminal.HashCode, new Choice(new Sequence(new Item(Nonterminal.Before))));
		}
	}
}
