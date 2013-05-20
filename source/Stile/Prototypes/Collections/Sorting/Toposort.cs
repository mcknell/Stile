#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Collections.Sorting
{
	/// <summary>
	/// <seealso cref="http://en.wikipedia.org/wiki/Topological_sorting"/>
	/// </summary>
	/// <typeparam name="TItem"></typeparam>
	public class Toposort<TItem>
	{
		private readonly Func<TItem, IEnumerable<TItem>> _adjacencyFunction;
		private readonly IEqualityComparer<TItem> _comparer;
		private readonly IEnumerable<TItem> _items;
		private readonly HashSet<TItem> _permanentlyMarked;
		private readonly HashSet<TItem> _temporarilyMarked;
		private HashSet<TItem> _unmarked;

		public Toposort([NotNull] IEnumerable<TItem> items,
			[NotNull] Func<TItem, IEnumerable<TItem>> adjacencyFunction,
			IEqualityComparer<TItem> comparer = null)
		{
			_items = items.ValidateArgumentIsNotNull();
			_adjacencyFunction = adjacencyFunction.ValidateArgumentIsNotNull();
			_comparer = comparer;

			_temporarilyMarked = new HashSet<TItem>(_comparer);
			_permanentlyMarked = new HashSet<TItem>(_comparer);
		}

		public IEnumerable<TItem> SortDag()
		{
			_unmarked = new HashSet<TItem>(_items);
			while (_unmarked.Any())
			{
				TItem item = _unmarked.First();
				foreach (TItem visited in Visit(item))
				{
					yield return visited;
				}
			}
		}

		private IEnumerable<TItem> Visit(TItem item)
		{
			if (_temporarilyMarked.Contains(item))
			{
				throw new Exception(string.Format("Cycle detected at item {0}.", item));
			}
			if (_permanentlyMarked.Contains(item) == false)
			{
				_temporarilyMarked.Add(item);
				IEnumerable<TItem> neighbors = _adjacencyFunction.Invoke(item);
				foreach (TItem neighbor in neighbors)
				{
					foreach (TItem nextNeighbor in Visit(neighbor))
					{
						yield return nextNeighbor;
					}
					yield return neighbor;
				}
				_temporarilyMarked.Remove(item);
				_permanentlyMarked.Add(item);
				_unmarked.Remove(item);
				yield return item;
			}
		}
	}
}
