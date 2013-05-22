#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
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

		public static IReadOnlyList<IFragment> Find(MemberInfo memberInfo, RuleFragmentAttribute ruleFragment)
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

		public static IReadOnlyList<IFragment> Find(MemberInfo memberInfo, RuleCategoryAttribute attribute)
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
			string symbol = GetSymbol(parameterInfo, symbolToken);
			var nonterminal = new Nonterminal(symbol, alias);
			return nonterminal;
		}

		public static Nonterminal GetNonterminal(MethodBase methodInfo, string symbolToken, string alias)
		{
			string symbol = GetSymbol(methodInfo, symbolToken);
			var nonterminal = new Nonterminal(symbol, alias);
			return nonterminal;
		}

		public static string GetSymbol(ParameterInfo parameterInfo, [CanBeNull] string symbolToken)
		{
			return symbolToken ?? parameterInfo.Name;
		}

		public static string GetSymbol(MethodBase methodInfo, [CanBeNull] string symbolToken)
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

		public static ProductionBuilder Make(MemberInfo memberInfo, RuleAttribute attribute)
		{
			var propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return Make(propertyInfo, attribute);
			}
			var methodBase = memberInfo as MethodBase;
			if (methodBase != null)
			{
				return new ProductionExtractorFromMethod(methodBase, attribute).Build();
			}
			throw new NotImplementedException();
		}

		public static ProductionBuilder Make(PropertyInfo propertyInfo, RuleAttribute attribute)
		{
			var left = new Nonterminal(attribute.Left);
			var symbol = new Nonterminal(propertyInfo.Name, attribute.Alias);
			IChoice right = new Choice(new Sequence(new Item(symbol)));
			var builder = new ProductionBuilder(left, right, canBeInlined : attribute.CanBeInlined);
			if (attribute.StartsGrammar)
			{
				builder.SortOrder = -1;
			}
			return builder;
		}

		private static IReadOnlyList<IFragment> Find(PropertyInfo propertyInfo, RuleFragmentAttribute ruleFragment)
		{
			var nonterminal = new Nonterminal(ruleFragment.Token ?? propertyInfo.Name, ruleFragment.Alias);
			Cardinality cardinality = ruleFragment.Optional ? Cardinality.ZeroOrOne : Cardinality.One;
			var fragment = new Fragment(ruleFragment.Prior, nonterminal, cardinality);
			return new List<IFragment> {fragment};
		}

		public static class Default
		{
			public static readonly IFragment[] Fragments = new IFragment[0];
		}
	}
}
