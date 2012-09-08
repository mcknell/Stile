#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
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
