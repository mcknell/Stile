#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public class ProductionConsolidator
	{
		private readonly IChoice _choice;

		public ProductionConsolidator(IChoice choice)
		{
			_choice = choice;
		}

		public IChoice Consolidate()
		{
			int priorCount = int.MaxValue;
			IChoice consolidated = _choice;
			while (priorCount > consolidated.Count)
			{
				priorCount = consolidated.Count;
				consolidated = Consolidate(consolidated);
			}
			return consolidated;
		}

		private IChoice Consolidate(IChoice choice)
		{
			List<ISequence> sequences = choice.Sequences.Select(Consolidate).ToList();
			var consolidated = new Choice(sequences);
			return consolidated;
		}

		private IList<IItem> Consolidate(IItem item)
		{
			var choice = item.Primary as IChoice;
			if (choice == null)
			{
				return new[] {item};
			}
			int minSequenceLength = choice.Sequences.Min(x => x.Items.Count);
			var stack = new Stack<IItem>();
			while (true)
			{
				if (stack.Count == minSequenceLength)
				{
					break;
				}
				if (stack.Count > minSequenceLength)
				{
					throw new Exception();
				}
				int offset = stack.Count + 1;
				List<IItem> distinct =
					choice.Sequences.Select(x => x.Items.ElementAt(x.Items.Count - offset)).Distinct().ToList();
				if (distinct.Count == 1)
				{
					stack.Push(distinct[0]);
				}
				else
				{
					break;
				}
			}
			var list = new List<IItem>();
			if (stack.Any())
			{
				List<ISequence> sequences =
					choice.Sequences.Select(x => new Sequence(x.Take(x.Items.Count - stack.Count)))
						.Where(x => x.Items.Any())
						.Select(Consolidate)
						.ToList();
				if (sequences.Count == 1)
				{
					list.AddRange(sequences[0]);
				}
				else
				{
					list.Add(new Item(new Choice(sequences), item.Cardinality));
				}
				List<IItem> items = stack.SelectMany(Consolidate).ToList();
				list.AddRange(items);
			}
			else
			{
				list.Add(new Item(Consolidate(choice), item.Cardinality));
			}
			List<IItem> flattened = list.SelectMany(x => x.Flatten()).ToList();
			return flattened;
		}

		private ISequence Consolidate(ISequence sequence)
		{
			List<IItem> items = sequence.Items.SelectMany(Consolidate).ToList();
			var consolidated = new Sequence(items);
			return consolidated;
		}
	}
}
