#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Collections.Sorting;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public class ProductionAccumulator
	{
		private readonly List<IFragment> _fragments;
		private readonly NonterminalSymbol _left;
		private readonly IChoice _right;

		public ProductionAccumulator(IEnumerable<IFragment> fragments, NonterminalSymbol left, IChoice right)
		{
			_left = left.ValidateArgumentIsNotNull();
			_right = right.ValidateArgumentIsNotNull();
			List<IFragment> list = fragments.ToList();
			list.AddRange(_right.Fragments());
			list.Sort(Fragment.Comparer);
			var toposort = new Toposort<IFragment>(list,
				x => list.Where(y => y.Right.Token == x.Left && x.Equals(y) == false).ToList(),
				Fragment.EqualityComparer);
			_fragments = new List<IFragment>(toposort.SortDag());
			Debug.Assert(list.Count == _fragments.Count);
		}

		public IReadOnlyList<IFragment> Fragments
		{
			get { return _fragments; }
		}

		public IProduction Build()
		{
			List<ISequence> sequences = _right.Sequences.Select(Clone).ToList();
			var choice = new Choice(sequences);
			return new Production(_left, choice);
		}

		private IChoice Clone(IChoice choice)
		{
			return new Choice(choice.Sequences.Select(Clone));
		}

		private ISequence Clone(ISequence sequence)
		{
			IItem first = sequence.Items.First();
			var items = new List<IItem> {first};
			List<IItem> successors = FindSuccessors(first);
			items.AddRange(Collect(successors));
			return new Sequence(items);
		}

		private IItem Clone(IItem item)
		{
			IPrimary primary;
			var nonterminal = item.Primary as NonterminalSymbol;
			if (nonterminal != null)
			{
				primary = nonterminal;
			}
			else
			{
				var terminal = item.Primary as StringLiteral;
				if (terminal != null)
				{
					primary = terminal;
				}
				else
				{
					primary = Clone((IChoice) item.Primary);
				}
			}
			return new Item(primary, item.Cardinality);
		}

		private IEnumerable<IItem> Collect(IList<IItem> alternatives)
		{
			switch (alternatives.Count)
			{
				case 0:
					return alternatives;
				case 1:
					return alternatives[0].Flatten();
				default:
					// collect the alternatives as a choice
					IEnumerable<Sequence> sequences = alternatives.Select(x => new Sequence(x));
					var choice = new Choice(sequences);
					return new[] { new Item(choice) }; 
			}
		}

		private List<IItem> FindSuccessors(IItem prior, IItem current = null)
		{
			var list = new List<IItem>();
			if (prior != null)
				// find all the alternatives given by the fragments
			{
				Symbol priorSymbol = prior.PrimaryAsSymbol();
				if (priorSymbol != null)
				{
					Symbol currentSymbol = null;
					if (current != null)
					{
						currentSymbol = current.PrimaryAsSymbol();
					}
					foreach (
						IFragment fragment in _fragments.Where(x => x.Left == priorSymbol.Token && x.Right != currentSymbol))
					{
						IItem clonedRight = Clone(fragment.RightAsItem());
						List<IItem> successors = FindSuccessors(clonedRight);
						switch (successors.Count)
						{
							case 0:
								list.Add(clonedRight);
								break;
							case 1:
								IEnumerable<IItem> items = successors[0].Flatten();
								list.Add(new Item(new Choice(new Sequence(items.Unshift(clonedRight)))));
								break;
							default:
								var alternatives = new Item(new Choice(successors.Select(x => new Sequence(x))));
								list.Add(new Item(new Choice(new Sequence(clonedRight, alternatives))));
								break;
						}
					}
				}
			}
			return list;
		}
	}
}
