#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Collections;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Types.Enumerables;
using Stile.Types.Primitives;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public interface IGrammarBuilder
	{
		void Add(IEnumerable<ILink> links);
		void Add(ILink link);
		void Add(IProductionRule rule, params IProductionRule[] rules);
		void Add(IEnumerable<IFragment> fragments);
		void Add(IFragment fragment);
		void Add(IProductionBuilder builder, params IProductionBuilder[] builders);
	}

	public class GrammarBuilder : IGrammarBuilder
	{
		private static readonly HashSet<TerminalSymbol> _terminalSymbols = new HashSet<TerminalSymbol>
		{
			TerminalSymbol.EBNFAlternation,
			TerminalSymbol.EBNFAssignment
		};
		private readonly HashBucket<Symbol, IClause> _clauses;
		private readonly HashSet<IFragment> _fragments;
		private readonly HashSet<ILink> _links;
		private readonly HashSet<IProductionBuilder> _productionBuilders;
		private readonly Dictionary<Symbol, int> _sortOrders;
		private readonly HashSet<NonterminalSymbol> _symbols;

		public GrammarBuilder()
		{
			_symbols = new HashSet<NonterminalSymbol>();
			_links = new HashSet<ILink>();
			_sortOrders = new Dictionary<Symbol, int>();
			_clauses = new HashBucket<Symbol, IClause>();
			_fragments = new HashSet<IFragment>();
			_productionBuilders = new HashSet<IProductionBuilder>();
		}

		public IReadOnlyHashBucket<Symbol, IClause> Clauses
		{
			get { return _clauses; }
		}

		public IReadOnlyList<ILink> Links
		{
			get { return _links.ToArray(); }
		}
		public IReadOnlyList<Symbol> Symbols
		{
			get { return _symbols.ToArray(); }
		}

		public void Add(IEnumerable<ILink> links)
		{
			foreach (ILink link in links)
			{
				Add(link);
			}
		}

		public void Add(ILink link)
		{
			_links.Add(link);
			GetOrAdd(link.Symbol);
			if (link.Prior != null)
			{
				GetOrAdd(link.Prior);
			}
		}

		public void Add(IProductionRule rule, params IProductionRule[] rules)
		{
			foreach (IProductionRule productionRule in rules.Unshift(rule))
			{
				AddOne(productionRule);
			}
		}

		public void Add(IEnumerable<IFragment> fragments)
		{
			foreach (IFragment fragment in fragments)
			{
				_fragments.Add(fragment);
			}
		}

		public void Add(IFragment fragment)
		{
			_fragments.Add(fragment);
			GetOrAdd(fragment.Left, null);
			GetOrAdd(fragment.Right);
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

		public IGrammar Buil()
		{
			var productions = new List<IProduction>();

			foreach (var builder in _productionBuilders.OrderBy(x => x.SortOrder))
			{
				productions.Add(builder.Assemble(_fragments));
			}
			return new Grammar(_symbols, _terminalSymbols, Nonterminal.Specification, productions);
		}

		[NotNull]
		public IGrammar Build(bool consolidate = true)
		{
			IList<IProductionRule> rules = new List<IProductionRule>();
			foreach (Symbol leftSymbol in Clauses.Keys.OrderBy(x => _sortOrders[x]))
			{
				foreach (IClause clause in Clauses[leftSymbol].OrderBy(x => x.ToString()))
				{
					IClause firstUnitClause = clause.GetFirstUnitClause();
					IClause clauses = BuildClauses(firstUnitClause);
					rules.Add(new ProductionRule(leftSymbol, clauses));
				}
			}
			if (consolidate)
			{
				rules = Consolidate(rules, _sortOrders);
			}
			return new Grammar(_symbols, _terminalSymbols, rules, Nonterminal.Specification);
		}

		public static IList<IProductionRule> Consolidate(IEnumerable<IProductionRule> rules,
			Dictionary<Symbol, int> sortOrders = null)
		{
			HashBucket<Symbol, IClause> hashBucket = rules.ToHashBucket(x => x.Left, x => x.Right);
			return Consolidate(hashBucket, sortOrders);
		}

		public static IList<IProductionRule> Consolidate(IReadOnlyHashBucket<Symbol, IClause> rules,
			Dictionary<Symbol, int> sortOrders = null)
		{
			var list = new List<IProductionRule>();
			IEnumerable<Symbol> keys = sortOrders == null ? rules.Keys : rules.Keys.OrderBy(x => sortOrders[x]);
			foreach (Symbol key in keys)
			{
				IClause right;
				if (rules[key].Count == 1)
				{
					right = rules[key].First();
				}
				else
				{
					List<IReadOnlyList<IClauseMember>> listsOfMembers = rules[key].Select(x => x.Members).ToList();
					var clauses = new List<IClause>(GetCommonClauses(listsOfMembers));
					if (clauses.Count != listsOfMembers.Max(x => x.Count))
					{
						List<IClause> back = GetCommonClauses(listsOfMembers, true);
						IEnumerable<IClauseMember> middle = GetMiddle(rules[key], clauses.Count, back.Count);
						clauses.Add(Clause.Make(middle));
						clauses.AddRange(back);
					}
					right = Clause.Make(clauses);
				}
				list.Add(new ProductionRule(key, right));
			}
			return list;
		}

		public Symbol GetOrAdd(Symbol symbol)
		{
			return GetOrAdd(symbol.Token, symbol.Alias);
		}

		public Symbol GetOrAdd(string token, string alias)
		{
			NonterminalSymbol symbol = _symbols.FirstOrDefault(x => x.Token == token);
			if (symbol != null)
			{
				if (symbol.Alias == alias || alias == null)
				{
					return symbol;
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
			return symbol;
		}

		private void AddOne(IProductionRule rule)
		{
			Symbol left = GetOrAdd(rule.Left);
			IClause right = rule.Right.Clone(GetOrAdd);
			_clauses.Add(left, right);
			RememberSortOrder(rule);
		}

		private IClause BuildClauses(IClause clause)
		{
			List<ILink> followers = GetFollowers(clause);
			if (followers.Count == 0)
			{
				return clause;
			}
			if (followers.Count == 1)
			{
				return Clause.Make(clause, BuildClauses(followers.First().Clause));
			}
			List<IClauseMember> clauses =
				followers.Select(x => BuildClauses(x.Clause)).ToList().Cast<IClauseMember>() //
					.Interlace(TerminalSymbol.EBNFAlternation) //
					.ToList();

			IClause alternatives = Clause.Make(clauses);
			return Clause.Make(clause, alternatives);
		}

		private static List<IClause> GetCommonClauses(
			[NotNull] IEnumerable<IEnumerable<IClauseMember>> listsOfMembers, bool reverse = false)
		{
			IEnumerable<IEnumerable<IClauseMember>> members = listsOfMembers.ValidateArgumentIsNotNull();
			if (reverse)
			{
				members = members.Select(x => x.Reverse());
			}
			var clauses = new List<IClause>();
			foreach (IList<IClauseMember> cohort in members.ForAll())
			{
				List<IClauseMember> distinct = cohort.Distinct().ToList();
				if (distinct.Count == 1)
				{
					clauses.Add(Clause.Make(distinct[0]));
				}
				else
				{
					break;
				}
			}
			if (reverse)
			{
				clauses.Reverse();
			}
			return clauses;
		}

		private List<ILink> GetFollowers(Symbol symbol)
		{
			return _links.Where(x => symbol.Equals(x.Prior)).ToList();
		}

		private List<ILink> GetFollowers(IClause clause)
		{
			Symbol symbol = clause.GetFirstNonterminal();
			return GetFollowers(symbol);
		}

		private static IEnumerable<IClauseMember> GetMiddle(IEnumerable<IClause> clauses,
			int frontMatches,
			int backMatches)
		{
			IEnumerable<IClauseMember> middle =
				clauses.Select(
					x => Clause.Make(x.Members.Skip(frontMatches).Take(x.Members.Count - frontMatches - backMatches)));
			return middle.Interlace(TerminalSymbol.EBNFAlternation);
		}

		private void RememberSortOrder(IProductionRule rule)
		{
			int order;
			_sortOrders.TryGetValue(rule.Left, out order);
			_sortOrders[rule.Left] = Math.Min(order, rule.SortOrder);
		}
	}
}
