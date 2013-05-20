#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
#endregion

namespace Stile.Types.Equality
{
	public static class EqualityExtensions
	{
		public static bool EqualsOrIsEquallyNull<TItem>(this TItem item, TItem other)
		{
			if (ReferenceEquals(null, item))
			{
				return ReferenceEquals(null, other);
			}
			return item.Equals(other);
		}
	}
}
