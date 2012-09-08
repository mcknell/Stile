using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Stile.Patterns.Traversal
{
	public class NonrepeatingTraverser<TItem> : Traverser<TItem>
	{
		private readonly HashSet<TItem> _visited;

		public NonrepeatingTraverser([NotNull] IEnumerable<TItem> items,
		                             [NotNull] Func<TItem, Move> accepter,
		                             Func<IEnumerable<TItem>, IEnumerable<TItem>> sequencer = null)
			: base(items, accepter, sequencer)
		{
			_visited = new HashSet<TItem>();
		}

		protected override bool AfterAccept(TItem item)
		{
			bool okay = _visited.Contains(item) == false;
			_visited.Add(item); // memorize
			return okay;
		}
	}
}