#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Types.Enumerables;
using Stile.Types.Primitives;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public interface IGrammarBuilder
	{
		void Add(IEnumerable<IFragment> fragments);
		void Add(IProductionBuilder builder, params IProductionBuilder[] builders);
	}

	public class GrammarBuilder : IGrammarBuilder
	{
		private static readonly HashSet<TerminalSymbol> _terminalSymbols = new HashSet<TerminalSymbol>
		{
			TerminalSymbol.EBNFAlternation,
			TerminalSymbol.EBNFAssignment
		};
		private readonly HashSet<IFragment> _fragments;
		private readonly HashSet<IProductionBuilder> _productionBuilders;
		private readonly HashSet<NonterminalSymbol> _symbols;

		public GrammarBuilder()
		{
			_symbols = new HashSet<NonterminalSymbol>();
			_fragments = new HashSet<IFragment>();
			_productionBuilders = new HashSet<IProductionBuilder>();
		}

		public void Add(IEnumerable<IFragment> fragments)
		{
			foreach (IFragment fragment in fragments)
			{
				_fragments.Add(fragment);
				GetOrAdd(fragment.Left, null);
				GetOrAdd(fragment.Right);
			}
		}

		public void Add(IProductionBuilder builder, params IProductionBuilder[] builders)
		{
			foreach (IProductionBuilder item in builders.Unshift(builder))
			{
				_productionBuilders.Add(item);
				GetOrAdd(item.Left);
				foreach (ISequence sequence in item.Right.Sequences)
				{
					GetOrAdd(sequence.FirstSymbol());
				}
				Add(item.Fragments);
			}
		}

		public IGrammar Build()
		{
			var productions = new List<IProduction>();
			Reduce();
			foreach (IProductionBuilder builder in _productionBuilders.OrderBy(x => x.SortOrder))
			{
				productions.Add(builder.Assemble(_fragments));
			}
			return new Grammar(_symbols, _terminalSymbols, Nonterminal.Specification, productions);
		}

		public void GetOrAdd(Symbol symbol)
		{
			GetOrAdd(symbol.Token, symbol.Alias);
		}

		public void GetOrAdd(string token, string alias)
		{
			NonterminalSymbol symbol = _symbols.FirstOrDefault(x => x.Token == token);
			if (symbol != null)
			{
				if (symbol.Alias == alias || alias == null)
				{
					return;
				}
				if (symbol.Alias == null)
				{
					_symbols.Remove(symbol);
				}
				else
				{
					throw new InvalidDataException(ErrorMessages.GrammarBuilder_AliasCollision.InvariantFormat(token,
						alias,
						symbol.Alias));
				}
			}
			symbol = new Nonterminal(token, alias);
			_symbols.Add(symbol);
		}

		internal void Reduce()
		{
			if (_productionBuilders.Count < 2)
			{
				return;
			}
			var processed = new List<IProductionBuilder>();
			IEnumerable<IGrouping<Nonterminal, IProductionBuilder>> groups = _productionBuilders.GroupBy(x => x.Left);
			foreach (IGrouping<Nonterminal, IProductionBuilder> grouping in groups)
			{
				IProductionBuilder first = grouping.First();
				if (grouping.Count() == 1)
				{
					processed.Add(first);
					continue;
				}
				IProductionBuilder aggregate = grouping.Skip(1).Aggregate(first, (x, y) => x.Combine(y));
				processed.Add(aggregate);
			}
			_productionBuilders.Clear();
			_productionBuilders.UnionWith(processed);
		}
	}
}
