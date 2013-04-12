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
using Stile.Prototypes.Collections;
using Stile.Prototypes.Compilation.Grammars;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Specifications.Grammar.Metadata
{
	public class Reflector
	{
		private const BindingFlags Everything =
			BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		private readonly List<Assembly> _assemblies;

		public Reflector()
			: this(typeof(Reflector).Assembly) {}

		public Reflector([NotNull] Assembly stile, params Assembly[] others)
		{
			_assemblies = others.Unshift(stile).ToList() //
				.Validate().EnumerableOf<Assembly>().IsNotNullOrEmpty();
		}

		public IEnumerable<Follower> FindRuleExpansions()
		{
			foreach (Tuple<MethodBase, RuleExpansionAttribute> tuple in GetMethods<RuleExpansionAttribute>())
			{
				MethodBase methodInfo = tuple.Item1;
				RuleExpansionAttribute ruleExpansion = tuple.Item2;
				string symbol = GetSymbol(methodInfo, ruleExpansion.SymbolToken);
				yield return new Follower(ruleExpansion.Prior, symbol);
			}
			foreach (Tuple<PropertyInfo, RuleExpansionAttribute> tuple in GetProperties<RuleExpansionAttribute>())
			{
				yield return new Follower(new Nonterminal(tuple.Item2.Prior), new Nonterminal(tuple.Item1.Name));
			}
		}

		public IEnumerable<IProductionRule> FindRules()
		{
			foreach (Tuple<MethodBase, RuleAttribute> tuple in GetRuleMethods())
			{
				IProductionRule rule = GetRule(tuple.Item1, tuple.Item2);
				yield return rule;
			}
			foreach (Tuple<PropertyInfo, RuleAttribute> tuple in GetRuleProperties())
			{
				IProductionRule rule = GetRule(tuple.Item1, tuple.Item2);
				yield return rule;
			}
			foreach (Tuple<MethodBase, SpecializationAttribute> tuple in GetMethods<SpecializationAttribute>())
			{
				ProductionRule rule = GetRule(tuple.Item1, tuple.Item2);
				yield return rule;
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

		private static ProductionRule GetRule(MethodBase methodInfo, SpecializationAttribute attribute)
		{
			string symbol = GetSymbol(methodInfo, attribute.SymbolToken);
			Type baseType = methodInfo.ReflectedType.BaseType;
			string name = baseType.Name;
			if (baseType.IsGenericType)
			{
				name = name.Substring(0, name.IndexOf("`"));
			}
			var left = new Nonterminal(name);
			var rule = new ProductionRule(left, new Nonterminal(symbol));
			return rule;
		}

		private IProductionRule GetRule([NotNull] PropertyInfo propertyInfo, [NotNull] RuleAttribute attribute)
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

		private IProductionRule GetRule([NotNull] MethodBase methodInfo, [NotNull] RuleAttribute attribute)
		{
			string symbol = GetSymbol(methodInfo, attribute.SymbolToken);
			var clauses = new List<IClause>();
			foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
			{
				string parameterName = parameterInfo.Name;
				var parameterAttribute = parameterInfo.GetCustomAttribute<SymbolAttribute>();
				if (parameterAttribute != null)
				{
					var cardinality = Cardinality.One;
					if (parameterInfo.IsOptional || parameterInfo.GetCustomAttribute<CanBeNullAttribute>() != null)
					{
						cardinality = Cardinality.ZeroOrOne;
					}
					clauses.Add(new Clause(cardinality, new Nonterminal(parameterName)));
				}
			}
			var clause = new Clause(clauses);
			var productionRule = new ProductionRule(new Nonterminal(symbol), clause);
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
			return symbol;
		}
	}
}
