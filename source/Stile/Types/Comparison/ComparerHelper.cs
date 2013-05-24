#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
#endregion

namespace Stile.Types.Comparison
{
	public static class ComparerHelper
	{
		public static IComparer<TItem> MakeComparer<TItem>([NotNull] this Func<TItem, TItem, int> comparer)
			where TItem : IComparable<TItem>
		{
			return new Comparr<TItem>(comparer);
		}

		private class Comparr<TItem> : Comparer<TItem>
			where TItem : IComparable<TItem>
		{
			private readonly Func<TItem, TItem, int> _comparer;

			public Comparr(Func<TItem, TItem, int> comparer)
			{
				_comparer = comparer;
			}

			public override int Compare(TItem x, TItem y)
			{
				if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
				{
					return 0;
				}
				return _comparer(x, y);
			}
		}
	}
}
