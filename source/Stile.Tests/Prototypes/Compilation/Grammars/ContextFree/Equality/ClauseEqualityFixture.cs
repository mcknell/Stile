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
	public class ClauseEqualityFixture : EqualityFixture<IClause>
	{
		protected override IClause OtherFactory()
		{
			return Clause.Make(Nonterminal.Specification);
		}

		protected override IClause TestSubjectFactory()
		{
			return Clause.Make(Nonterminal.Deadline);
		}
	}

	[TestFixture]
	public class SequenceEqualityFixture : EqualityFixture<ISequence>
	{
		protected override ISequence OtherFactory()
		{
			return new Sequence(new Item(Nonterminal.Action));
		}

		protected override ISequence TestSubjectFactory()
		{
			return new Sequence(new Item(Nonterminal.Action),
				new Item(Nonterminal.AndLater),
				new Item(Nonterminal.Deadline));
		}
	}

	[TestFixture]
	public class ItemEqualityFixture : EqualityFixture<IItem>
	{
		protected override IItem OtherFactory()
		{
			return new Item(Nonterminal.Action);
		}

		protected override IItem TestSubjectFactory()
		{
			return new Item(Nonterminal.AndLater);
		}
	}

	[TestFixture]
	public class ChoiceEqualityFixture : EqualityFixture<IChoice>
	{
		protected override IChoice OtherFactory()
		{
			return new Choice(new Sequence(new Item(Nonterminal.Action)));
		}

		protected override IChoice TestSubjectFactory()
		{
			return new Choice(new Sequence(new Item(Nonterminal.AndLater)));
		}
	}
}
