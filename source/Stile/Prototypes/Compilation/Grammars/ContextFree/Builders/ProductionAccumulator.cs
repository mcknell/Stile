#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Linq;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Collections.Sorting;
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
			_fragments = new List<IFragment>(fragments);
			var toposort = new Toposort<IFragment>(_fragments,
				x => _fragments.Where(y => x.Right.Token == y.Left),
				Fragment.EqualityComparer);
			_fragments = new List<IFragment>(toposort.SortDag());
			_fragments.Sort(Fragment.Comparer);
		}

		public IReadOnlyList<IFragment> Fragments
		{
			get { return _fragments; }
		}

		public IProduction Build()
		{
			IEnumerable<ISequence> sequences = _right.Sequences.Select(Clone);
			var choice = new Choice(sequences);
			return new Production(_left, choice);
		}

		private List<IItem> AlternativesTo(IItem prior, IItem current = null)
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
						list.Add(clonedRight);
					}
				}
			}
			return list;
		}

		private IChoice Clone(IChoice choice)
		{
			return new Choice(choice.Sequences.Select(Clone));
		}

		private ISequence Clone(ISequence sequence)
		{
			var items = new List<IItem>();
			IItem prior = null;
			foreach (IItem item in sequence.Items)
			{
				IItem clone = Clone(item);
				var alternatives = new List<IItem> {clone};
				alternatives.AddRange(AlternativesTo(prior, clone));
				items.Add(Collect(alternatives));
				// setup next
				prior = item;
			}
			List<IItem> list = AlternativesTo(prior).ToList();
			if (list.Any())
			{
				items.Add(Collect(list));
			}
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

		private IItem Collect(IList<IItem> alternatives)
		{
			if (alternatives.Count > 1)
				// collect the alternatives as a choice
			{
				IEnumerable<Sequence> sequences = alternatives.Select(x => new Sequence(x));
				var choice = new Choice(sequences);
				return new Item(choice);
			}
			return alternatives[0];
		}
	}
}
