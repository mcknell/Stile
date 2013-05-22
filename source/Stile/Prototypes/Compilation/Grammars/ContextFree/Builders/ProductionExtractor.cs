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
	public interface IProductionExtractor
	{
		bool CanBeInlined { get; }
		IReadOnlyList<IFragment> Fragments { get; }
		Nonterminal Left { get; }
		[CanBeNull]
		IChoice Right { get; }
		int SortOrder { get; set; }
	}

	public class ProductionExtractor : IProductionExtractor
	{
		public ProductionExtractor(Nonterminal left,
			IChoice right,
			RuleAttribute attribute,
			IReadOnlyList<IFragment> fragments = null)
			: this(left, right, fragments, attribute.StartsGrammar ? -1 : 0, attribute.CanBeInlined) {}

		public ProductionExtractor(Nonterminal left,
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

		public static IReadOnlyList<IFragment> Find(MethodBase methodBase, RuleExpansionAttribute ruleExpansion)
		{
			return new FragmentExtractorFromMethod(methodBase, ruleExpansion).Build();
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

		public static ProductionExtractor Make(MemberInfo memberInfo, RuleAttribute attribute)
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

		public static ProductionExtractor Make(PropertyInfo propertyInfo, RuleAttribute attribute)
		{
			var left = new Nonterminal(attribute.Left);
			var symbol = new Nonterminal(propertyInfo.Name, attribute.Alias);
			IChoice right = new Choice(new Sequence(new Item(symbol)));
			var builder = new ProductionExtractor(left, right, canBeInlined : attribute.CanBeInlined);
			if (attribute.StartsGrammar)
			{
				builder.SortOrder = -1;
			}
			return builder;
		}

		public static class Default
		{
			public static readonly IFragment[] Fragments = new IFragment[0];
		}
	}
}
