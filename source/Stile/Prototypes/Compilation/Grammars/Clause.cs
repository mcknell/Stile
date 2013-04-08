#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars
{
	public interface IClause
	{
		Cardinality Cardinality { get; }
		[NotNull]
		IReadOnlyList<IClause> Clauses { get; }
		[NotNull]
		IReadOnlyList<Symbol> Symbols { get; }
	}

	public class Clause : IClause
	{
		private static readonly IClause[] EmptyClauseArray = new IClause[0];
		private static readonly Symbol[] EmptySymbolArray = new Symbol[0];

		public Clause([NotNull] IEnumerable<Symbol> symbols, Cardinality? cardinality = null)
			: this(EmptyClauseArray, symbols, cardinality) {}

		public Clause(Cardinality cardinality, [NotNull] Symbol symbol, params Symbol[] symbols)
			: this(EmptyClauseArray, symbols.Unshift(symbol.ValidateArgumentIsNotNull()), cardinality) {}

		public Clause([NotNull] IEnumerable<IClause> clauses, Cardinality? cardinality = null)
			: this(clauses, EmptySymbolArray, cardinality) {}

		private Clause([NotNull] IEnumerable<IClause> clauses,
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
	}
}
