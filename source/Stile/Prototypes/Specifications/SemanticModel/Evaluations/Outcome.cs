#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Evaluations
{
	public struct Outcome : IEquatable<Outcome>,
		IComparable<Outcome>
	{
		public static readonly Outcome Unclear;
		public static readonly Outcome Succeeded;
		public static readonly Outcome Failed;
		public static readonly Outcome Incomplete;
		public static readonly Outcome Interrupted;
		public static readonly ReadOnlyCollection<Outcome> Values;
		private static readonly IEqualityComparer<Outcome> ValueComparerInstance = new ValueEqualityComparer();
		private readonly int _value;

		static Outcome()
		{
			var outcomes = new List<Outcome>();

			Unclear = MakeNext(outcomes);
			Succeeded = MakeNext(outcomes);
			Failed = MakeNext(outcomes);
			Incomplete = MakeNext(outcomes);
			Interrupted = MakeNext(outcomes);

			Values = outcomes.ToReadOnly();
		}

		public Outcome(int value)
		{
			_value = value;
		}

		public static IEqualityComparer<Outcome> ValueComparer
		{
			get { return ValueComparerInstance; }
		}

		public int CompareTo(Outcome other)
		{
			return other._value.CompareTo(_value);
		}

		public bool Equals(Outcome other)
		{
			return _value == other._value;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			return obj is Outcome && Equals((Outcome) obj);
		}

		public override int GetHashCode()
		{
			return _value;
		}

		private static Outcome MakeNext(List<Outcome> outcomes)
		{
			var outcome = new Outcome(outcomes.Count);
			outcomes.Add(outcome);
			return outcome;
		}

		public static bool operator ==(Outcome left, Outcome right)
		{
			return left.Equals(right);
		}

		public static explicit operator Outcome(bool b)
		{
			return b ? Succeeded : Failed;
		}

		public static implicit operator bool(Outcome outcome)
		{
			return outcome.Equals(Succeeded);
		}

		public static bool operator !=(Outcome left, Outcome right)
		{
			return !left.Equals(right);
		}

		private sealed class ValueEqualityComparer : IEqualityComparer<Outcome>
		{
			public bool Equals(Outcome x, Outcome y)
			{
				return x._value == y._value;
			}

			public int GetHashCode(Outcome obj)
			{
				return obj._value;
			}
		}
	}
}
