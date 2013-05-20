#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

namespace Stile.Types.Primitives
{
	public static class BoolExtensions
	{
		public static string ToNot(this bool maybe)
		{
			if (!maybe)
			{
				return "not";
			}
			return string.Empty;
		}
	}
}
