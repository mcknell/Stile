#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Prototypes.Specifications.Grammar.Metadata;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	[TestFixture]
	public class ProductionBuilderFixture : ExtractorFixtureBase
	{
		[RuleFragment(Prior, Token, Alias, Optional = true)]
		public ProductionBuilderFixture(int foo) {}

		public ProductionBuilderFixture() {}

		[SetUp]
		public void Init() {}

		[Test]
		public void Combine()
		{
			var left = new Nonterminal("left");
			var sequenceRight = new Sequence(new Item(new Nonterminal("right")));
			var sequenceCame = new Sequence(new Item(new Nonterminal("came"), Cardinality.ZeroOrOne));
			var builder = new ProductionBuilder(left, new Choice(sequenceRight));
			var other = new ProductionBuilder(left, new Choice(sequenceCame));

			IProductionBuilder result = builder.Combine(other);

			Assert.NotNull(result);
			Assert.That(result.Left, Is.EqualTo(left));
			Assert.That(result.Right, Is.EqualTo(new Choice(sequenceRight, sequenceCame)));
		}

		[Test]
		public void GetFragmentsFromCategory()
		{
			Action<int> action = Category;
			string firstLeft = GetType().Name;
			string alias = MakeAlias(action, Alias);
			AssertCategoryFromMember(action.Method, firstLeft, alias);
		}

		[Test]
		public void GetFragmentsFromCtor()
		{
			ConstructorInfo constructorInfo = GetType().GetConstructor(new[] {typeof(int)});
			AssertExpansionFromMember(constructorInfo);
		}

		[Test]
		public void GetFragmentsFromMethod()
		{
			Action<int, int> action = Expansion;
			AssertExpansionFromMember(action.Method,
				null,
				new SymbolMetadata("Foo", "Foo"),
				new SymbolMetadata("Bar", "Bar"));
		}

		[Test]
		public void GetFragmentsFromProperty()
		{
			PropertyInfo propertyInfo = GetPropertyInfo(x => x.ExpansionProperty);
			AssertExpansionFromMember(propertyInfo);
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

		[RuleFragment(Prior, Token, Alias, Optional = true)]
		protected int ExpansionProperty
		{
			get { throw new NotImplementedException(); }
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

		[RuleCategory(Token, Alias = Alias)]
		protected static void Category([Symbol] int foo) {}

		[RuleFragment(Prior, Token, Alias, Optional = true)]
		protected static void Expansion([NonterminalSymbol] int foo, [NonterminalSymbol] int bar) {}

		private static PropertyInfo GetPropertyInfo(Expression<Func<ProductionBuilderFixture, object>> expression)
		{
			Expression body = expression.Body;
			var unaryExpression = body as UnaryExpression;
			if (unaryExpression != null)
			{
				body = unaryExpression.Operand;
			}
			var memberExpression = (MemberExpression) body;
			var propertyInfo = (PropertyInfo) memberExpression.Member;
			return propertyInfo;
		}
	}
}
