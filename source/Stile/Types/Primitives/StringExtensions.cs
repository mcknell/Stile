#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

using System.Globalization;
using JetBrains.Annotations;

namespace Stile.Types.Primitives
{
	public static class StringExtensions
	{
		public static bool IsNeitherNullNorEmpty(this string s)
		{
			return string.IsNullOrEmpty(s) == false;
		}

		[StringFormatMethod("format")]
		public static string InvariantFormat(this string format, params object[] args)
		{
			return string.Format(CultureInfo.InvariantCulture, format, args);
		}
	}
}
