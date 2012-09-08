#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using System.Linq.Expressions;
using Stile.Types.Enums;
using Stile.Types.Expressions;
#endregion

namespace Stile.Validation
{
	public static class ValidateArgument
	{
		public static TEnum IsInEnum<TEnum>(Expression<Func<TEnum>> expression) where TEnum : struct
		{
			TEnum invoked = expression.Compile().Invoke();
			return IsInEnum(invoked, expression);
		}

		public static TEnum IsInEnum<TEnum>(TEnum arg, Expression<Func<TEnum>> expression) where TEnum : struct
		{
			if (Enumeration.IsDefined(arg) == false)
			{
				throw Enumeration.FailedToRecognize(expression);
			}
			return arg;
		}

		public static TArg IsNotDefault<TArg>(Expression<Func<TArg>> expression)
		{
			TArg invoked = expression.Compile().Invoke();
			Lazy<string> lazyDebugString = expression.Body.ToLazyDebugString();
			return IsNotDefault(invoked, lazyDebugString);
		}

		public static TArg IsNotDefault<TArg>(TArg arg, Lazy<string> argumentName)
		{
			if (typeof(TArg).IsValueType == false)
			{
				if (ReferenceEquals(arg, null))
				{
					throw new ArgumentNullException(argumentName.Value);
				}
			}
			else if (default(TArg).Equals(arg))
			{
				throw new ArgumentOutOfRangeException(argumentName.Value);
			}

			return arg;
		}
	}
}
