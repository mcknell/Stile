#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Types.Reflection;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public interface IExtractor
	{
		IReadOnlyList<IFragment> Find(MemberInfo memberInfo, RuleFragmentAttribute ruleFragment);
		IReadOnlyList<IFragment> Find(MemberInfo memberInfo, RuleCategoryAttribute attribute);
	}

	public class Extractor : IExtractor
	{
		public IReadOnlyList<IFragment> Find(MemberInfo memberInfo, RuleFragmentAttribute ruleFragment)
		{
			var propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return Find(propertyInfo, ruleFragment);
			}
			var methodBase = memberInfo as MethodBase;
			if (methodBase != null)
			{
				return new FragmentExtractorFromMethod(methodBase, ruleFragment).Build();
			}
			throw new NotImplementedException();
		}

		public IReadOnlyList<IFragment> Find(MemberInfo memberInfo, RuleCategoryAttribute attribute)
		{
			var methodBase = memberInfo as MethodBase;
			if (methodBase != null)
			{
				return new FragmentExtractorFromCategory(methodBase, attribute).Build();
			}
			throw new NotImplementedException();
		}

		public static string GetName(Type baseType)
		{
			string name = baseType.Name;
			if (baseType.IsGenericType)
			{
				name = name.Substring(0,
					name.IndexOf(TypeStringBuilder.GenericArgumentDelimiter, StringComparison.Ordinal));
			}
			return name;
		}

		public static Nonterminal GetNonterminal(ParameterInfo parameterInfo, string symbolToken, string alias)
		{
			string symbol = GetToken(parameterInfo, symbolToken);
			var nonterminal = new Nonterminal(symbol, alias);
			return nonterminal;
		}

		public static Nonterminal GetNonterminal(MethodBase methodInfo, string symbolToken, string alias)
		{
			string symbol = GetToken(methodInfo, symbolToken);
			var nonterminal = new Nonterminal(symbol, alias);
			return nonterminal;
		}

		public static string GetToken(ParameterInfo parameterInfo, [CanBeNull] string symbolToken)
		{
			return symbolToken ?? parameterInfo.Name;
		}

		public static string GetToken(MethodBase methodInfo, [CanBeNull] string symbolToken)
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

		private IReadOnlyList<IFragment> Find(PropertyInfo propertyInfo, RuleFragmentAttribute ruleFragment)
		{
			var nonterminal = new Nonterminal(ruleFragment.Token ?? propertyInfo.Name, ruleFragment.Alias);
			Cardinality cardinality = ruleFragment.Optional ? Cardinality.ZeroOrOne : Cardinality.One;
			var fragment = new Fragment(ruleFragment.Prior, nonterminal, cardinality);
			return new List<IFragment> {fragment};
		}
	}
}
