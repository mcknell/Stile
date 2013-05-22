#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using Stile.Prototypes.Specifications.Grammar.Metadata;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	[TestFixture]
	public class ProductionExtractorFixture : ExtractorFixtureBase
	{
		[RuleExpansion(Prior, Token, Alias, Optional = true)]
		public ProductionExtractorFixture(int foo) {}

		public ProductionExtractorFixture() {}

		[Test]
		public void GetFragmentsFromCtor_WithAlias()
		{
			// symbol, alias, optional
			ConstructorInfo constructorInfo = GetType().GetConstructor(new[] {typeof(int)});
			AssertExpansionFromMember(constructorInfo);
		}

		[Test]
		public void GetFragmentsFromMethod()
		{
			Action<int, int> action = Expansion;
			AssertExpansionFromMember(action.Method, new SymbolMetadata("Foo", "foo"), new SymbolMetadata("Bar", "bar"));
		}

		[Test]
		public void GetProductionFromProperty_ThatCannotBeInlined()
		{
			PropertyInfo propertyInfo = GetPropertyInfo(x => x.NoInline);
			string name = propertyInfo.Name;
			AssertRuleFromMember(propertyInfo, false, new SymbolMetadata(name));
		}

		[Test]
		public void GetProductionFromProperty_WithAlias()
		{
			PropertyInfo propertyInfo = GetPropertyInfo(x => x.WithAlias);
			AssertRuleFromMember(propertyInfo, true, new SymbolMetadata(propertyInfo.Name, Alias));
		}

		[Rule(Prior, CanBeInlined = false)]
		private object NoInline
		{
			get { return null; }
		}
		[Rule(Prior, Alias)]
		private object WithAlias
		{
			get { return null; }
		}

		[RuleExpansion(Prior, Token, Alias, Optional = true)]
		protected static void Expansion([NonterminalSymbol] int foo, [NonterminalSymbol] int bar) {}

		private static PropertyInfo GetPropertyInfo(Expression<Func<ProductionExtractorFixture, object>> expression)
		{
			var memberExpression = (MemberExpression) expression.Body;
			var propertyInfo = (PropertyInfo) memberExpression.Member;
			return propertyInfo;
		}
	}
}
