#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Readability;
using Stile.Types.Enums;
using Stile.Types.Equality;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public interface ILink
	{
		Cardinality Cardinality { get; }
		IClause Clause { get; }
		[CanBeNull]
		Symbol Prior { get; }
		Symbol Symbol { get; }
	}

	public partial class Link : ILink
	{
		public Link([CanBeNull] Symbol prior,
			Symbol symbol,
			Cardinality cardinality = Cardinality.One,
			IClause clause = null)
		{
			Prior = prior;
			Symbol = symbol.ValidateArgumentIsNotNull();
			Cardinality = cardinality;
			Clause = clause ?? ContextFree.Clause.Make(Cardinality, symbol);
		}

		public Cardinality Cardinality { get; private set; }
		public IClause Clause { get; private set; }
		public Symbol Prior { get; private set; }
		public Symbol Symbol { get; private set; }

		public override string ToString()
		{
			string s;
			switch (Cardinality)
			{
				case Cardinality.One:
					s = string.Empty;
					break;
				case Cardinality.OneOrMore:
					s = "+";
					break;
				case Cardinality.ZeroOrMore:
					s = "*";
					break;
				case Cardinality.ZeroOrOne:
					s = "?";
					break;
				default:
					throw Enumeration.FailedToRecognize(() => Cardinality);
			}
			return string.Format("{0} {1}{2}", Prior.ToDebugString(), Symbol, s);
		}
	}

	public partial class Link : IEquatable<Link>
	{
		public bool Equals(Link other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return Cardinality == other.Cardinality && Symbol.Equals(other.Symbol)
				&& Prior.EqualsOrIsEquallyNull(other.Prior);
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
			var other = obj as Link;
			return other != null && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (int) Cardinality;
				if (Prior != null)
				{
					hashCode = (hashCode * 397) ^ Prior.GetHashCode();
				}
				hashCode = (hashCode * 397) ^ Symbol.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(Link left, Link right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Link left, Link right)
		{
			return !Equals(left, right);
		}
	}
}
