#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Propter project on GitHub
#endregion

#region using...
using System.Collections.Generic;
#endregion

namespace Stile.Prototypes.Collections
{
    public class HashBucket<TKey, TItem> : Dictionary<TKey, IList<TItem>>
    {
        public void Add(TKey key, TItem item)
        {
            IList<TItem> list;
            if (TryGetValue(key, out list) == false)
            {
                list = new List<TItem> {item};
                Add(key, list);
            }
            else
            {
                list.Add(item);
            }
        }
    }
}
