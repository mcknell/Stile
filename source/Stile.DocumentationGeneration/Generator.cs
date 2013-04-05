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
using Stile.Prototypes.Compilation.Grammars.CodeMetadata;
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
			HashBucket<string, ProductionRule> initialRules = GetInitialRules();
			HashBucket<string, ProductionRule> ruleCandidates = _reflector.FindRules();
			HashBucket<string, ProductionRule> rules = ruleCandidates.Concat(initialRules);

			var grammarBuilder = new GrammarBuilder(rules);

			foreach (SymbolLink symbolLink in _reflector.FindRuleExpansions())
			{
				grammarBuilder.AddLink(symbolLink);	
			}

			IEnumerable<ProductionRule> consolidated = Consolidate(rules);

			IOrderedEnumerable<ProductionRule> sorted = consolidated.OrderBy(x => x.SortOrder);

			string generated = string.Join(Environment.NewLine, sorted.Select(x => x.ToEBNF(_literalLexicon)));
			return generated;
		}

		private List<ProductionRule> CollapseSameLeftHandSides(HashBucket<string, ProductionRule> rules)
		{
			var list = new List<ProductionRule>();
			foreach (string key in rules.Keys)
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

				IList<string> firstSymbolList = set.First().Right;
				string beginning = Print(firstSymbolList, take : firstDifference);

				string end = Print(firstSymbolList, firstSymbolList.Count - lastDifference);

				IEnumerable<string> varyingInteriors =
					set.Select(x => Print(x.Right, firstDifference, x.Right.Count - lastDifference));
				string middle = string.Join("\r\n\t| ", varyingInteriors);
				if (middle.Length > 0)
				{
					middle = string.Format(" ({0}\r\n\t) ", middle);
				}
				int earliestSortOrder = set.Min(x => x.SortOrder);
				var rule = new ProductionRule(key, beginning, middle, end) {SortOrder = earliestSortOrder};
				list.Add(rule);
			}
			return list;
		}

		private IEnumerable<ProductionRule> Consolidate(HashBucket<string, ProductionRule> rules)
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

		private int FindLastDifference(ISet<ProductionRule> rules, int smallestBucket)
		{
			int lastDifference = 0;
			for (; lastDifference < smallestBucket; lastDifference++)
			{
				IList<string> symbols = rules.First().Right;
				int copy = lastDifference;
				Func<IList<string>, string> getSymbol = list => list[list.Count - 1 - copy];
				string symbolFromFirstRule = getSymbol(symbols);
				if (rules.Skip(1).Any(x => symbolFromFirstRule != getSymbol(x.Right)))
				{
					break;
				}
			}
			return lastDifference;
		}

		private static HashBucket<string, ProductionRule> GetInitialRules()
		{
			string negated = Variable.Negated.ToString();
			string conjunction = Variable.Conjunction.ToString();
			string eol = Terminal.EOL.ToString();
			Func<string, string, bool, ProductionRule> factory =
				(left, right, inline) => new ProductionRule(left, right) {CanBeInlined = inline, SortOrder = 99};
			var initialRules = new HashBucket<string, ProductionRule>
			{
				{negated, factory.Invoke(negated, "'not'?", true)},
				{conjunction, factory.Invoke(conjunction, eol + "? ( 'and' | 'but' )", false)},
				{eol, factory.Invoke(eol, "'CRLF' | '<br/>' | ''", false)},
			};
			return initialRules;
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
					string left = ruleToInline.Left;
					ProductionRule[] toRewrite =
						list.Where(x => x.Left != left && x.Right.Any(y => y.Contains(left))).ToArray();
					foreach (ProductionRule rewrite in toRewrite)
					{
						list.Remove(rewrite);
						rewrites++;

						const string followAWordBoundary = @"(?<=\b)";
						const string precedeAWordBoundary = @"(?=\b)";
						string pattern = string.Format("{0}{1}{2}", followAWordBoundary, left, precedeAWordBoundary);
						string replacement = string.Join(" ", ruleToInline.Right);
						list.Add(rewrite.RewriteRightSideWith(pattern, replacement));
					}
					list.Remove(ruleToInline);
				}
			}
			while (list.Count < oldListSize || rewrites > 0);
			return list;
		}

		private string Print(IList<string> symbols, int? skip = null, int? take = null)
		{
			IEnumerable<string> slice = symbols.Skip(skip ?? 0).Take(take ?? symbols.Count());
			string substituted = slice.Substitute(_literalLexicon);
			return substituted;
		}
	}
}
