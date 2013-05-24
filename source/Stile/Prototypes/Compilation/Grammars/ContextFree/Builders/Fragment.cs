#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using Stile.Patterns.Behavioral.Validation;
using Stile.Types.Comparison;
using Stile.Types.Primitives;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public interface IFragment : IEquatable<IFragment>,
		IComparable<IFragment>
	{
		Cardinality Cardinality { get; }
		string Left { get; }
		NonterminalSymbol Right { get; }

		IItem RightAsItem();
	}

	public partial class Fragment : IFragment
	{
		public Fragment(string left, NonterminalSymbol right, Cardinality cardinality = Cardinality.One)
		{
			left = left.ValidateNotNullOrEmpty();
			Left = Symbol.ToTitleCase(left);
			Right = right.ValidateArgumentIsNotNull();
			if (Right.Token == Left)
			{
				throw new ArgumentException("Left cannot equal the token on the right.");
			}
			Cardinality = cardinality;
		}

		public Cardinality Cardinality { get; private set; }

		public string Left { get; private set; }
		public NonterminalSymbol Right { get; private set; }

		public IItem RightAsItem()
		{
			return new Item(Right, Cardinality);
		}

		public override string ToString()
		{
			return "{0}<-{1}{2}".InvariantFormat(Left, Right, Cardinality.ToEbnfString());
		}
	}

	public partial class Fragment // comparison
	{
		private static readonly IEqualityComparer<IFragment> _equalityComparer =
			EqualityComparerHelper.MakeEqualityComparer<IFragment>(EqualityCompare);
		private static readonly IComparer<IFragment> _comparer = ComparerHelper.MakeComparer<IFragment>(Compare);

		public static IComparer<IFragment> Comparer
		{
			get { return _comparer; }
		}
		public static IEqualityComparer<IFragment> EqualityComparer
		{
			get { return _equalityComparer; }
		}

		public int CompareTo(IFragment other)
		{
			if (other.Left == Right.Token)
			{
				return -1;
			}
			if (Left == other.Right.Token)
			{
				return 1;
			}
			return 0;
		}

		private static int Compare(IFragment left, IFragment right)
		{
			return left.CompareTo(right);
		}

		private static bool EqualityCompare(IFragment left, IFragment right)
		{
			if (left == null)
			{
				return right == null;
			}
			return left.Equals(right);
		}

		public static bool operator >(Fragment left, Fragment right)
		{
			return left.CompareTo(right) > 0;
		}

		public static bool operator >=(Fragment left, Fragment right)
		{
			return left.CompareTo(right) >= 0;
		}

		public static bool operator <(Fragment left, Fragment right)
		{
			return left.CompareTo(right) < 0;
		}

		public static bool operator <=(Fragment left, Fragment right)
		{
			return left.CompareTo(right) <= 0;
		}
	}

	public partial class Fragment // equality
	{
		public bool Equals(IFragment other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return Cardinality == other.Cardinality && string.Equals(Left, other.Left) && Right.Equals(other.Right);
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
			var other = obj as IFragment;
			return other != null && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (int) Cardinality;
				hashCode = (hashCode * 397) ^ Left.GetHashCode();
				hashCode = (hashCode * 397) ^ Right.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(Fragment left, Fragment right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Fragment left, Fragment right)
		{
			return !Equals(left, right);
		}
	}
}
