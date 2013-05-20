#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
#endregion

namespace Stile.Readability
{
	public static class Identity
	{
		public const string Format = "{0}";

		public static Func<TItem, TItem> Map<TItem>()
		{
			return x => x;
		}
	}
}
