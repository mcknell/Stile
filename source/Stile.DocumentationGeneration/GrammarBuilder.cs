#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Stile.Prototypes.Collections;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Compilation.Grammars.CodeMetadata;
using Stile.Types.Enumerables;
#endregion

namespace Stile.DocumentationGeneration
{
	public class GrammarBuilder
	{
		private readonly HashSet<SymbolLink> _links;
		private readonly ConcurrentDictionary<string, Symbol> _symbols;

		public GrammarBuilder(HashBucket<string, ProductionRule> rules)
		{
			_symbols = new ConcurrentDictionary<string, Symbol>();
			_links = new HashSet<SymbolLink>();
			foreach (ProductionRule rule in rules.SelectMany(pair => pair.Value))
			{
				GetOrAdd(rule.Left);
				foreach (Tuple<Symbol, Symbol> tuple in rule.Right.Select(GetOrAdd).ToAdjacentPairs())
				{
					_links.Add(new SymbolLink(tuple.Item1, tuple.Item2));
				}
			}
		}

		public IReadOnlyCollection<SymbolLink> Links
		{
			get { return _links.ToArray(); }
		}
		public IReadOnlyCollection<Symbol> Symbols
		{
			get { return _symbols.Values.ToArray(); }
		}

		public void AddLink(string prior, string symbol)
		{
			Symbol priorSymbol = GetOrAdd(prior);
			Symbol currentSymbol = GetOrAdd(symbol);
			_links.Add(new SymbolLink(priorSymbol, currentSymbol));
		}

		public void AddLink([NotNull] SymbolLink symbolLink)
		{
			AddLink(symbolLink.Prior.Token, symbolLink.Current.Token);
		}

		private Symbol GetOrAdd(string token)
		{
			return _symbols.GetOrAdd(token, Symbol.Make);
		}
	}
}
