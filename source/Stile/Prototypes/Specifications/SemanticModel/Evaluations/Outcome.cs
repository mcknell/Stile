#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Stile.Types.Comparison;
using Stile.Types.Enumerables;
using Stile.Types.Enums;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Evaluations
{
	public struct Outcome : IEquatable<Outcome>,
		IComparable<Outcome>
	{
		[Flags]
		private enum Outcomes : byte
		{
			Failed = 0,
			Succeeded = 1,
			Incomplete = 2,
			Interrupted = 4,
			TimedOut = 8,
			Suspended = 16
		}

#pragma warning disable 649
		public static readonly Outcome Failed;
		public static readonly Outcome Incomplete;
		public static readonly Outcome Interrupted;
		public static readonly Outcome Suspended;
		public static readonly Outcome Succeeded;
		public static readonly Outcome TimedOut;
#pragma warning restore 649
		private static readonly ReadOnlyCollection<Outcome> sValues;
		private static readonly IEqualityComparer<Outcome> ValueComparerInstance = new ValueEqualityComparer();
		private readonly Outcomes _value;

		static Outcome()
		{
			var outcomes = new List<Outcome>();

			Make(() => Failed, outcomes, Outcomes.Failed);
			Make(() => Succeeded, outcomes, Outcomes.Succeeded);
			Make(() => Incomplete, outcomes, Outcomes.Incomplete);
			Make(() => Interrupted, outcomes, Outcomes.Interrupted);
			Make(() => TimedOut, outcomes, Outcomes.Incomplete | Outcomes.TimedOut);
			Make(() => Suspended, outcomes, Outcomes.Suspended);

			sValues = outcomes.ToReadOnly();
		}

		private Outcome(Outcomes enumerated)
		{
			_value = enumerated;
		}

		public static IEqualityComparer<Outcome> ValueComparer
		{
			get { return ValueComparerInstance; }
		}
		public static ReadOnlyCollection<Outcome> Values
		{
			get { return sValues; }
		}

		public int CompareTo(Outcome other)
		{
			return other._value.CompareTo(_value);
		}

		public bool Equals(Outcome other)
		{
			return _value == other._value;
		}

		[Pure]
		public bool Covers(Outcome other)
		{
			return (_value & other._value) == other._value;
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
			return (int) _value;
		}

		[Pure]
		public bool IsCoveredBy(Outcome other)
		{
			return other.Covers(this);
		}

		public override string ToString()
		{
			return _value.ToString();
		}

		private static void Make(Expression<Func<Outcome>> field, List<Outcome> outcomes, Outcomes enumerated)
		{
			var memberExpression = (MemberExpression) field.Body;
			var fieldInfo = (FieldInfo) memberExpression.Member;
			Outcome outcome = MakeNext(outcomes, enumerated);
			fieldInfo.SetValue(null,
				outcome,
				BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Public,
				null,
				CultureInfo.InvariantCulture);
		}

		private static Outcome MakeNext(List<Outcome> outcomes, Outcomes enumerated)
		{
			var outcome = new Outcome(enumerated);
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

		public static bool operator >(Outcome left, Outcome right)
		{
			return left.IsGreaterThan(right);
		}

		public static bool operator >=(Outcome left, Outcome right)
		{
			return left.IsGreaterThanOrEqualTo(right);
		}

		public static implicit operator bool(Outcome outcome)
		{
			return outcome.Equals(Succeeded);
		}

		public static implicit operator Outcome(TaskStatus taskStatus)
		{
			switch (taskStatus)
			{
				case TaskStatus.RanToCompletion:
					return Succeeded;
				case TaskStatus.Canceled:
				case TaskStatus.Created:
				case TaskStatus.Running:
					return Incomplete;
				case TaskStatus.Faulted:
					return Interrupted;
				case TaskStatus.WaitingForActivation:
				case TaskStatus.WaitingForChildrenToComplete:
				case TaskStatus.WaitingToRun:
					return Suspended;
			}
			throw Enumeration.FailedToRecognize(() => taskStatus);
		}

		public static bool operator !=(Outcome left, Outcome right)
		{
			return !left.Equals(right);
		}

		public static bool operator <(Outcome left, Outcome right)
		{
			return left.IsLessThan(right);
		}

		public static bool operator <=(Outcome left, Outcome right)
		{
			return left.IsLessThanOrEqualTo(right);
		}

		private sealed class ValueEqualityComparer : IEqualityComparer<Outcome>
		{
			public bool Equals(Outcome x, Outcome y)
			{
				return x._value == y._value;
			}

			public int GetHashCode(Outcome obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}
