#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public interface IFragment : IEquatable<IFragment>
	{
		Cardinality Cardinality { get; }
		string Left { get; }
		NonterminalSymbol Right { get; }
	}

	public partial class Fragment : IFragment
	{
		public Fragment(string left, NonterminalSymbol right, Cardinality cardinality = Cardinality.One)
		{
			Left = left.ValidateStringNotNullOrEmpty();
			Right = right.ValidateArgumentIsNotNull();
			Cardinality = cardinality;
		}

		public Cardinality Cardinality { get; private set; }

		public string Left { get; private set; }
		public NonterminalSymbol Right { get; private set; }
	}

	public partial class Fragment
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
