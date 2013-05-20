#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
#endregion

namespace Stile.Types
{
	public static class LazyExtensions
	{
		public static Lazy<TValue> ToLazy<TValue>(this TValue value)
		{
			return new Lazy<TValue>(() => value);
		}

		public static Lazy<TValue> ToLazy<TValue>(this Func<TValue> func)
		{
			return new Lazy<TValue>(func);
		}
	}
}
