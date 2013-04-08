#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
#endregion

namespace Stile.Patterns.Behavioral.Traversal
{
	public class NonrepeatingTraverser<TItem> : Traverser<TItem>
	{
		private readonly HashSet<TItem> _visited;

		public NonrepeatingTraverser([NotNull] IEnumerable<TItem> items,
			[NotNull] Func<TItem, Move> accepter,
			Func<IEnumerable<TItem>, IEnumerable<TItem>> sequencer = null,
			IEqualityComparer<TItem> equalityComparer = null)
			: base(items, accepter, sequencer)
		{
			_visited = new HashSet<TItem>(equalityComparer);
		}

		protected override bool AfterAccept(TItem item)
		{
			bool okay = _visited.Contains(item) == false;
			_visited.Add(item); // memorize
			return okay;
		}
	}
}
