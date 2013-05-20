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
using Stile.Types.Primitives;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public interface IClause : IClauseMember,
		IEquatable<IClause>
	{
		Cardinality Cardinality { get; }
		[NotNull]
		IReadOnlyList<IClauseMember> Members { get; }

		[NotNull]
		IClause Clone([NotNull] Func<Symbol, Symbol> symbolCloner);

		NonterminalSymbol GetFirstNonterminal();
		IClause GetFirstUnitClause();
		bool Intersects(HashSet<Symbol> symbols);
	}

	public partial class Clause : IClause
	{
		public delegate IClauseMember ClauseMemberCloner(IClauseMember member, Func<Symbol, Symbol> symbolCloner);

		private readonly ClauseMemberCloner _cloner;

		protected Clause([NotNull] IEnumerable<IClauseMember> members,
			Cardinality? cardinality = null,
			ClauseMemberCloner cloner = null)
		{
			Members = members.Validate().EnumerableOf<IClauseMember>().IsNotNullOrEmpty().ToArray();
			Cardinality = cardinality ?? Cardinality.One;
			_cloner = cloner ?? DefaultCloner;
		}

		public Cardinality Cardinality { get; private set; }
		public IReadOnlyList<IClauseMember> Members { get; private set; }

		public void Accept(IGrammarVisitor visitor)
		{
			visitor.Visit(this);
		}

		public IClause Clone(Func<Symbol, Symbol> symbolCloner)
		{
			symbolCloner = symbolCloner.ValidateArgumentIsNotNull();
			var members = new List<IClauseMember>();
			foreach (IClauseMember member in Members)
			{
				members.Add(_cloner.Invoke(member, symbolCloner));
			}
			return Make(members, Cardinality, _cloner);
		}

		public NonterminalSymbol GetFirstNonterminal()
		{
			return Members.Select(GetSymbol).First(x => x != null);
		}

		public IClause GetFirstUnitClause()
		{
			if (Members.Count == 1)
			{
				return this;
			}
			List<IClause> clauses = Members.OfType<IClause>().ToList();
			if (clauses.None())
			{
				return this;
			}
			return clauses.Select(x => x.GetFirstUnitClause()).First();
		}

		public bool Intersects(HashSet<Symbol> symbols)
		{
			if (symbols.None())
			{
				return false;
			}
			if (symbols.Intersect(Members.OfType<Symbol>()).Any())
			{
				return true;
			}
			IEnumerable<Clause> clauses = Members.OfType<Clause>();
			return clauses.Any(x => x.Intersects(symbols));
		}

		public static Clause Make(Cardinality cardinality,
			[NotNull] IClauseMember member,
			params IClauseMember[] members)
		{
			return Make(members.Unshift(member.ValidateArgumentIsNotNull()), cardinality);
		}

		public static Clause Make([NotNull] IClauseMember member, params IClauseMember[] members)
		{
			return Make(members.Unshift(member.ValidateArgumentIsNotNull()));
		}

		public static Clause Make([NotNull] IEnumerable<IClauseMember> members,
			Cardinality? cardinality = null,
			ClauseMemberCloner cloner = null)
		{
			return new Clause(members, cardinality, cloner).Tidy();
		}

		public override string ToString()
		{
			string s = string.Join(" ", Members);
			if (Members.Count > 1)
			{
				s = "({0})".InvariantFormat(s);
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

		private Clause Consolidate()
		{
			Clause clause = this;
			List<Clause> clauses = clause.Members.OfType<Clause>().ToList();
			if (clauses.Count > 1 && clauses.None(x => x.Members.Count == 1))
			{
				List<IClauseMember> lastMembers = clauses.Select(x => x.Members.Last()).Distinct().ToList();
				if (lastMembers.Count == 1)
				{
					var members = new List<IClauseMember>();
					foreach (IClauseMember clauseMember in clause.Members)
					{
						var subclause = clauseMember as Clause;
						if (subclause == (IClause) null)
						{
							members.Add(clauseMember);
						}
						else
						{
							members.Add(Make(subclause.Members.Take(subclause.Members.Count - 1)).Tidy());
						}
					}
					IClause tidied = Make(members);
					clause = Make(tidied, lastMembers[0]);
				}
			}
			return clause;
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

		private NonterminalSymbol GetSymbol(IClauseMember member)
		{
			var symbol = member as NonterminalSymbol;
			return symbol ?? ((IClause) member).GetFirstNonterminal();
		}

		private static int MemberHash(int x, IClauseMember y)
		{
			unchecked
			{
				return (y.GetHashCode() * 397) ^ x;
			}
		}

		private Clause Tidy()
		{
			Clause clause = Consolidate();
			while (clause.Members.Count == 1 && clause.Cardinality == Cardinality.One)
			{
				var subClause = clause.Members[0] as Clause;
				if (subClause == (IClause) null)
				{
					break;
				}
				clause = subClause;
			}
			return clause;
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
				int aggregateHash = Members.Aggregate(0, MemberHash);
				return aggregateHash ^ (int) Cardinality;
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
