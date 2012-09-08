#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
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
		public static ArgumentOutOfRangeException FailedToRecognize<TEnum>(Expression<Func<TEnum>> expression) where TEnum : struct
		{
			TEnum invoked = expression.Compile().Invoke();
			return new ArgumentOutOfRangeException(expression.Body.ToDebugString(),
				string.Format(CultureInfo.InvariantCulture, "Failed to recognize {0} value '{1}'.", typeof(TEnum).ToDebugString(), invoked));
		}

		public static bool IsDefined<TEnum>(TEnum value) where TEnum : struct
		{
			return Enum.IsDefined(typeof(TEnum), value);
		}
	}
}
