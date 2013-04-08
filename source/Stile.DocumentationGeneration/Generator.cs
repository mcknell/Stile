#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Stile.Prototypes.Collections;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Types.Comparison;
#endregion

namespace Stile.DocumentationGeneration
{
	public class Generator
	{
		private readonly Dictionary<string, string> _literalLexicon;
		private readonly Reflector _reflector;

		public Generator(params Assembly[] others)
			: this(typeof(VersionedLanguage).Assembly, ProductionRuleExtensions.Lexicon, others) {}

		public Generator([NotNull] Assembly stile,
			Dictionary<string, string> literalLexicon,
			params Assembly[] others)
		{
			_literalLexicon = literalLexicon;
			_reflector = new Reflector(stile, others);
		}

		[NotNull]
		public string Generate()
		{
			HashBucket<Symbol, ProductionRule> rules = _reflector.FindRules();

			var grammarBuilder = new ContextFreeGrammarBuilder(rules.SelectMany(x => x.Value));

			foreach (SymbolLink symbolLink in _reflector.FindRuleExpansions())
			{
				grammarBuilder.AddLink(symbolLink);
			}

			IEnumerable<ProductionRule> consolidated = Consolidate(rules);

			IOrderedEnumerable<ProductionRule> sorted = consolidated.OrderBy(x => x.SortOrder);

			string generated = string.Join(Environment.NewLine, sorted.Select(x => x.ToEBNF()));
			return generated;
		}

		private List<ProductionRule> CollapseSameLeftHandSides(HashBucket<Symbol, ProductionRule> rules)
		{
			var list = new List<ProductionRule>();
			foreach (Symbol key in rules.Keys)
			{
				ISet<ProductionRule> set = rules[key];
				if (set.Count == 1)
				{
					list.Add(set.First());
					continue;
				}
				int smallestBucket = set.Min(x => x.Right.Count);

				int firstDifference = FindFirstDifference(set, smallestBucket);
				int lastDifference = FindLastDifference(set, smallestBucket);
				if (firstDifference + lastDifference > smallestBucket)
				{
					throw new Exception("Too much overlap");
				}

				IList<Symbol> firstSymbolList = set.First().Right;
				IEnumerable<Symbol> beginning = Slice(firstSymbolList, take : firstDifference);

				string end = Print(firstSymbolList, firstSymbolList.Count - lastDifference);

				//IEnumerable<IEnumerable<Symbol>> varyingInteriors =
				//	set.Select(x => Slice(x.Right, firstDifference, x.Right.Count - lastDifference));
				//string middle = string.Join("\r\n\t| ", varyingInteriors);
				//if (middle.Length > 0)
				//{
				//	middle = string.Format(" ({0}\r\n\t) ", middle);
				//}
				//int earliestSortOrder = set.Min(x => x.SortOrder);
				//var rule = new ProductionRule(key, beginning, middle, end) {SortOrder = earliestSortOrder};
				//list.Add(rule);
			}
			return list;
		}

		private IEnumerable<ProductionRule> Consolidate(HashBucket<Symbol, ProductionRule> rules)
		{
			List<ProductionRule> list = CollapseSameLeftHandSides(rules);
			IEnumerable<ProductionRule> inlined = Inline(list);
			return inlined;
		}

		private int FindFirstDifference(ISet<ProductionRule> rules, int smallestBucket)
		{
			int firstDifference = 0;
			for (; firstDifference < smallestBucket; firstDifference++)
			{
				string symbolFromFirstRule = rules.First().Right[firstDifference];
				if (rules.Skip(1).Any(x => symbolFromFirstRule != x.Right[firstDifference]))
				{
					break;
				}
			}
			return firstDifference;
		}

		private int FindLastDifference(ISet<ProductionRule> rules, int limit)
		{
			int lastDifference = 0;
			for (; lastDifference < limit; lastDifference++)
			{
				IList<Symbol> symbols = rules.First().Right;
				int copy = lastDifference;
				Func<IList<Symbol>, Symbol> getSymbol = list => list[list.Count - 1 - copy];
				Symbol symbolFromFirstRule = getSymbol(symbols);
				if (rules.Skip(1).Any(x => symbolFromFirstRule != getSymbol(x.Right)))
				{
					break;
				}
			}
			return lastDifference;
		}

		private IEnumerable<ProductionRule> Inline(List<ProductionRule> rules)
		{
			IEqualityComparer<ProductionRule> comparer =
				ComparerHelper.MakeComparer<ProductionRule>((x, y) => x.Left == y.Left);
			IEnumerable<ProductionRule> distinct = rules.Distinct(comparer);
			int distinctCount = distinct.Count();
			if (distinctCount < rules.Count)
			{
				throw new ArgumentException(
					string.Format("Rules must have distinct left-hand sides, but there were {0} redundancies.",
						(rules.Count - distinctCount)));
			}

			var list = new List<ProductionRule>(rules);
			int oldListSize;
			int rewrites;
			do
			{
				oldListSize = list.Count;
				rewrites = 0;
				// try to reduce the list
				List<ProductionRule> canBeInlined = list.Where(x => x.CanBeInlined).ToList();
				if (canBeInlined.Any() == false)
				{
					break;
				}

				foreach (ProductionRule ruleToInline in canBeInlined.ToArray())
				{
					Symbol left = ruleToInline.Left;
					ProductionRule[] toRewrite = list.Where(x => x != ruleToInline && x.Right.Any(y => y == left)).ToArray();
					foreach (ProductionRule rewrite in toRewrite)
					{
						list.Remove(rewrite);
						rewrites++;
						list.Add(rewrite.Inline(left, ruleToInline.Right));
					}
					list.Remove(ruleToInline);
				}
			}
			while (list.Count < oldListSize || rewrites > 0);
			return list;
		}

		private string Print(IList<Symbol> symbols, int? skip = null, int? take = null)
		{
			IEnumerable<Symbol> slice = symbols.Skip(skip ?? 0).Take(take ?? symbols.Count());
			string substituted = slice.Substitute(_literalLexicon);
			return substituted;
		}

		private IEnumerable<Symbol> Slice(IList<Symbol> symbols, int? skip = null, int? take = null)
		{
			IEnumerable<Symbol> slice = symbols.Skip(skip ?? 0).Take(take ?? symbols.Count());
			return slice;
		}
	}
}
