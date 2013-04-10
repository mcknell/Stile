#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars
{
	public interface IClauseMember
	{
		[NotNull]
		IEnumerable<Symbol> Flatten();
	}

	public interface IClause : IClauseMember,
		IEquatable<IClause>
	{
		Cardinality Cardinality { get; }
		[NotNull]
		IReadOnlyList<IClauseMember> Members { get; }

		[NotNull]
		IClause Clone([NotNull] Func<Symbol, Symbol> symbolCloner);

		[NotNull]
		IClause Prune();
	}

	public partial class Clause : IClause
	{
		public delegate IClauseMember ClauseMemberCloner(IClauseMember member, Func<Symbol, Symbol> symbolCloner);

		private readonly ClauseMemberCloner _cloner;

		public Clause(Cardinality cardinality, [NotNull] IClauseMember member, params IClauseMember[] members)
			: this(members.Unshift(member.ValidateArgumentIsNotNull()), cardinality) {}

		public Clause([NotNull] IClauseMember member, params IClauseMember[] members)
			: this(members.Unshift(member.ValidateArgumentIsNotNull())) {}

		public Clause([NotNull] IEnumerable<IClauseMember> members,
			Cardinality? cardinality = null,
			ClauseMemberCloner cloner = null)
		{
			Members = members.ValidateArgumentIsNotNull().ToArray();
			Cardinality = cardinality ?? Cardinality.One;
			_cloner = cloner ?? DefaultCloner;
		}

		public Cardinality Cardinality { get; private set; }
		public IReadOnlyList<IClauseMember> Members { get; private set; }

		public IClause Clone(Func<Symbol, Symbol> symbolCloner)
		{
			symbolCloner = symbolCloner.ValidateArgumentIsNotNull();
			var members = new List<IClauseMember>();
			foreach (IClauseMember member in Members)
			{
				members.Add(_cloner.Invoke(member, symbolCloner));
			}
			return new Clause(members, Cardinality);
		}

		public IEnumerable<Symbol> Flatten()
		{
			foreach (IClauseMember member in Members)
			{
				foreach (Symbol symbol in member.Flatten())
				{
					yield return symbol;
				}
			}
		}

		public IClause Prune()
		{
			IClause clause = this;
			while (clause.Members.Count == 1 && clause.Cardinality == Cardinality.One)
			{
				var subClause = clause.Members[0] as IClause;
				if (subClause == null)
				{
					break;
				}
				clause = subClause;
			}
			return clause;
		}

		public override string ToString()
		{
			string s = string.Join(" ", Members);
			if (Members.Count > 1)
			{
				s = string.Format("({0})", s);
			}
			switch (Cardinality)
			{
				case Cardinality.OneOrMore:
					return s + "+";
				case Cardinality.ZeroOrMore:
					return s + "*";
				case Cardinality.ZeroOrOne:
					return s + "?";
			}
			return s;
		}

		private IClauseMember DefaultCloner([NotNull] IClauseMember member, Func<Symbol, Symbol> symbolCloner)
		{
			IClauseMember validMember = member.ValidateArgumentIsNotNull();
			Func<Symbol, Symbol> cloner = symbolCloner.ValidateArgumentIsNotNull();
			var symbol = validMember as Symbol;
			if (symbol != null)
			{
				return cloner.Invoke(symbol);
			}
			var clause = validMember as IClause;
			if (clause != null)
			{
				return clause.Clone(cloner);
			}
			throw new ArgumentOutOfRangeException("member");
		}
	}

	public partial class Clause
	{
		public bool Equals(IClause other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return Cardinality == other.Cardinality && Members.SequenceEqual(other.Members);
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
			var other = obj as IClause;
			return other != null && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Members.GetHashCode() * 397) ^ (int) Cardinality;
			}
		}

		public static bool operator ==(Clause left, IClause right)
		{
			return Equals(left, right);
		}

		public static bool operator ==(IClause left, Clause right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Clause left, IClause right)
		{
			return !Equals(left, right);
		}

		public static bool operator !=(IClause left, Clause right)
		{
			return !Equals(left, right);
		}
	}
}
