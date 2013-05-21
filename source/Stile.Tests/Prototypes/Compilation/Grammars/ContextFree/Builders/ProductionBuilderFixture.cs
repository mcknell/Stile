#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
using Stile.Prototypes.Specifications.Grammar.Metadata;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	[TestFixture]
	public class ProductionBuilderFixture : BuilderFixtureBase<PropertyInfo>
	{
		private const string Alias = "Is Not?";

		[Test]
		public void GetProductionFromProperty_ThatCannotBeInlined()
		{
			PropertyInfo propertyInfo = GetPropertyInfo(x => x.NoInline);
			string name = propertyInfo.Name;
			AssertRuleFromMember(propertyInfo, name, false);
		}

		[Test]
		public void GetProductionFromProperty_WithAlias()
		{
			PropertyInfo propertyInfo = GetPropertyInfo(x => x.WithAlias);
			AssertRuleFromMember(propertyInfo, propertyInfo.Name, true, Alias);
		}

		protected override Func<PropertyInfo, RuleAttribute, ProductionBuilder> Method
		{
			get { return ProductionBuilder.Make; }
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

		private static PropertyInfo GetPropertyInfo(Expression<Func<ProductionBuilderFixture, object>> expression)
		{
			var memberExpression = (MemberExpression) expression.Body;
			var propertyInfo = (PropertyInfo) memberExpression.Member;
			return propertyInfo;
		}
	}
}
