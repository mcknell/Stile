#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Linq;
using Stile.Patterns.Behavioral.Validation;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public interface IChoice : IPrimary
	{
		IReadOnlyList<ISequence> Sequences { get; }
	}

	public class Choice : IChoice
	{
		public Choice(ISequence sequence, params ISequence[] sequences)
			: this(sequences.Unshift(sequence)) {}

		public Choice(IEnumerable<ISequence> sequences)
		{
			sequences = sequences.Validate().EnumerableOf<ISequence>().IsNotNullOrEmpty();
			Sequences = sequences.ToArray();
		}

		public IReadOnlyList<ISequence> Sequences { get; private set; }

		public void Accept(IGrammarVisitor visitor)
		{
			visitor.Visit(this);
		}

		public TData Accept<TData>(IGrammarVisitor<TData> visitor, TData data)
		{
			return visitor.Visit(this, data);
		}

		public override string ToString()
		{
			string s = string.Join(" | ", Sequences);
			return Sequences.Count > 1 ? string.Format("({0})", s) : s;
		}
	}
}
