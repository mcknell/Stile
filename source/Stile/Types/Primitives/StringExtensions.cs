#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Globalization;
using JetBrains.Annotations;
#endregion

namespace Stile.Types.Primitives
{
	public static class StringExtensions
	{
		[StringFormatMethod("format")]
		public static string InvariantFormat(this string format, params object[] args)
		{
			return string.Format(CultureInfo.InvariantCulture, format, args);
		}

		public static bool IsNeitherNullNorEmpty(this string s)
		{
			return string.IsNullOrEmpty(s) == false;
		}
	}
}
