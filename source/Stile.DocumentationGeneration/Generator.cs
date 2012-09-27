#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Propter project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Stile.Prototypes.Collections;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
using Stile.Types.Enumerables;
#endregion

namespace Stile.DocumentationGeneration
{
    public class Generator
    {
        private readonly List<Assembly> _assemblies;
        private readonly Dictionary<string, string> _literalLexicon;

        public Generator([NotNull] Assembly stile, Dictionary<string, string> literalLexicon, params Assembly[] others)
        {
            _literalLexicon = literalLexicon;
            _assemblies = others.Unshift(stile).ToList();
        }

        public string Generate()
        {
            HashBucket<string, ProductionRule> rules = GetRuleCandidates();

            IEnumerable<ProductionRule> consolidated = Consolidate(rules);

            string generated = string.Join(Environment.NewLine, consolidated.Select(x => x.ToEBNF(_literalLexicon)));
            return generated;
        }

        private IEnumerable<ProductionRule> Consolidate(HashBucket<string, ProductionRule> rules)
        {
            var list = new List<ProductionRule>();
            foreach (string key in rules.Keys)
            {
                IList<ProductionRule> bucket = rules[key];
                if (bucket.Count == 1)
                {
                    list.Add(bucket.First());
                    continue;
                }
                int smallestBucket = bucket.Min(x => x.Right.Count);

                int firstDifference = FindFirstDifference(bucket, smallestBucket);
                int lastDifference = FindLastDifference(bucket, smallestBucket);
                if (firstDifference + lastDifference > smallestBucket)
                {
                    throw new Exception("Too much overlap");
                }

                IList<string> firstSymbolList = bucket.First().Right;
                string beginning = Print(firstSymbolList, take : firstDifference);

                string end = Print(firstSymbolList, firstSymbolList.Count - lastDifference);

                IEnumerable<string> varyingInteriors =
                    bucket.Select(x => Print(x.Right, firstDifference, x.Right.Count - lastDifference));
                string middle = string.Join("\r\n\t| ", varyingInteriors);
                if (middle.Length > 0)
                {
                    middle = string.Format(" ({0}\r\n\t) ", middle);
                }
                list.Add(new ProductionRule(key, beginning, middle, end));
            }
            return list;
        }

        private int FindFirstDifference(IList<ProductionRule> rules, int smallestBucket)
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

        private int FindLastDifference(IList<ProductionRule> rules, int smallestBucket)
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

        private ProductionRule GetRule([NotNull] MethodBase methodInfo, [NotNull] RuleAttribute attribute)
        {
            string symbol = attribute.SymbolToken ?? methodInfo.Name;
            var symbols = new List<string>();
            foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
            {
                string parameterName = parameterInfo.Name;
                var parameterAttribute = parameterInfo.GetCustomAttribute<SymbolAttribute>();
                if (parameterAttribute != null)
                {
                    parameterName = parameterAttribute.SymbolToken ?? parameterName;
                    IEnumerable<string> strings = new[]
                    {parameterAttribute.PrefixToken, parameterName, parameterAttribute.SuffixToken} //
                        .Where(x => string.IsNullOrWhiteSpace(x) == false);
                    parameterName = string.Join(" ", strings);

                    if (parameterInfo.IsOptional)
                    {
                        parameterName = string.Format("( {0} )?", parameterName);
                    }
                    symbols.Add(parameterName);
                }
            }
            return new ProductionRule(symbol, symbols);
        }

        private HashBucket<string, ProductionRule> GetRuleCandidates()
        {
            var rules = new HashBucket<string, ProductionRule>();
            foreach (Tuple<MethodBase, RuleAttribute> tuple in GetRuleMethods(_assemblies))
            {
                ProductionRule rule = GetRule(tuple.Item1, tuple.Item2);
                rules.Add(rule.Left, rule);
            }
            return rules;
        }

        private static IEnumerable<Tuple<MethodBase, RuleAttribute>> GetRuleMethods(IEnumerable<Assembly> assemblies)
        {
            const BindingFlags bindingFlags =
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    IEnumerable<MethodBase> methodBases =
                        type.GetMethods(bindingFlags).Cast<MethodBase>().Concat(type.GetConstructors(bindingFlags));
                    foreach (MethodBase methodInfo in methodBases)
                    {
                        var attribute = methodInfo.GetCustomAttribute<RuleAttribute>();
                        if (attribute != null)
                        {
                            yield return Tuple.Create(methodInfo, attribute);
                        }
                    }
                }
            }
        }

        private string Print(IList<string> symbols, int? skip = null, int? take = null)
        {
            IEnumerable<string> slice = symbols.Skip(skip ?? 0).Take(take ?? symbols.Count());
            string substituted = slice.Substitute(_literalLexicon);
            return substituted;
        }
    }
}
