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
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
using Stile.Types.Enumerables;
using Stile.Types.Equality;
using Stile.Types.Primitives;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public interface IChoice : IPrimary,
		IEquatable<IChoice>,
		IEnumerable<ISequence>
	{
		IReadOnlyList<ISequence> Sequences { get; }

		IEnumerable<IFragment> Fragments();
	}

	public partial class Choice : IChoice
	{
		public Choice(ISequence sequence, params ISequence[] sequences)
			: this(sequences.Unshift(sequence)) {}

		public Choice(IEnumerable<ISequence> sequences)
		{
			sequences = sequences.ValidateArgumentIsNotNullOrEmpty();
			Sequences = sequences.OrderBy(x => x.FirstSymbol().Token).ToArray();
			Count = Sequences.Sum(x => x.Count);
		}

		public int Count { get; private set; }

		public IReadOnlyList<ISequence> Sequences { get; private set; }

		public void Accept(IGrammarVisitor visitor)
		{
			visitor.Visit(this);
		}

		public TData Accept<TData>(IGrammarVisitor<TData> visitor, TData data)
		{
			return visitor.Visit(this, data);
		}

		public IEnumerable<IFragment> Fragments()
		{
			foreach (ISequence sequence in Sequences)
			{
				IItem first = sequence.Items.First();
				Symbol prior = first.PrimaryAsSymbol();
				if (prior == null)
				{
					foreach (IFragment fragment in first.Fragments())
					{
						yield return fragment;
					}
				}
				else
				{
					foreach (IItem item in sequence.Skip(1))
					{
						var symbol = item.Primary as NonterminalSymbol;
						if (symbol == null)
						{
							foreach (IFragment fragment in item.Fragments())
							{
								yield return fragment;
							}
						}
						else
						{
							yield return new Fragment(prior.Token, symbol, item.Cardinality);
						}
					}
				}
			}
		}

		public override string ToString()
		{
			string s = string.Join(" | ", Sequences);
			return Sequences.Count > 1 ? "({0})".InvariantFormat(s) : s;
		}
	}

	public partial class Choice // enumerable
	{
		public IEnumerator<ISequence> GetEnumerator()
		{
			return Sequences.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	public partial class Choice // equality
	{
		public bool Equals(IChoice other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return Sequences.SequenceEqual(other.Sequences);
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
			var other = obj as Choice;
			return other != null && Equals(other);
		}

		public override int GetHashCode()
		{
			return Sequences.Aggregate(0, EqualityExtensions.HashForAccumulation);
		}

		public static bool operator ==(Choice left, IChoice right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Choice left, IChoice right)
		{
			return !Equals(left, right);
		}
	}
}
