#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Grammar;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public class ProductionAccumulator
	{
		private readonly HashSet<IFragment> _fragments;
		private readonly NonterminalSymbol _left;
		private readonly IChoice _right;
		private bool _reentrant;

		public ProductionAccumulator(IEnumerable<IFragment> fragments, NonterminalSymbol left, IChoice right)
		{
			_left = left.ValidateArgumentIsNotNull();
			_right = right.ValidateArgumentIsNotNull();
			_fragments = new HashSet<IFragment>(fragments);
			_reentrant = false;
		}

		public IProduction Build()
		{
			var sequencesToPrepare = new List<ISequence>();
			sequencesToPrepare.AddRange(_right.Sequences);

			foreach (IFragment fragment in _fragments.Where(x => x.Left == _left.Token))
			{
				IFragment copy = fragment;
				if (_right.Sequences.None(x => x.FirstSymbol().Token == copy.Right.Token))
					// if no sequence represents this fragment
				{
					sequencesToPrepare.Add(new Sequence(new Item(new Nonterminal(copy.Right.Token))));
				}
			}
			var sequences = sequencesToPrepare.Select(Prepare);
			return new Production(_left, new Choice(sequences));
		}

		private IChoice Prepare(IChoice choice)
		{
			if (_reentrant == false)
			{
				_reentrant = true;
				var sequences = new List<ISequence>();
				foreach (ISequence sequence in choice.Sequences)
				{
					sequences.Add(Prepare(sequence));
				}
				return new Choice(sequences);
			}
			return choice;
		}

		private ISequence Prepare(ISequence sequence)
		{
			throw new NotImplementedException();
		}
	}
}
