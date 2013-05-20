#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Collections.ObjectModel;
#endregion

namespace Stile.Types.Enumerables
{
	public static class ListExtensions
	{
		public static ReadOnlyCollection<TItem> ToReadOnly<TItem>(this IList<TItem> list)
		{
			return new ReadOnlyCollection<TItem>(list);
		}
	}
}
