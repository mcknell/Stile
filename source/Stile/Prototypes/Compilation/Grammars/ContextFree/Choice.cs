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
	public interface IChoice : IPrimary,
		IEquatable<IChoice>,
		IEnumerable<ISequence>
	{
		IReadOnlyList<ISequence> Sequences { get; }
	}

	public partial class Choice : IChoice
	{
		public Choice(ISequence sequence, params ISequence[] sequences)
			: this(sequences.Unshift(sequence)) {}

		public Choice(IEnumerable<ISequence> sequences)
		{
			sequences = sequences.ValidateArgumentIsNotNullOrEmpty();
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
