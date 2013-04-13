#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
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

		public void Add(string prior, string symbol)
		{
			Symbol priorSymbol = GetOrAdd(prior);
			Symbol currentSymbol = GetOrAdd(symbol);
			_links.Add(new Follower(priorSymbol, currentSymbol));
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
						clauses.Add(new Clause(middle));
						clauses.AddRange(back);
					}
					right = new Clause(clauses).Inline();
				}
				list.Add(new ProductionRule(key, right));
			}
			return list;
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
				return new Clause(clause, BuildClauses(followers.First().Current));
			}
			IEnumerable<IClauseMember> clauses = followers.Select(x => BuildClauses(x.Current)) //
				.Cast<IClauseMember>() //
				.Interlace(TerminalSymbol.EBNFAlternation);
			IClause alternatives = new Clause(clauses).Inline();
			return new Clause(clause, alternatives).Inline();
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
					clauses.Add(new Clause(distinct[0]).Inline());
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
					x => new Clause(x.Members.Skip(frontMatches).Take(x.Members.Count - frontMatches - backMatches)));
			return middle.Interlace(TerminalSymbol.EBNFAlternation);
		}

		private Symbol GetOrAdd(Symbol symbol)
		{
			return GetOrAdd(symbol.Token);
		}

		private Symbol GetOrAdd(string token)
		{
			NonterminalSymbol symbol = _symbols.FirstOrDefault(x => x.Token == token);
			if (symbol != null)
			{
				return symbol;
			}
			symbol = new Nonterminal(token);
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
					if (symbolsToInline.Intersect(clause.Flatten()).Any())
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
			return new Clause(members, clause.Cardinality);
		}
	}
}
