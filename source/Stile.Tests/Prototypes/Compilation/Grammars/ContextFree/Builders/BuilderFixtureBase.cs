#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
using Stile.Prototypes.Specifications.Grammar.Metadata;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	[TestFixture]
	public abstract class BuilderFixtureBase<TMember>
		where TMember : MemberInfo
	{
		protected const int Prior = 8;

		protected abstract Func<TMember, RuleAttribute, ProductionBuilder> Method { get; }

		protected void AssertRuleFromMember(TMember memberInfo, string token, bool canBeInlined, string alias = null)
		{
			RuleAttribute attribute = memberInfo.GetCustomAttributes<RuleAttribute>(false).First();

			// act
			ProductionBuilder builder = Method.Invoke(memberInfo, attribute);

			Assert.NotNull(builder);
			Assert.That(builder.CanBeInlined, Is.EqualTo(canBeInlined));
			Assert.That(builder.Left.Token, Is.EqualTo(Prior.ToString(CultureInfo.InvariantCulture)));
			Assert.NotNull(builder.Right);
			Assert.That(builder.Right.Sequences.Count, Is.EqualTo(1));
			ISequence sequence = builder.Right.Sequences[0];
			Assert.That(sequence.Items.Count, Is.EqualTo(1));
			IItem firstItem = sequence.Items[0];
			Assert.That(firstItem.Cardinality, Is.EqualTo(Cardinality.One));
			Assert.That(firstItem.Primary, Is.InstanceOf<NonterminalSymbol>());
			var nonterminalSymbol = (NonterminalSymbol) firstItem.Primary;
			Assert.That(nonterminalSymbol.Token, Is.EqualTo(token));
			Assert.That(nonterminalSymbol.Alias, Is.EqualTo(alias));
		}
	}
}
