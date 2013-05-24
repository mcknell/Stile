#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.NUnit;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree.Equality
{
	[TestFixture]
	public class ChoiceEqualityFixture : EqualityFixture<IChoice>
	{
		protected override IChoice OtherFactory()
		{
			return new Choice(new Sequence(new Item(Nonterminal.Action)), new Sequence(new Item(Nonterminal.Deadline)));
		}

		protected override IChoice TestSubjectFactory()
		{
			return new Choice(new Sequence(new Item(Nonterminal.Before)), new Sequence(new Item(Nonterminal.AndLater)));
		}
	}
}
