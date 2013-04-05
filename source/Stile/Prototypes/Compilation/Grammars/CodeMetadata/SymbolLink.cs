#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.CodeMetadata
{
	public class SymbolLink : IEquatable<SymbolLink>
	{
		public SymbolLink([NotNull] Symbol prior, [NotNull] Symbol current)
		{
			Prior = prior.ValidateArgumentIsNotNull();
			Current = current.ValidateArgumentIsNotNull();
		}

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
			// shift in case Current === Prior, so XOR doesn't yield zero (except in rare edge cases)
			return Current.GetHashCode() ^ (Prior.GetHashCode() >> 1);
		}

		public static SymbolLink Make([NotNull] Symbol prior, [NotNull] Symbol next)
		{
			return new SymbolLink(prior, next);
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
