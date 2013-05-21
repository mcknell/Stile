#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public interface IFragment
	{
		Token Left { get; }
		Token Right { get; }
		ISequence Sequence { get; }
	}

	public class Fragment : IFragment
	{
		public Fragment(string left, string right, string alias)
			: this(left, right, new Sequence(new Item(new Nonterminal(right, alias)))) {}

		public Fragment(string left, string right, ISequence sequence)
		{
			Left = Token.For(left);
			Right = Token.For(right);
			Sequence = sequence.ValidateArgumentIsNotNull();
			if (Sequence.Items.None())
			{
				throw new ArgumentException(ErrorMessages.Fragment_SequenceMustHaveItems, "sequence");
			}
			if (Sequence.Items.Select(x => x.Primary).OfType<NonterminalSymbol>().SingleOrDefault() == null)
			{
				throw new ArgumentException(ErrorMessages.Fragment_SequenceMustHaveExactlyOneNonterminal, "sequence");
			}
		}

		public Token Left { get; private set; }
		public Token Right { get; private set; }
		public ISequence Sequence { get; private set; }
	}
}
