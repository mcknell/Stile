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
using NUnit.Framework;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
using Stile.Prototypes.Specifications.Grammar.Metadata;
using Stile.Types.Enumerables;
using Stile.Types.Enums;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	[TestFixture]
	public abstract class ExtractorFixtureBase
	{
		protected const int Prior = 8;
		protected const string Token = "tkn";
		protected const string Alias = "Is Not?";

		protected void AssertAttributeFromMember<TAttribute>(
			Func<MemberInfo, TAttribute, IReadOnlyList<IFragment>> func,
			MemberInfo memberInfo,
			string left = null,
			string alias = null,
			params SymbolMetadata[] symbols) where TAttribute : Attribute, IMetadataWithPrior
		{
			TAttribute attribute = memberInfo.GetCustomAttributes<TAttribute>(false).Single();

			// act
			IReadOnlyList<IFragment> fragments = func.Invoke(memberInfo, attribute);

			Assert.NotNull(fragments);
			Assert.That(fragments.Count, Is.EqualTo(symbols.Length + 1));

			IFragment fragment = fragments[0];
			Assert.That(fragment.Left, Is.EqualTo(left ?? attribute.Prior));
			string name = GetName(memberInfo);
			string titleCase = Symbol.ToTitleCase(attribute.Token ?? name);
			Assert.That(fragment.Right.Token, Is.EqualTo(titleCase));
			Assert.That(fragment.Right.Alias, Is.EqualTo(alias ?? attribute.Alias));

			var priorMetadata = new SymbolMetadata(attribute.Token, attribute.Alias);
			for (int i = 0; i < symbols.Length; i++)
			{
				SymbolMetadata metadata = symbols[i];
				string message = metadata.ToString();

				fragment = fragments[i + 1];
				titleCase = Symbol.ToTitleCase(priorMetadata.Token);
				Assert.That(fragment.Left, Is.EqualTo(titleCase), message);
				Assert.That(fragment.Right.Token, Is.EqualTo(metadata.Token), message);
				Assert.That(fragment.Right.Alias, Is.EqualTo(metadata.Alias), message);
				priorMetadata = metadata;
			}
		}

		protected void AssertCategoryFromMember(MemberInfo memberInfo,
			string left = null,
			string alias = null,
			params SymbolMetadata[] symbols)
		{
			AssertAttributeFromMember<RuleCategoryAttribute>(ProductionExtractor.Find, memberInfo, left, alias, symbols);
		}

		protected void AssertExpansionFromMember(MemberInfo memberInfo,
			string left = null,
			params SymbolMetadata[] symbols)
		{
			AssertAttributeFromMember<RuleFragmentAttribute>(ProductionExtractor.Find, memberInfo, left, null, symbols);
		}

		protected void AssertRuleFromMember<TMember>(TMember memberInfo,
			bool canBeInlined,
			SymbolMetadata symbol,
			params SymbolMetadata[] symbols) where TMember : MemberInfo
		{
			symbols = symbols.Unshift(symbol).ToArray();
			RuleAttribute attribute = memberInfo.GetCustomAttributes<RuleAttribute>(false).Single();

			// act
			ProductionExtractor extractor = ProductionExtractor.Make(memberInfo, attribute);

			Assert.NotNull(extractor);
			Assert.That(extractor.CanBeInlined, Is.EqualTo(canBeInlined));
			Assert.That(extractor.Left.Token, Is.EqualTo(Prior.ToString(CultureInfo.InvariantCulture)));
			Assert.NotNull(extractor.Right);
			Assert.That(extractor.Right.Sequences.Count, Is.EqualTo(1));
			ISequence sequence = extractor.Right.Sequences[0];
			Assert.That(sequence.Items.Count, Is.EqualTo(1));

			IItem item = sequence.Items[0];
			SymbolMetadata metadata = symbols[0];
			string message = metadata.ToString();
			Assert.That(item.Cardinality, Is.EqualTo(metadata.Cardinality), message);
			switch (metadata.Primary)
			{
				case SymbolMetadata.PrimaryFlavor.Nonterminal:
					Assert.That(item.Primary, Is.InstanceOf<NonterminalSymbol>(), message);
					break;
				case SymbolMetadata.PrimaryFlavor.StringLiteral:
					Assert.That(item.Primary, Is.InstanceOf<StringLiteral>(), message);
					break;
				case SymbolMetadata.PrimaryFlavor.Choice:
					Assert.That(item.Primary, Is.InstanceOf<IChoice>(), message);
					break;
				default:
					throw Enumeration.FailedToRecognize(() => metadata.Primary);
			}
			var nonterminalSymbol = (NonterminalSymbol) item.Primary;
			Assert.That(nonterminalSymbol.Token, Is.EqualTo(metadata.Token), message);
			Assert.That(nonterminalSymbol.Alias, Is.EqualTo(metadata.Alias), message);

			Assert.That(extractor.Fragments.Count, Is.EqualTo(symbols.Length - 1));

			SymbolMetadata priorMetadata = metadata;
			for (int i = 1; i < symbols.Length; i++)
			{
				metadata = symbols[i];
				message = metadata.ToString();

				IFragment fragment = extractor.Fragments[i - 1];
				Assert.That(fragment.Left, Is.EqualTo(priorMetadata.Token), message);
				Assert.That(fragment.Right.Token, Is.EqualTo(metadata.Token), message);
				Assert.That(fragment.Right.Alias, Is.EqualTo(metadata.Alias), message);
				priorMetadata = metadata;
			}
		}

		protected string GetName(MemberInfo memberInfo)
		{
			var methodBase = memberInfo as MethodBase;
			if (methodBase != null)
			{
				return methodBase.IsConstructor
					// ReSharper disable PossibleNullReferenceException
					? methodBase.DeclaringType.Name
					// ReSharper restore PossibleNullReferenceException
					: methodBase.Name;
			}
			return memberInfo.Name;
		}

		protected static string MakeAlias(Action<int> action, string name = null)
		{
			name = name ?? action.Method.Name;
			string firstParameterName = action.Method.GetParameters()[0].Name;
			string alias = string.Format("{0} \"{1}\"", name, firstParameterName);
			return alias;
		}

		protected class SymbolMetadata
		{
			public enum PrimaryFlavor
			{
				Nonterminal,
				StringLiteral,
				Choice
			}

			public SymbolMetadata(string token,
				string @alias = null,
				Cardinality cardinality = Cardinality.One,
				PrimaryFlavor primary = PrimaryFlavor.Nonterminal)
			{
				Token = token.ValidateStringNotNullOrEmpty();
				Alias = alias;
				Cardinality = cardinality;
				Primary = primary;
			}

// ReSharper disable MemberHidesStaticFromOuterClass
			public string Alias { get; private set; }
			public Cardinality Cardinality { get; private set; }
			public PrimaryFlavor Primary { get; private set; }
			public string Token { get; private set; }
// ReSharper restore MemberHidesStaticFromOuterClass

			public override string ToString()
			{
				return string.Format("{0} {1} {2} {3}", Token, Alias, Cardinality, Primary);
			}
		}
	}
}
