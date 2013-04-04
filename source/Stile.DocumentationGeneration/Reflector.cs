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

		public HashBucket<string, ProductionRule> GetRuleCandidates()
		{
			var rules = new HashBucket<string, ProductionRule>();
			foreach (Tuple<MethodBase, RuleAttribute> tuple in GetRuleMethods())
			{
				ProductionRule rule = GetRule(tuple.Item1, tuple.Item2);
				rules.Add(rule.Left, rule);
			}
			foreach (Tuple<PropertyInfo, RuleAttribute> tuple in GetRuleProperties())
			{
				ProductionRule rule = GetProperty(tuple.Item1, tuple.Item2);
				rules.Add(rule.Left, rule);
			}
			return rules;
		}

		public IEnumerable<Tuple<MethodBase, RuleAttribute>> GetRuleMethods()
		{
			foreach (Assembly assembly in (IEnumerable<Assembly>) _assemblies)
			{
				foreach (Type type in assembly.GetTypes())
				{
					IEnumerable<MethodBase> methodBases = type.GetMethods(Everything).Cast<MethodBase>() //
						.Concat(type.GetConstructors(Everything)) //
						.Where(x => x.ReflectedType == x.DeclaringType);
					foreach (MethodBase methodInfo in methodBases)
					{
						var attribute = methodInfo.GetCustomAttribute<RuleAttribute>(false);
						if (attribute != null)
						{
							yield return Tuple.Create(methodInfo, attribute);
						}
					}
				}
			}
		}

		public IEnumerable<Tuple<PropertyInfo, RuleAttribute>> GetRuleProperties()
		{
			foreach (Assembly assembly in (IEnumerable<Assembly>) _assemblies)
			{
				foreach (Type type in assembly.GetTypes())
				{
					IEnumerable<PropertyInfo> methodBases = type.GetProperties(Everything) //
						.Where(x => x.ReflectedType == x.DeclaringType);
					foreach (PropertyInfo propertyInfo in methodBases)
					{
						var attribute = propertyInfo.GetCustomAttribute<RuleAttribute>(false);
						if (attribute != null)
						{
							yield return Tuple.Create(propertyInfo, attribute);
						}
					}
				}
			}
		}

		private ProductionRule GetProperty([NotNull] PropertyInfo propertyInfo, [NotNull] RuleAttribute attribute)
		{
			var productionRule = new ProductionRule(attribute.SymbolToken, propertyInfo.Name);
			if (attribute.StartsGrammar)
			{
				productionRule.SortOrder = -1;
			}
			return productionRule;
		}

		private ProductionRule GetRule([NotNull] MethodBase methodInfo, [NotNull] RuleAttribute attribute)
		{
			string symbol = GetSymbol(methodInfo, attribute);
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
			var productionRule = new ProductionRule(symbol, symbols);
			if (attribute.StartsGrammar)
			{
				productionRule.SortOrder = -1;
			}
			return productionRule;
		}

		private static string ToTitleCase(string parameterName)
		{
			return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(parameterName);
		}

		private static string GetSymbol(MethodBase methodInfo, RuleAttribute attribute)
		{
			var constructorInfo = methodInfo as ConstructorInfo;
			string symbol;
			if (attribute.SymbolToken != null)
			{
				symbol = attribute.SymbolToken;
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
	}
}
