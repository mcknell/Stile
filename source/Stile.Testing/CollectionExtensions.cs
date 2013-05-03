#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
#endregion

namespace Stile.Testing
{
	public static class CollectionExtensions
	{
		public static ICollection<TItem> CollectionIdentity<TItem>(this ICollection<TItem> collection)
		{
			return collection;
		}
	}
}
