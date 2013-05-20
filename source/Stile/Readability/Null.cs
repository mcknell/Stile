#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Types.Reflection;
#endregion

namespace Stile.Readability
{
	public static class Null
	{
		public static readonly string String = null;
		public static readonly Type Type = null;
		public static readonly int? Int = null;

		public static bool IsNullOrDefault<TItem>(this TItem item)
		{
			Type type = typeof(TItem);

			if (type.IsNullable())
			{
				return ReferenceEquals(null, item);
			}
			if (!type.IsValueType)
			{
				return ReferenceEquals(null, item);
			}
			return default(TItem).Equals(item);
		}
	}
}
