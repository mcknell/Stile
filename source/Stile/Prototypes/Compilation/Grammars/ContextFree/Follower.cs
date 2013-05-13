#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Types.Primitives;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public interface IFollower : IEquatable<IFollower>
	{
		[NotNull]
		IClause Current { get; }
		[NotNull]
		IClause Prior { get; }

		[NotNull]
		IFollower Clone([NotNull] Func<Symbol, Symbol> symbolCloner);
	}

	public partial class Follower : IFollower
	{
		public Follower([NotNull] string prior, [NotNull] string current, string alias = null)
			: this(prior, Clause.Make(new Nonterminal(current, alias))) {}

		public Follower([NotNull] string prior, [NotNull] IClause current)
			: this(Clause.Make(new Nonterminal(prior)), current) {}

		public Follower([NotNull] IClause prior, [NotNull] IClause current)
		{
			Prior = prior.ValidateArgumentIsNotNull();
			Current = current.ValidateArgumentIsNotNull();
		}

		public IClause Current { get; private set; }
		public IClause Prior { get; private set; }

		public IFollower Clone(Func<Symbol, Symbol> symbolCloner)
		{
			return new Follower(Prior.Clone(symbolCloner), Current.Clone(symbolCloner));
		}

		public override string ToString()
		{
			return "{{{0}}} {{{1}}}".InvariantFormat(Prior, Current);
		}
	}

	public partial class Follower
	{
		public bool Equals(IFollower other)
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
			var other = obj as IFollower;
			return other != null && Equals(other);
		}

		public override int GetHashCode()
		{
			return Current.GetHashCode() ^ (Prior.GetHashCode() >> 1);
		}

		public static bool operator ==(Follower left, IFollower right)
		{
			return Equals(left, right);
		}

		public static bool operator ==(IFollower left, Follower right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Follower left, IFollower right)
		{
			return !Equals(left, right);
		}

		public static bool operator !=(IFollower left, Follower right)
		{
			return !Equals(left, right);
		}
	}
}
