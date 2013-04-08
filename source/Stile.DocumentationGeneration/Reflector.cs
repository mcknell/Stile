#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Collections;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Compilation.Grammars.CodeMetadata;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata;
using Stile.Types.Enumerables;
#endregion

namespace Stile.DocumentationGeneration
{
	public class Reflector
	{
		private const BindingFlags Everything =
			BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		private readonly List<Assembly> _assemblies;

		public Reflector([NotNull] Assembly stile, params Assembly[] others)
		{
			_assemblies = others.Unshift(stile).ToList() //
				.Validate().EnumerableOf<Assembly>().IsNotNullOrEmpty();
		}

		public IEnumerable<SymbolLink> FindRuleExpansions()
		{
			foreach (Tuple<MethodBase, RuleExpansionAttribute> tuple in GetMethods<RuleExpansionAttribute>())
			{
				MethodBase methodInfo = tuple.Item1;
				RuleExpansionAttribute ruleExpansion = tuple.Item2;
				string symbol = GetSymbol(methodInfo, ruleExpansion.SymbolToken);
				yield return new SymbolLink(new Nonterminal(ruleExpansion.Prior), new Nonterminal(symbol));
			}
			foreach (Tuple<PropertyInfo, RuleExpansionAttribute> tuple in GetProperties<RuleExpansionAttribute>())
			{
				yield return
					new SymbolLink(new Nonterminal(tuple.Item2.Prior), new Nonterminal(tuple.Item1.Name));
			}
		}

		public HashBucket<Symbol, ProductionRule> FindRules()
		{
			var rules = new HashBucket<Symbol, ProductionRule>();
			foreach (Tuple<MethodBase, RuleAttribute> tuple in GetRuleMethods())
			{
				ProductionRule rule = GetRule(tuple.Item1, tuple.Item2);
				rules.Add(rule.Left, rule);
			}
			foreach (Tuple<PropertyInfo, RuleAttribute> tuple in GetRuleProperties())
			{
				ProductionRule rule = GetRule(tuple.Item1, tuple.Item2);
				rules.Add(rule.Left, rule);
			}
			return rules;
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

		private IEnumerable<Tuple<MethodBase, TAttribute>> GetMethods<TAttribute>() where TAttribute : Attribute
		{
			foreach (Type type in Types)
			{
				IEnumerable<MethodBase> methodBases = type.GetMethods(Everything).Cast<MethodBase>() //
					.Concat(type.GetConstructors(Everything)) //
					.Where(x => x.ReflectedType == x.DeclaringType);
				foreach (MethodBase methodInfo in methodBases)
				{
					var attribute = methodInfo.GetCustomAttribute<TAttribute>(false);
					if (attribute != null)
					{
						yield return Tuple.Create(methodInfo, attribute);
					}
				}
			}
		}

		private ProductionRule GetRule([NotNull] PropertyInfo propertyInfo, [NotNull] RuleAttribute attribute)
		{
			var left = new Nonterminal(attribute.SymbolToken);
			var right = new Nonterminal(propertyInfo.Name);
			var productionRule = new ProductionRule(left, right) {CanBeInlined = attribute.CanBeInlined};
			if (attribute.StartsGrammar)
			{
				productionRule.SortOrder = -1;
			}
			return productionRule;
		}

		private ProductionRule GetRule([NotNull] MethodBase methodInfo, [NotNull] RuleAttribute attribute)
		{
			string symbol = GetSymbol(methodInfo, attribute.SymbolToken);
			var symbols = new List<string>();
			foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
			{
				string parameterName = parameterInfo.Name;
				var parameterAttribute = parameterInfo.GetCustomAttribute<SymbolAttribute>();
				if (parameterAttribute != null)
				{
					if (parameterInfo.IsOptional)
					{
						parameterName = string.Format("{0}?", parameterName);
					}
					symbols.Add(ToTitleCase(parameterName));
				}
			}
			var productionRule = new ProductionRule(new Nonterminal(symbol), symbols.Select(Symbol.Make).ToList());
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
				int firstBackTick = symbol.IndexOf("`", StringComparison.InvariantCulture);
				if (firstBackTick > 0)
				{
					symbol = symbol.Substring(0, firstBackTick);
				}
			}
			else
			{
				symbol = methodInfo.Name;
			}
			string titleCase = ToTitleCase(symbol);
			return titleCase;
		}

		private static string ToTitleCase(string parameterName)
		{
			return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(parameterName);
		}
	}
}
