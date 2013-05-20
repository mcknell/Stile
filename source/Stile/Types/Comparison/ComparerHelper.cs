#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Types.Comparison
{
	public static class ComparerHelper
	{
		public static IEqualityComparer<TItem> MakeComparer<TItem>([NotNull] this Func<TItem, TItem, bool> comparer)
		{
			return new Comparer<TItem>(comparer);
		}

		private class Comparer<TItem> : EqualityComparer<TItem>
		{
			private readonly Func<TItem, TItem, bool> _comparer;

			public Comparer([NotNull] Func<TItem, TItem, bool> comparer)
			{
				_comparer = comparer.ValidateArgumentIsNotNull();
			}

			public override bool Equals(TItem x, TItem y)
			{
				return _comparer(x, y);
			}

			public override int GetHashCode(TItem obj)
			{
				if (ReferenceEquals(null, obj))
				{
					return 0;
				}
				return obj.GetHashCode();
			}
		}
	}
}
