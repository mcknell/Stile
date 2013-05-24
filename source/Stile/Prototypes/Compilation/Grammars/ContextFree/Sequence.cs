#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stile.Patterns.Behavioral.Validation;
using Stile.Types.Enumerables;
using Stile.Types.Equality;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public interface ISequence : IAcceptGrammarVisitors,
		IEquatable<ISequence>,
		IEnumerable<IItem>
	{
		IReadOnlyList<IItem> Items { get; }
		Symbol FirstSymbol();
	}

	public partial class Sequence : ISequence
	{
		public Sequence(IItem item, params IItem[] items)
			: this(items.Unshift(item)) {}

		public Sequence(IEnumerable<IItem> items)
		{
			items=items.ValidateArgumentIsNotNull();
			Items = items.SelectMany(x=> x.Flatten()).ToArray();
			Count = Items.Sum(x => x.Count);
		}

		public IReadOnlyList<IItem> Items { get; private set; }

		public void Accept(IGrammarVisitor visitor)
		{
			visitor.Visit(this);
		}

		public int Count { get; private set; }

		public TData Accept<TData>(IGrammarVisitor<TData> visitor, TData data)
		{
			return visitor.Visit(this, data);
		}

		public Symbol FirstSymbol()
		{
			IPrimary primary = Items[0].Primary;
			var nonterminal = primary as NonterminalSymbol;
			if (nonterminal != null)
			{
				return nonterminal;
			}
			var terminalSymbol = primary as TerminalSymbol;
			if (terminalSymbol != null)
			{
				return terminalSymbol;
			}
			var choice = (IChoice) primary;
			return choice.Sequences[0].FirstSymbol();
		}

		public override string ToString()
		{
			return string.Join(" ", Items);
		}
	}

	public partial class Sequence // equality
	{
		public IEnumerator<IItem> GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	public partial class Sequence // equality
	{
		public bool Equals(ISequence other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return Items.SequenceEqual(other.Items);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			var other = obj as ISequence;
			return other != null && Equals(other);
		}

		public override int GetHashCode()
		{
			return Items.Aggregate(0, EqualityExtensions.HashForAccumulation);
		}

		public static bool operator ==(Sequence left, ISequence right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Sequence left, ISequence right)
		{
			return !Equals(left, right);
		}
	}
}
