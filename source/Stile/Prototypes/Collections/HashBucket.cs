#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Collections
{
	public interface IReadOnlyHashBucket<TKey, TItem> : IReadOnlyDictionary<TKey, ISet<TItem>> {}

	public class HashBucket<TKey, TItem> : Dictionary<TKey, ISet<TItem>>,
		IReadOnlyHashBucket<TKey, TItem>
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

	public static class HashBucketExtensions
	{
		public static HashBucket<TKey, TValue> ToHashBucket<TItem, TKey, TValue>(
			[NotNull] this IEnumerable<TItem> items,
			[NotNull] Func<TItem, TKey> keySelector,
			[NotNull] Func<TItem, TValue> valueSelector,
			IEqualityComparer<TKey> keyComparer = null,
			IEqualityComparer<TValue> itemComparer = null)
		{
			var hashBucket = new HashBucket<TKey, TValue>(keyComparer, itemComparer);
			foreach (TItem item in items)
			{
				hashBucket.Add(keySelector.Invoke(item), valueSelector.Invoke(item));
			}
			return hashBucket;
		}
	}
}
