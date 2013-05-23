#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Linq;
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
		private const string Right = "Right";
		private const string Next = "Next";
		private const string Last = "Last";
		private Nonterminal _left;
		private Nonterminal _right;
		private Choice _rightChoice;

		[SetUp]
		public void Init()
		{
			_left = new Nonterminal("Left");
			_right = new Nonterminal(Right);
			_rightChoice = new Choice(new Sequence(new Item(_right)));
		}

		[Test]
		public void AssembleExtends()
		{
			const Cardinality cardinality = Cardinality.ZeroOrOne;
			var next = new Nonterminal(Next);
			var fragments = new HashSet<IFragment> {new Fragment(Right, next, cardinality)};
			var testSubject = new ProductionAccumulator(fragments, _left, _rightChoice);

			// act
			IProduction production = testSubject.Build();

			Assert.NotNull(production);
			Assert.That(production.Left.Token, Is.EqualTo(_left.Token));
			Assert.That(production.Right.Sequences[0].Items[0].Primary, Is.EqualTo(_right));
			Assert.That(production.Right.Sequences[0].Items[1].Primary, Is.EqualTo(next));
			Assert.That(production.Right.Sequences[0].Items[1].Cardinality, Is.EqualTo(cardinality));
			Assert.That(production.Right.Sequences[0].Items.Count, Is.EqualTo(2));
		}

		[Test]
		public void AssembleFindsAlternates()
		{
			var next = new Nonterminal(Next);
			var last = new Nonterminal(Last);
			var fragments = new HashSet<IFragment> {new Fragment(Right, last)};
			var sequence = new Sequence(new Item(_right), new Item(next));
			Assert.That(sequence.Items[0].PrimaryAsSymbol().Token, Is.EqualTo(Right));
			Assert.That(sequence.Items[1].PrimaryAsSymbol().Token, Is.EqualTo(Next));
			var choice = new Choice(sequence);
			var testSubject = new ProductionAccumulator(fragments, _left, choice);

			// act
			IProduction production = testSubject.Build();

			Assert.NotNull(production);
			Assert.That(production.Left.Token, Is.EqualTo(_left.Token));
			Assert.That(production.Right.Sequences[0].Items[0].Primary, Is.EqualTo(_right));
			Assert.That(production.Right.Sequences[0].Items[1].Primary, Is.Not.Null);
			var second = production.Right.Sequences[0].Items[1].Primary as IChoice;
			Assert.NotNull(second);
			Assert.That(second.Sequences[0].Items[0].PrimaryAsSymbol(), Is.EqualTo(next));
			Assert.That(second.Sequences[1].Items[0].PrimaryAsSymbol(), Is.EqualTo(last));
			Assert.That(second.Sequences.Count, Is.EqualTo(2));
		}

		[Test]
		public void ToposortsFragments()
		{
			var lib = new FragmentLibrary();
			var fragments = new List<IFragment> {lib.A_B, lib.B_C, lib.C_D, lib.B_D};
			Assert.That(fragments.IndexOf(lib.A_B), Is.LessThan(fragments.IndexOf(lib.B_C)));
			Assert.That(fragments.IndexOf(lib.B_C), Is.LessThan(fragments.IndexOf(lib.C_D)));
			Assert.That(fragments.IndexOf(lib.A_B), Is.LessThan(fragments.IndexOf(lib.B_D)));
			var accumulator = new ProductionAccumulator(Enumerable.Reverse(fragments), _left, _rightChoice);

			// act
			List<IFragment> list = accumulator.Fragments.ToList();

			Assert.That(list.IndexOf(lib.A_B), Is.LessThan(list.IndexOf(lib.B_C)));
			Assert.That(list.IndexOf(lib.B_C), Is.LessThan(list.IndexOf(lib.C_D)));
			Assert.That(list.IndexOf(lib.A_B), Is.LessThan(list.IndexOf(lib.B_D)));
		}
	}
}
