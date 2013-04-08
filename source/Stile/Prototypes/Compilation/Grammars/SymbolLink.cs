#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Compilation.Grammars
{
	public class SymbolLink : IEquatable<SymbolLink>
	{
		public SymbolLink([NotNull] Symbol prior, [NotNull] Symbol current, Cardinality? cardinality = null)
		{
			Prior = prior.ValidateArgumentIsNotNull();
			Current = current.ValidateArgumentIsNotNull();
			Cardinality = cardinality ?? Grammars.Cardinality.One;
		}

		public Cardinality Cardinality { get; private set; }

		[NotNull]
		public Symbol Current { get; private set; }
		[NotNull]
		public Symbol Prior { get; private set; }

		public bool Equals(SymbolLink other)
		{
			if (ReferenceEquals(other, null))
			{
				return false;
			}
			return Current.Equals(other.Current) && Prior.Equals(other.Prior);
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
			var other = obj as SymbolLink;
			return other != null && Equals(other);
		}

		public override int GetHashCode()
		{
			return Current.GetHashCode() ^ (Prior.GetHashCode() >> 1);
		}

		public static SymbolLink Make([NotNull] Symbol prior,
			[NotNull] Symbol next,
			Cardinality? cardinality = null)
		{
			return new SymbolLink(prior, next, cardinality);
		}

		public static bool operator ==(SymbolLink left, SymbolLink right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(SymbolLink left, SymbolLink right)
		{
			return !Equals(left, right);
		}
	}
}
