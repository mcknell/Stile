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
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
using Stile.Types.Enumerables;
using Stile.Types.Reflection;
#endregion

namespace Stile.Prototypes.Specifications.Grammar.Metadata
{
	public class Reflector
	{
		private const BindingFlags Everything =
			BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		private readonly List<Assembly> _assemblies;
		private readonly IGrammarBuilder _grammarBuilder;

		public Reflector(IGrammarBuilder grammarBuilder)
			: this(grammarBuilder, typeof(Reflector).Assembly) {}

		public Reflector(IGrammarBuilder grammarBuilder, Assembly stile, params Assembly[] others)
		{
			_grammarBuilder = grammarBuilder.ValidateArgumentIsNotNull();
			_assemblies = others.Unshift(stile).ToList() //
				.Validate().EnumerableOf<Assembly>().IsNotNullOrEmpty();
		}

		public void FindRuleExpansions()
		{
			foreach (Tuple<MethodBase, RuleAttribute> tuple in GetRuleMethods())
			{
				_grammarBuilder.Add(GetLinks(tuple.Item1));
			}
			foreach (Tuple<MethodBase, RuleFragmentAttribute> tuple in GetMethods<RuleFragmentAttribute>())
			{
				_grammarBuilder.Add(GetLinks(tuple.Item1, tuple.Item2));
			}
			foreach (
				Tuple<MethodBase, RuleCategoryAttribute> tuple in GetMethods<RuleCategoryAttribute>(false))
			{
				_grammarBuilder.Add(GetLink(tuple.Item1, tuple.Item2));
			}
			foreach (Tuple<PropertyInfo, RuleFragmentAttribute> tuple in GetProperties<RuleFragmentAttribute>())
			{
				_grammarBuilder.Add(GetLink(tuple.Item1, tuple.Item2));
			}
		}

		public void FindRules()
		{
			foreach (Tuple<MethodBase, RuleAttribute> tuple in GetRuleMethods())
			{
				IProductionRule rule = GetRule(tuple.Item1, tuple.Item2);
				_grammarBuilder.Add(rule);
			}
			foreach (Tuple<PropertyInfo, RuleAttribute> tuple in GetRuleProperties())
			{
				IProductionRule rule = GetRule(tuple.Item1, tuple.Item2);
				_grammarBuilder.Add(rule);
			}
		}

		public IEnumerable<Tuple<PropertyInfo, TAttribute>> GetProperties<TAttribute>() where TAttribute : Attribute
		{
			foreach (Type type in Types)
			{
				IEnumerable<PropertyInfo> methodBases = type.GetProperties(Everything) //
					.Where(x => x.ReflectedType == x.DeclaringType);
				foreach (PropertyInfo propertyInfo in methodBases)
				{
					var attribute = propertyInfo.GetCustomAttribute<TAttribute>(false);
					if (attribute != null)
					{
						yield return Tuple.Create(propertyInfo, attribute);
					}
				}
			}
		}

		private IEnumerable<Type> Types
		{
			get { return _assemblies.SelectMany(x => x.GetTypes()); }
		}

		private static IClause GetFirstRightClause(MethodBase methodInfo)
		{
			Clause clause = null;
			Tuple<ParameterInfo, SymbolAttribute> tuple =
				methodInfo.GetParametersWith<SymbolAttribute>().FirstOrDefault(t => t.Item2 != null);
			if (tuple != null)
			{
				ParameterInfo parameterInfo = tuple.Item1;
				SymbolAttribute symbolAttribute = tuple.Item2;
				var cardinality = Cardinality.One;
				if (parameterInfo.IsOptional || parameterInfo.GetCustomAttribute<CanBeNullAttribute>() != null)
				{
					cardinality = Cardinality.ZeroOrOne;
				}
				string name = parameterInfo.Name;
				string token = symbolAttribute.Token ?? name;
				string alias = symbolAttribute.Alias ?? (symbolAttribute.Terminal ? "\"" + name + "\"" : null);
				var nonterminal = new Nonterminal(token, alias);
				clause = Clause.Make(cardinality, nonterminal);
			}
			return clause;
		}

		private static Link GetLink(PropertyInfo propertyInfo, RuleFragmentAttribute fragmentAttribute)
		{
			var nonterminal = new Nonterminal(propertyInfo.Name, fragmentAttribute.Alias);
			return new Link(new Nonterminal(fragmentAttribute.Prior), nonterminal);
		}

		private static ILink GetLink(MethodBase methodBase, RuleCategoryAttribute attribute)
		{
			string name = attribute.Prior ?? Extractor.GetName(methodBase.ReflectedType);
			var prior = new Nonterminal(name);
			var symbol = new Nonterminal(methodBase.Name);
			IEnumerable<Tuple<ParameterInfo, SymbolAttribute>> parameters =
				methodBase.GetParametersWith<SymbolAttribute>();
			Clause clause = MakeParametersClause(parameters, symbol);
			return new Link(prior, symbol, clause : clause);
		}

		internal static IEnumerable<ILink> GetLinks(MethodBase methodBase, RuleFragmentAttribute ruleFragment)
		{
			var prior = new Nonterminal(ruleFragment.Prior);
			Nonterminal nonterminal = GetNonterminal(methodBase, ruleFragment.Token, ruleFragment.Alias);
			Cardinality cardinality = ruleFragment.Optional ? Cardinality.ZeroOrOne : Cardinality.One;
			List<Tuple<ParameterInfo, SymbolAttribute>> parameters =
				methodBase.GetParametersWith<SymbolAttribute>().ToList();
			if (parameters.Count > 0 && parameters.All(x => x.Item2.Terminal))
			{
				Clause clause = MakeParametersClause(parameters, nonterminal);
				yield return new Link(prior, nonterminal, cardinality, clause);
				yield break;
			}
			yield return new Link(prior, nonterminal, cardinality);
			prior = nonterminal;
			foreach (ILink link in GetParameterLinks(parameters, prior))
			{
				yield return link;
			}
		}

		internal static IEnumerable<ILink> GetLinks(MethodBase methodInfo)
		{
			List<Tuple<ParameterInfo, SymbolAttribute>> parameters =
				methodInfo.GetParametersWith<SymbolAttribute>().ToList();
			Tuple<ParameterInfo, SymbolAttribute> first = parameters.FirstOrDefault();
			if (first != null)
			{
				Nonterminal prior = GetNonterminal(first.Item1, first.Item2.Token);
				foreach (ILink link in GetParameterLinks(parameters.Skip(1), prior))
				{
					yield return link;
				}
			}
		}

		private static IClauseMember GetMember(Nonterminal nonterminal, Cardinality cardinality)
		{
			if (cardinality == Cardinality.One)
			{
				return nonterminal;
			}
			return Clause.Make(cardinality, nonterminal);
		}

		private IEnumerable<Tuple<MethodBase, TAttribute>> GetMethods<TAttribute>(
			bool reflectedTypeIsDeclaringType = true) where TAttribute : Attribute
		{
			foreach (Type type in Types)
			{
				IEnumerable<MethodBase> methodBases = type.GetMethods(Everything).Cast<MethodBase>() //
					.Concat(type.GetConstructors(Everything)) //
					.Where(x => (x.ReflectedType == x.DeclaringType) == reflectedTypeIsDeclaringType);
				foreach (MethodBase methodInfo in methodBases)
				{
					IEnumerable<TAttribute> attributes = methodInfo.GetCustomAttributes<TAttribute>(false);
					foreach (TAttribute attribute in attributes)
					{
						yield return Tuple.Create(methodInfo, attribute);
					}
				}
			}
		}

		private static Nonterminal GetNonterminal(MethodBase methodInfo, string symbolToken, string alias)
		{
			string symbol = GetSymbol(methodInfo, symbolToken);
			var nonterminal = new Nonterminal(symbol, alias);
			return nonterminal;
		}

		private static Nonterminal GetNonterminal(ParameterInfo parameterInfo, [CanBeNull] string symbolToken)
		{
			string symbol = GetSymbol(parameterInfo, symbolToken);
			return new Nonterminal(symbol);
		}

		private static IEnumerable<ILink> GetParameterLinks(
			IEnumerable<Tuple<ParameterInfo, SymbolAttribute>> parameters, NonterminalSymbol prior)
		{
			foreach (Tuple<Nonterminal, Cardinality> tuple in GetParameterSymbols(parameters))
			{
				Nonterminal nonterminal = tuple.Item1;
				Cardinality cardinality = tuple.Item2;
				yield return new Link(prior, nonterminal, cardinality);
				prior = nonterminal;
			}
		}

		private static IEnumerable<Tuple<Nonterminal, Cardinality>> GetParameterSymbols(
			IEnumerable<Tuple<ParameterInfo, SymbolAttribute>> parameters)
		{
			foreach (Tuple<ParameterInfo, SymbolAttribute> tuple in parameters)
			{
				ParameterInfo parameterInfo = tuple.Item1;
				SymbolAttribute attribute = tuple.Item2;
				var cardinality = Cardinality.One;
				if (parameterInfo.IsOptional || parameterInfo.GetCustomAttribute<CanBeNullAttribute>() != null)
				{
					cardinality = Cardinality.ZeroOrOne;
				}
				string token = GetSymbol(parameterInfo, attribute.Token);
				string alias = attribute.Alias ?? (attribute.Terminal ? "\"" + parameterInfo.Name + "\"" : null);
				var nonterminal = new Nonterminal(token, alias);
				yield return Tuple.Create(nonterminal, cardinality);
			}
		}

		private IProductionRule GetRule(PropertyInfo propertyInfo, RuleAttribute attribute)
		{
			var left = new Nonterminal(attribute.Left);
			var symbol = new Nonterminal(propertyInfo.Name, attribute.Alias);
			Clause right = Clause.Make(symbol);
			var productionRule = new ProductionRule(left, right) {CanBeInlined = attribute.CanBeInlined};
			if (attribute.StartsGrammar)
			{
				productionRule.SortOrder = -1;
			}
			return productionRule;
		}

		internal static IProductionRule GetRule([NotNull] MethodBase methodInfo, [NotNull] RuleAttribute attribute)
		{
			Nonterminal left = GetNonterminal(methodInfo, attribute.Left, attribute.Alias);
			IClause clause;
			if (attribute.NameIsSymbol)
			{
				string symbol = GetSymbol(methodInfo, null);

				IEnumerable<Tuple<ParameterInfo, SymbolAttribute>> parameters =
					methodInfo.GetParametersWith<SymbolAttribute>().Where(x => x.Item2.Terminal);
				IEnumerable<Tuple<Nonterminal, Cardinality>> symbols = GetParameterSymbols(parameters);
				IClauseMember[] members = symbols.Select(tuple => GetMember(tuple.Item1, tuple.Item2)).ToArray();
				clause = Clause.Make(new Nonterminal(symbol), members);
			}
			else
			{
				clause = GetFirstRightClause(methodInfo);
			}
			var productionRule = new ProductionRule(left, clause);
			if (attribute.StartsGrammar)
			{
				productionRule.SortOrder = -1;
			}
			return productionRule;
		}

		private IEnumerable<Tuple<MethodBase, RuleAttribute>> GetRuleMethods()
		{
			return GetMethods<RuleAttribute>();
		}

		private IEnumerable<Tuple<PropertyInfo, RuleAttribute>> GetRuleProperties()
		{
			return GetProperties<RuleAttribute>();
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

		private static Clause MakeParametersClause(IEnumerable<Tuple<ParameterInfo, SymbolAttribute>> parameters,
			Nonterminal symbol)
		{
			IEnumerable<Tuple<Nonterminal, Cardinality>> symbols = GetParameterSymbols(parameters);
			IEnumerable<Clause> clauses = symbols.Select(x => Clause.Make(x.Item2, x.Item1));
			Clause clause = Clause.Make(symbol, clauses.ToArray());
			return clause;
		}
	}
}
