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
	public interface IClause : IEquatable<IClause>
	{
		Cardinality Cardinality { get; }
		[NotNull]
		IReadOnlyList<IClause> Clauses { get; }
		[NotNull]
		IReadOnlyList<Symbol> Symbols { get; }

		[NotNull]
		IClause Clone([NotNull] Func<Symbol, Symbol> symbolCloner);

		[NotNull]
		IEnumerable<Symbol> Flatten();
	}

	public partial class Clause : IClause
	{
		private static readonly IClause[] EmptyClauseArray = new IClause[0];
		private static readonly Symbol[] EmptySymbolArray = new Symbol[0];

		public Clause([NotNull] IEnumerable<Symbol> symbols, Cardinality? cardinality = null)
			: this(EmptyClauseArray, symbols, cardinality) {}

		public Clause([NotNull] Symbol symbol, params Symbol[] symbols)
			: this(Cardinality.One, symbol, symbols) {}

		public Clause(Cardinality cardinality, [NotNull] Symbol symbol, params Symbol[] symbols)
			: this(EmptyClauseArray, symbols.Unshift(symbol.ValidateArgumentIsNotNull()), cardinality) {}

		public Clause([NotNull] IClause clause, params IClause[] clauses)
			: this(clauses.Unshift(clause.ValidateArgumentIsNotNull()), Cardinality.One) {}

		public Clause([NotNull] IEnumerable<IClause> clauses, Cardinality? cardinality = null)
			: this(clauses, EmptySymbolArray, cardinality) {}

		public Clause([NotNull] IEnumerable<IClause> clauses,
			[NotNull] IEnumerable<Symbol> symbols,
			Cardinality? cardinality = null)
		{
			Cardinality = cardinality ?? Cardinality.One;
			Clauses = clauses.ToArray();
			Symbols = symbols.ToArray();
		}

		public Cardinality Cardinality { get; private set; }
		public IReadOnlyList<IClause> Clauses { get; private set; }
		public IReadOnlyList<Symbol> Symbols { get; private set; }

		public IClause Clone(Func<Symbol, Symbol> symbolCloner)
		{
			Func<Symbol, Symbol> cloner = symbolCloner.ValidateArgumentIsNotNull();
			var clauses = new List<IClause>();
			foreach (IClause subClause in Clauses)
			{
				clauses.Add(subClause.Clone(cloner));
			}
			IEnumerable<Symbol> symbols = Symbols.Select(cloner);
			return new Clause(clauses, symbols, Cardinality);
		}

		public IEnumerable<Symbol> Flatten()
		{
			foreach (IClause clause in Clauses)
			{
				foreach (Symbol symbol in clause.Flatten())
				{
					yield return symbol;
				}
			}
			foreach (Symbol symbol in Symbols)
			{
				yield return symbol;
			}
		}

		public override string ToString()
		{
			string s = Symbols.Any() ? string.Join(" ", Symbols) : string.Join(" ", Clauses);
			if (Symbols.Count > 1)
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
			return Cardinality == other.Cardinality && Clauses.SequenceEqual(other.Clauses)
				&& Symbols.SequenceEqual(other.Symbols);
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
				return (Clauses.GetHashCode() * 397) ^ (int) Cardinality;
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
