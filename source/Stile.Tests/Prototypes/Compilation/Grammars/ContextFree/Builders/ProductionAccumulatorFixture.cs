#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using NUnit.Framework;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
using Stile.Prototypes.Specifications.Grammar;
#endregion

namespace Stile.Tests.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	[TestFixture]
	public class ProductionAccumulatorFixture
	{
		private const string Right = "right";

		[SetUp]
		public void Init() {}

		[Test]
		[Explicit]
		public void AssembleExtends()
		{
			const Cardinality cardinality = Cardinality.ZeroOrOne;
			var next = new Nonterminal("next");
			var fragments = new HashSet<IFragment> {new Fragment(Right, next, cardinality)};
			var left = new Nonterminal("left");
			var right = new Nonterminal(Right);
			var rightChoice = new Choice(new Sequence(new Item(right)));
			var testSubject = new ProductionAccumulator(fragments, left, rightChoice);

			// act
			IProduction production = testSubject.Build();

			Assert.NotNull(production);
			Assert.NotNull(production);
			Assert.That(production.Left.Token, Is.EqualTo(left.Token));
			Assert.That(production.Right.Sequences[0].Items[0].Primary, Is.EqualTo(right.Token));
			Assert.That(production.Right.Sequences[0].Items[1].Primary, Is.EqualTo(next.Token));
			Assert.That(production.Right.Sequences[0].Items[1].Cardinality, Is.EqualTo(cardinality));
			Assert.Fail("wip");
		}
	}
}
