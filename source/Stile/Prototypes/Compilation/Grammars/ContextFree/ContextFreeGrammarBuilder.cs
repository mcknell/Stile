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
using Stile.Prototypes.Collections;
using Stile.Prototypes.Compilation.Grammars.CodeMetadata;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public class ContextFreeGrammarBuilder
	{
		private readonly HashSet<Symbol> _leftSymbols;
		private readonly HashSet<SymbolLink> _links;
		private readonly HashBucket<Symbol, Symbol> _ruleStartingSymbols;
		private readonly HashSet<Symbol> _symbols;

		public ContextFreeGrammarBuilder([NotNull] ProductionRule rule, params ProductionRule[] rules)
			: this(rules.Unshift(rule.ValidateArgumentIsNotNull())) {}

		public ContextFreeGrammarBuilder(IEnumerable<ProductionRule> rules)
		{
			_symbols = new HashSet<Symbol>();
			_links = new HashSet<SymbolLink>();
			_leftSymbols = new HashSet<Symbol>();
			_ruleStartingSymbols = new HashBucket<Symbol, Symbol>();
			foreach (ProductionRule rule in rules)
			{
				Symbol left = GetOrAdd(rule.Left, false);
				List<Symbol> rightSymbols = rule.Right.Select(GetOrAdd).ToList();
				if (rightSymbols.Any())
				{
					_ruleStartingSymbols.Add(rightSymbols.First(), left);
				}
				foreach (Tuple<Symbol, Symbol> tuple in rightSymbols.ToAdjacentPairs())
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
			get { return _symbols.ToArray(); }
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

		[NotNull]
		public ContextFreeGrammar Build()
		{
			var rules = new List<ProductionRule>();
			foreach (Symbol leftSymbol in _leftSymbols)
			{
				List<Symbol> startingSymbols =
					_ruleStartingSymbols.Where(x => x.Value.Any(y => y == leftSymbol)).Select(x => x.Key).ToList();
				foreach (Symbol ruleStartingSymbol in startingSymbols)
				{
					var stack = new Stack<Symbol>();
					stack.Push(ruleStartingSymbol);
					foreach (IList<string> list in EnumerateRules(stack))
					{
						rules.Add(new ProductionRule(leftSymbol.Token, list));
					}
				}
			}
			return new ContextFreeGrammar(_symbols, new HashSet<Symbol>(), rules, NonterminalSymbol.Specification);
		}

		private IEnumerable<IList<string>> EnumerateRules(Stack<Symbol> symbols)
		{
			List<SymbolLink> following = _links.Where(x => x.Prior == symbols.Peek()).ToList();
			foreach (SymbolLink link in following)
			{
				symbols.Push(link.Current);
				foreach (IList<string> list in EnumerateRules(symbols))
				{
					yield return list;
				}
				symbols.Pop();
			}
			if (following.None())
			{
				yield return symbols.Select(x => x.Token).Reverse().ToList();
			}
		}

		private Symbol GetOrAdd(string token)
		{
			return GetOrAdd(token, true);
		}

		private Symbol GetOrAdd(string token, bool canBeInlined)
		{
			Symbol symbol = _symbols.FirstOrDefault(x => x.Token == token);
			if (symbol != null)
			{
				return symbol;
			}
			symbol = new Symbol(token);
			if (canBeInlined == false)
			{
				_leftSymbols.Add(symbol);
			}
			_symbols.Add(symbol);
			return symbol;
		}
	}
}
