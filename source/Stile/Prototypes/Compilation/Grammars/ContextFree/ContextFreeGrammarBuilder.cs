#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Collections;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public class ContextFreeGrammarBuilder
	{
		private readonly HashBucket<Symbol, IClause> _clauses;
		private readonly HashSet<IFollower> _links;
		private readonly HashSet<NonterminalSymbol> _symbols;

		public ContextFreeGrammarBuilder(params IProductionRule[] rules)
			: this(rules.AsEnumerable()) {}

		public ContextFreeGrammarBuilder([NotNull] IEnumerable<IProductionRule> rules)
		{
			_symbols = new HashSet<NonterminalSymbol>();
			_links = new HashSet<IFollower>();
			_clauses = new HashBucket<Symbol, IClause>();
			foreach (IProductionRule rule in rules.ValidateArgumentIsNotNull())
			{
				Symbol left = GetOrAdd(rule.Left);
				_clauses.Add(left, rule.Right.Clone(GetOrAdd));
			}
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
		public ContextFreeGrammar Build()
		{
			var rules = new List<IProductionRule>();
			foreach (Symbol leftSymbol in Clauses.Keys)
			{
				foreach (IClause clause in Clauses[leftSymbol])
				{
					var stack = new Stack<IClause>();
					stack.Push(clause);
					foreach (IClause enumeratedClause in EnumerateClauses(stack))
					{
						rules.Add(new ProductionRule(leftSymbol, enumeratedClause));
					}
				}
			}
			return new ContextFreeGrammar(_symbols, new HashSet<TerminalSymbol>(), rules, Nonterminal.Specification);
		}

		public static IList<IProductionRule> Consolidate(IReadOnlyHashBucket<Symbol, IClause> rules)
		{
			var list = new List<IProductionRule>();
			foreach (Symbol key in rules.Keys)
			{
				List<IReadOnlyList<IClauseMember>> listsOfMembers = rules[key].Select(x => x.Members).ToList();
				List<IClause> front = GetCommonClauses(listsOfMembers);
				List<IClause> back = GetCommonClauses(listsOfMembers.Select(x=>x.Reverse()));
				IEnumerable<IClauseMember> middle = GetMiddle(rules[key], front.Count, back.Count);
				var clauses = new List<IClause>(front);
				clauses.Add(new Clause(middle));
				clauses.AddRange(Enumerable.Reverse(back));
				var right = new Clause(clauses);
				list.Add(new ProductionRule(key, right));
			}
			return list;
		}

		public string ToEBNF()
		{
			var builder = new StringBuilder();
			foreach (Symbol leftSymbol in Clauses.Keys)
			{
				builder.AppendFormat("{0}{1} {2} ", Environment.NewLine, leftSymbol, TerminalSymbol.EBNFAssignment);
				ISet<IClause> clauses = Clauses[leftSymbol];
				if (clauses.Count > 1)
				{
					builder.Append("(");
				}
				foreach (IClause clause in clauses.SkipWith(x =>
				{
					builder.AppendFormat("{0} ", x);
					AppendFollowers(builder, x);
				}))
				{
					builder.AppendFormat(" | {0}", clause);
					AppendFollowers(builder, clause);
				}
				if (clauses.Count > 1)
				{
					builder.Append(")");
				}
			}
			return builder.ToString().Replace("  ", " ");
		}

		private void AppendFollowers(StringBuilder builder, IClause clause)
		{
			List<IFollower> followers = GetFollowers(clause);
			if (followers.Count > 1)
			{
				builder.Append("(");
			}
			foreach (IFollower follower in followers.SkipWith(x =>
			{
				builder.AppendFormat("{0} ", x.Current);
				AppendFollowers(builder, x.Current);
			}))
			{
				builder.AppendFormat(" | {0}", follower.Current);
				AppendFollowers(builder, follower.Current);
			}
			if (followers.Count > 1)
			{
				builder.Append(")");
			}
		}

		private IEnumerable<IClause> EnumerateClauses(Stack<IClause> clauses)
		{
			List<IFollower> following = GetFollowers(clauses.Peek());
			foreach (IFollower link in following)
			{
				clauses.Push(link.Current);
				foreach (IClause list in EnumerateClauses(clauses))
				{
					yield return list;
				}
				clauses.Pop();
			}
			if (following.None())
			{
				if (clauses.Count == 1)
				{
					yield return clauses.Peek();
				}
				else
				{
					yield return new Clause(clauses.Reverse());
				}
			}
		}

		private static List<IClause> GetCommonClauses(IEnumerable<IEnumerable<IClauseMember>> listsOfMembers)
		{
			var clauses = new List<IClause>();
			foreach (IList<IClauseMember> cohort in listsOfMembers.ForAll())
			{
				List<IClauseMember> distinct = cohort.Distinct().ToList();
				if (distinct.Count == 1)
				{
					clauses.Add(new Clause(distinct[0]).Prune());
				}
				else
				{
					break;
				}
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

		public class ConsolidatedClause
		{
			public IList<IClause> CommonBack { get; private set; }
			public IList<IClause> CommonFront { get; private set; }
			public IList<IList<IClause>> UniqueMiddles { get; private set; }
		}
	}
}
