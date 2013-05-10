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
using Stile.Prototypes.Specifications.Grammar;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public class GrammarBuilder
	{
		private readonly HashBucket<Symbol, IClause> _clauses;
		private readonly HashSet<IFollower> _links;
		private readonly HashSet<NonterminalSymbol> _symbols;

		public GrammarBuilder(params IProductionRule[] rules)
			: this(rules.AsEnumerable()) {}

		public GrammarBuilder([NotNull] IEnumerable<IProductionRule> rules)
		{
			_symbols = new HashSet<NonterminalSymbol>();
			_links = new HashSet<IFollower>();
			_clauses = Inline(rules.ValidateArgumentIsNotNull());
		}

		public IReadOnlyHashBucket<Symbol, IClause> Clauses
		{
			get { return _clauses; }
		}

		public IReadOnlyList<IFollower> Links
		{
			get { return _links.ToArray(); }
		}
		public IReadOnlyList<Symbol> Symbols
		{
			get { return _symbols.ToArray(); }
		}

		public void Add([NotNull] IFollower follower)
		{
			_links.Add(follower.Clone(GetOrAdd));
		}

		[NotNull]
		public IGrammar Build(bool consolidate = true)
		{
			IList<IProductionRule> rules = new List<IProductionRule>();
			foreach (Symbol leftSymbol in Clauses.Keys)
			{
				foreach (IClause clause in Clauses[leftSymbol])
				{
					IClause clauses = BuildClauses(clause);
					rules.Add(new ProductionRule(leftSymbol, clauses));
				}
			}
			if (consolidate)
			{
				rules = Consolidate(rules);
			}
			return new Grammar(_symbols,
				new HashSet<TerminalSymbol> {TerminalSymbol.EBNFAlternation, TerminalSymbol.EBNFAssignment},
				rules,
				Nonterminal.Specification);
		}

		public static IList<IProductionRule> Consolidate(IEnumerable<IProductionRule> rules)
		{
			HashBucket<Symbol, IClause> hashBucket = rules.ToHashBucket(x => x.Left, x => x.Right);
			return Consolidate(hashBucket);
		}

		public static IList<IProductionRule> Consolidate(IReadOnlyHashBucket<Symbol, IClause> rules)
		{
			var list = new List<IProductionRule>();
			foreach (Symbol key in rules.Keys)
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

		internal void Add(string prior, string symbol, string alias = null)
		{
			Symbol priorSymbol = GetOrAdd(prior, null);
			Symbol currentSymbol = GetOrAdd(symbol, alias);
			_links.Add(new Follower(priorSymbol, currentSymbol));
		}

		private IClause BuildClauses(IClause clause)
		{
			List<IFollower> followers = GetFollowers(clause);
			if (followers.Count == 0)
			{
				return clause;
			}
			if (followers.Count == 1)
			{
				return Clause.Make(clause, BuildClauses(followers.First().Current));
			}
			List<IClauseMember> clauses = followers.Select(x => BuildClauses(x.Current)) //
				.ToList() //
				.Cast<IClauseMember>() //
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

		private List<IFollower> GetFollowers(IClause clause)
		{
			return _links.Where(x => x.Prior.Equals(clause)).ToList();
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

		private Symbol GetOrAdd(Symbol symbol)
		{
			return GetOrAdd(symbol.Token, symbol.Alias);
		}

		private Symbol GetOrAdd(string token, string alias)
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
					throw new Exception(
						string.Format(
							"Tried to add token '{0}' with alias '{1}', but found a symbol with that token and alias '{2}' instead.",
							token,
							alias,
							symbol.Alias));
				}
			}
			symbol = new Nonterminal(token, alias);
			_symbols.Add(symbol);
			return symbol;
		}

		private HashBucket<Symbol, IClause> Inline(IEnumerable<IProductionRule> rules)
		{
			var clauses = new HashBucket<Symbol, IClause>();
			var candidatesToInline = new HashSet<IProductionRule>();
			foreach (IProductionRule rule in rules)
			{
				Symbol left = GetOrAdd(rule.Left);
				IClause right = rule.Right.Clone(GetOrAdd); // GetOrAdd symbols, even if we don't use the clause
				if (rule.CanBeInlined)
				{
					candidatesToInline.Add(rule);
				}
				else
				{
					clauses.Add(left, right);
				}
			}
			IList<IProductionRule> consolidatedCandidates = Consolidate(candidatesToInline);
			var rulesToInline = new HashSet<IProductionRule>();
			foreach (IProductionRule rule in consolidatedCandidates)
			{
				bool symbolMatchesRuleThatCannotInline = clauses.ContainsKey(rule.Left);
				if (symbolMatchesRuleThatCannotInline)
				{
					clauses.Add(rule.Left, rule.Right);
				}
				else
				{
					rulesToInline.Add(rule);
				}
			}
			return Inline(rulesToInline, clauses);
		}

		private HashBucket<Symbol, IClause> Inline(HashSet<IProductionRule> rulesToInline,
			HashBucket<Symbol, IClause> clauses)
		{
			HashSet<Symbol> symbolsToInline = rulesToInline.Select(y => y.Left).ToHashSet();
			var symbolsNeverInlined = new HashSet<Symbol>(symbolsToInline);
			var result = new HashBucket<Symbol, IClause>();
			foreach (Symbol key in clauses.Keys)
			{
				foreach (IClause clause in clauses[key])
				{
					if (clause.Intersects(symbolsToInline))
					{
						foreach (IProductionRule rule in rulesToInline)
						{
							symbolsNeverInlined.Remove(rule.Left);
							IClause inlined = Inline(rule, clause);
							result.Add(key, inlined);
						}
					}
					else
					{
						result.Add(key, clause);
					}
				}
			}
			foreach (IProductionRule ruleNeverInlined in rulesToInline.Where(x => symbolsNeverInlined.Contains(x.Left))
				)
			{
				result.Add(GetOrAdd(ruleNeverInlined.Left), ruleNeverInlined.Right.Clone(GetOrAdd));
			}
			return result;
		}

		private IClause Inline(IProductionRule ruleToInline, IClause clause)
		{
			var members = new List<IClauseMember>();
			foreach (IClauseMember member in clause.Members)
			{
				var memberAsClause = member as IClause;
				if (memberAsClause != null)
				{
					members.Add(Inline(ruleToInline, memberAsClause));
				}
				else if (member.Equals(ruleToInline.Left))
				{
					members.Add(ruleToInline.Right);
				}
				else
				{
					members.Add(member);
				}
			}
			return Clause.Make(members, clause.Cardinality);
		}
	}
}
