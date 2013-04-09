#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
#endregion

namespace Stile.Prototypes.Collections
{
	public class HashBucket<TKey, TItem> : Dictionary<TKey, ISet<TItem>>
	{
		private readonly IEqualityComparer<TItem> _itemComparer;

		public HashBucket(IEqualityComparer<TKey> keyComparer = null, IEqualityComparer<TItem> itemComparer = null)
			: base(keyComparer)
		{
			_itemComparer = itemComparer;
		}

		public void Add(TKey key, TItem item)
		{
			ISet<TItem> set;
			if (TryGetValue(key, out set) == false)
			{
				set = new HashSet<TItem>(_itemComparer) {item};
				Add(key, set);
			}
			else
			{
				set.Add(item);
			}
		}

		public HashBucket<TKey, TItem> Concat(HashBucket<TKey, TItem> hashBucket)
		{
			foreach (KeyValuePair<TKey, ISet<TItem>> pair in hashBucket)
			{
				foreach (TItem item in pair.Value)
				{
					Add(pair.Key, item);
				}
			}
			return this;
		}
	}
}
