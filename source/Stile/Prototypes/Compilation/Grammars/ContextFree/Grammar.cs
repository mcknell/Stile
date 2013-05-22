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
using Stile.Readability;
using Stile.Types.Primitives;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public interface IGrammar : IAcceptGrammarVisitors
	{
		Symbol InitialToken { get; }
		IReadOnlyList<Symbol> Nonterminals { get; }
		IReadOnlyList<IProductionRule> ProductionRules { get; }
		IReadOnlyList<IProduction> Productions { get; }
		IReadOnlyList<Symbol> Terminals { get; }
	}

	public class Grammar : IGrammar
	{
		private readonly HashSet<NonterminalSymbol> _nonterminals;
		private readonly HashSet<TerminalSymbol> _terminals;

		public Grammar(HashSet<NonterminalSymbol> nonterminals,
			HashSet<TerminalSymbol> terminals,
			Symbol initialToken,
			IReadOnlyList<IProduction> productions)
			: this(nonterminals, terminals, new IProductionRule[0], initialToken)
		{
			Productions = productions.ValidateArgumentIsNotNull();
		}

		public Grammar([NotNull] HashSet<NonterminalSymbol> nonterminals,
			[NotNull] HashSet<TerminalSymbol> terminals,
			[NotNull] IList<IProductionRule> productionRules,
			[NotNull] Symbol initialToken)
		{
			_nonterminals = nonterminals.ValidateArgumentIsNotNull();
			_terminals = terminals.ValidateArgumentIsNotNull();
			ProductionRules = productionRules.ValidateArgumentIsNotNull().ToArray();
			InitialToken = initialToken.ValidateArgumentIsNotNull();

			List<Symbol> overlap = nonterminals.Cast<Symbol>().Intersect(terminals).ToList();
			if (overlap.Any())
			{
				int count = overlap.Count;
				throw new ArgumentException(ErrorMessages.Grammar_Overlap.InvariantFormat(count,
					count.Pluralize("item"),
					string.Join(", ", overlap)));
			}

			if (_nonterminals.Contains(InitialToken) == false)
			{
				throw new ArgumentException(ErrorMessages.Grammar_InitialToken.InvariantFormat(InitialToken));
			}
		}

		public Symbol InitialToken { get; private set; }
		public IReadOnlyList<Symbol> Nonterminals
		{
			get { return _nonterminals.ToArray(); }
		}
		public IReadOnlyList<IProductionRule> ProductionRules { get; private set; }
		public IReadOnlyList<IProduction> Productions { get; private set; }
		public IReadOnlyList<Symbol> Terminals
		{
			get { return _terminals.ToArray(); }
		}

		public void Accept(IGrammarVisitor visitor)
		{
			visitor.Visit(this);
		}

		public TData Accept<TData>(IGrammarVisitor<TData> visitor, TData data)
		{
			return visitor.Visit(this, data);
		}
	}
}
