#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Globalization;
using System.Linq.Expressions;
using Stile.Readability;
#endregion

namespace Stile.Types.Enums
{
	public static class Enumeration
	{
		public static ArgumentOutOfRangeException FailedToRecognize<TEnum>(Expression<Func<TEnum>> expression)
			where TEnum : struct
		{
			TEnum invoked = expression.Compile().Invoke();
			return new ArgumentOutOfRangeException(expression.Body.ToDebugString(),
				string.Format(CultureInfo.InvariantCulture,
					"Failed to recognize {0} value '{1}'.",
					typeof(TEnum).ToDebugString(),
					invoked));
		}

		public static bool IsDefined<TEnum>(TEnum value) where TEnum : struct
		{
			return Enum.IsDefined(typeof(TEnum), value);
		}
	}
}
