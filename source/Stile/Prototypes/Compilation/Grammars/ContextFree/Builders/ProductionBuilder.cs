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
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Types.Reflection;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public interface IProductionBuilder
	{
		bool CanBeInlined { get; }
		IReadOnlyList<IFragment> Fragments { get; }
		Nonterminal Left { get; }
		[CanBeNull]
		IChoice Right { get; }
		int SortOrder { get; set; }
	}

	public class ProductionBuilder : IProductionBuilder
	{
		public ProductionBuilder(Nonterminal left,
			IChoice right,
			RuleAttribute attribute,
			IReadOnlyList<IFragment> fragments = null)
			: this(left, right, fragments, attribute.StartsGrammar ? -1 : 0, attribute.CanBeInlined) {}

		public ProductionBuilder(Nonterminal left,
			IChoice right,
			IReadOnlyList<IFragment> fragments = null,
			int sortOrder = 0,
			bool canBeInlined = false)

		{
			Left = left.ValidateArgumentIsNotNull();
			CanBeInlined = canBeInlined;
			Fragments = fragments ?? Default.Fragments;
			Right = right;
			SortOrder = sortOrder;
		}

		public bool CanBeInlined { get; private set; }
		public IReadOnlyList<IFragment> Fragments { get; private set; }
		public Nonterminal Left { get; private set; }
		public IChoice Right { get; private set; }
		public int SortOrder { get; set; }

		public static ProductionBuilder Make(MethodBase methodBase, RuleAttribute attribute)
		{
			List<Tuple<ParameterInfo, SymbolAttribute>> parameterSymbols = GetParameterSymbols(methodBase).ToList();
			Nonterminal first = null;
			string firstAlias;
			string firstToken;
			Tuple<ParameterInfo, SymbolAttribute> priorTuple = null;
			if (attribute.NameIsSymbol)
			{
				firstAlias = attribute.Alias;
				firstToken = methodBase.Name;
			}
			else
			{
				Tuple<ParameterInfo, SymbolAttribute> tuple = parameterSymbols.First();
				parameterSymbols.RemoveAt(0);
				firstAlias = tuple.Item2.Alias;
				firstToken = firstAlias ?? tuple.Item2.Token ?? tuple.Item1.Name;
				if (tuple.Item2.Terminal == false)
				{
					priorTuple = tuple;
					first = new Nonterminal(firstToken, firstAlias);
				}
			}
			var triples = new List<Tuple<ParameterInfo, string, string>>();
			var aliases = new List<string> {firstAlias ?? firstToken};
			string alias = null;
// ReSharper disable AccessToModifiedClosure
			Action<Tuple<ParameterInfo, SymbolAttribute>> process = tuple =>
			{
				if (priorTuple == null)
				{
					first = new Nonterminal(firstToken, alias);
				}
				else
				{
					triples.Add(Tuple.Create(priorTuple.Item1, priorTuple.Item2.Token, alias));
				}
			};

			foreach (Tuple<ParameterInfo, SymbolAttribute> tuple in parameterSymbols)
			{
				SymbolAttribute symbolAttribute = tuple.Item2;
				if (symbolAttribute.Terminal)
				{
					aliases.Add(MakeAlias(tuple));
				}
				else
				{
					alias = aliases.Any() ? string.Join(" ", aliases) : null;
					process.Invoke(tuple);
					// for future loops
					priorTuple = tuple;
					aliases.Clear();
					aliases.Add(symbolAttribute.Alias ?? symbolAttribute.Token);
				}
			}
			if (aliases.Any())
			{
				alias = string.Join(" ", aliases);
				process.Invoke(priorTuple);
			}
// ReSharper restore AccessToModifiedClosure
			return Make(methodBase, attribute, first, triples);
		}

		private static Cardinality GetCardinality(ParameterInfo parameterInfo)
		{
			if (parameterInfo.IsOptional || parameterInfo.GetCustomAttribute<CanBeNullAttribute>() != null)
			{
				return Cardinality.ZeroOrOne;
			}
			return Cardinality.One;
		}

		private static Nonterminal GetNonterminal(ParameterInfo parameterInfo, string symbolToken, string alias)
		{
			string symbol = GetSymbol(parameterInfo, symbolToken);
			var nonterminal = new Nonterminal(symbol, alias);
			return nonterminal;
		}

		private static Nonterminal GetNonterminal(MethodBase methodInfo, string symbolToken, string alias)
		{
			string symbol = GetSymbol(methodInfo, symbolToken);
			var nonterminal = new Nonterminal(symbol, alias);
			return nonterminal;
		}

		private static IEnumerable<Tuple<ParameterInfo, SymbolAttribute>> GetParameterSymbols(MethodBase methodBase)
		{
			return methodBase.GetParametersWith<SymbolAttribute>();
		}

		private static string GetSymbol(ParameterInfo parameterInfo, [CanBeNull] string symbolToken)
		{
			return symbolToken ?? parameterInfo.Name;
		}

		private static string GetSymbol(MethodBase methodInfo, [CanBeNull] string symbolToken)
		{
			var constructorInfo = methodInfo as ConstructorInfo;
			string symbol;
			if (symbolToken != null)
			{
				symbol = symbolToken;
			}
			else if (constructorInfo != null)
			{
				Type declaringType = constructorInfo.DeclaringType;

				symbol = //
					// declaringType will only be null when MemberInfo object is a global member
					// see http://msdn.microsoft.com/en-us/library/system.reflection.memberinfo.declaringtype.aspx
					// ReSharper disable PossibleNullReferenceException
					declaringType
						// ReSharper restore PossibleNullReferenceException
						.Name;
				int firstBackTick = symbol.IndexOf(TypeStringBuilder.GenericArgumentDelimiter, StringComparison.Ordinal);
				if (firstBackTick > 0)
				{
					symbol = symbol.Substring(0, firstBackTick);
				}
			}
			else
			{
				symbol = methodInfo.Name;
			}
			return symbol;
		}

		private static ProductionBuilder Make(MethodBase methodBase,
			RuleAttribute attribute,
			Nonterminal first,
			IEnumerable<Tuple<ParameterInfo, string, string>> parametersToConsider)
		{
			Nonterminal left = GetNonterminal(methodBase, attribute.Symbol, attribute.Alias);
			Nonterminal prior = first;
			var fragments = new List<IFragment>();
			foreach (Tuple<ParameterInfo, string, string> tuple in parametersToConsider)
			{
				Nonterminal latest = GetNonterminal(tuple.Item1, tuple.Item2, tuple.Item3);
				var fragment = new Fragment(prior.Token, latest.Token, tuple.Item3);
				fragments.Add(fragment);
				// clean up loop
				prior = latest;
			}
			var right = new Choice(new Sequence(new Item(first)));
			var builder = new ProductionBuilder(left, right, attribute, fragments);
			return builder;
		}

		private static string MakeAlias(Tuple<ParameterInfo, SymbolAttribute> tuple)
		{
			ParameterInfo parameterInfo = tuple.Item1;
			SymbolAttribute attribute = tuple.Item2;
			Cardinality cardinality = GetCardinality(parameterInfo);
			string symbol = GetSymbol(parameterInfo, attribute.Token);
			string alias = attribute.Alias ?? symbol;
			var terminal = new StringLiteral(alias);
			Tuple<StringLiteral, Cardinality> terminalTuple1 = Tuple.Create(terminal, cardinality);
			Tuple<StringLiteral, Cardinality> terminalTuple = terminalTuple1;
			string s = terminalTuple.Item1.Alias.Trim() + terminalTuple.Item2.ToEbnfString();
			return s;
		}

		private static class Default
		{
			public static readonly IFragment[] Fragments = new IFragment[0];
		}
	}
}
