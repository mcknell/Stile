#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Stile.Types.Enums;
using Stile.Types.Expressions;
using Stile.Types.Primitives;
#endregion

namespace Stile.Patterns.Behavioral.Validation
{
	public static class ValidateArgument
	{
		[ContractArgumentValidator]
		public static TEnum IsInEnum<TEnum>(Expression<Func<TEnum>> expression) where TEnum : struct
		{
			TEnum invoked = expression.Compile().Invoke();
			return IsInEnum(invoked, expression);
		}

		[ContractArgumentValidator]
		public static TEnum IsInEnum<TEnum>(TEnum arg, Expression<Func<TEnum>> expression) where TEnum : struct
		{
			if (Enumeration.IsDefined(arg) == false)
			{
				throw Enumeration.FailedToRecognize(expression);
			}
			return arg;
		}

		[ContractArgumentValidator]
		public static TArg IsNotDefault<TArg>(Expression<Func<TArg>> expression)
		{
			TArg invoked = expression.Compile().Invoke();
			Lazy<string> lazyDebugString = expression.Body.ToLazyDebugString();
			return IsNotDefault(invoked, lazyDebugString);
		}

		[ContractArgumentValidator]
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

		[ContractArgumentValidator]
		[NotNull]
		[System.Diagnostics.Contracts.Pure]
		public static TArg ValidateArgumentIsNotNull<TArg>(this TArg arg) where TArg : class
		{
			return arg.ValidateArgumentIsNotNull(2);
		}

		[ContractArgumentValidator]
		[NotNull]
		[System.Diagnostics.Contracts.Pure]
		public static TArg ValidateArgumentIsNotNullOrEmpty<TArg>(this TArg arg) where TArg : class, IEnumerable
		{
			arg.ValidateArgumentIsNotNull(2);
			if (arg.GetEnumerator().MoveNext() == false)
			{
				string paramName;
				new StackTrace(true).TryGetParameterName<TArg>(out paramName, 1);
				throw new ArgumentException(
					ErrorMessages.ValidateArgument_ValidateArgumentIsNotNullOrEmpty.InvariantFormat(paramName));
			}
			return arg;
		}

		[ContractArgumentValidator]
		public static string ValidateIsNotNullOrWhitespace(this string arg)
		{
			if (string.IsNullOrWhiteSpace(arg))
			{
				string paramName;
				if (new StackTrace(true).TryGetParameterName<string>(out paramName, 1) == false)
				{
					paramName =
						ErrorMessages.ValidateArgument_ValidateArgumentIsNotNullOrEmpty.InvariantFormat(typeof(string).Name);
				}
				throw new ArgumentNullException(paramName);
			}
			return arg;
		}

		[ContractArgumentValidator]
		public static string ValidateNotNullOrEmpty(this string arg)
		{
			if (string.IsNullOrEmpty(arg))
			{
				string paramName;
				if (new StackTrace(true).TryGetParameterName<string>(out paramName, 1) == false)
				{
					paramName =
						ErrorMessages.ValidateArgument_ValidateArgumentIsNotNullOrEmpty.InvariantFormat(typeof(string).Name);
				}
				throw new ArgumentNullException(paramName);
			}
			return arg;
		}

		[ContractArgumentValidator]
		public static TArg ValidateNotNullOrEmpty<TArg>(this TArg arg) where TArg : class, IEnumerable
		{
			if (arg.ValidateArgumentIsNotNull(2) != null)
			{
				if (arg.GetEnumerator().MoveNext() == false)
				{
					string paramName;
					if (new StackTrace(true).TryGetParameterName<TArg>(out paramName, 1) == false)
					{
						paramName =
							ErrorMessages.ValidateArgument_ValidateArgumentIsNotNullOrEmpty.InvariantFormat(typeof(TArg).Name);
					}
					throw new ArgumentNullException(paramName);
				}
			}
			return arg;
		}

		private static string FormatParameterInfos(this List<ParameterInfo> list)
		{
			IEnumerable<string> strings =
				list.Select(
					x => ErrorMessages.ValidateArgument_TryGetParameterName_ParameterFormat.InvariantFormat(x.Name));
			string joined = string.Join(ErrorMessages.ValidateArgument_TryGetParameterName_Delimiter, strings);
			return ErrorMessages.ValidateArgument_TryGetParameterName_OneOf.InvariantFormat(joined);
		}

		private static bool TryGetParameterName<TArg>(this StackTrace stackTrace,
			out string paramName,
			int stackOffset = 2)
		{
			StackFrame stackFrame = stackTrace.GetFrame(stackOffset);
			MethodBase method = stackFrame.GetMethod();
			ParameterInfo[] parameters = method.GetParameters();
			if (parameters.Length == 1)
			{
				paramName = parameters[0].Name;
				return true;
			}

			Type type = typeof(TArg);
			List<ParameterInfo> matchingTypes = parameters.Where(x => x.ParameterType == type).ToList();
			int count = matchingTypes.Count;
			switch (count)
			{
				case 1:
					paramName = matchingTypes.First().Name;
					return true;
				case 0:
					break;
				default: // more than 1 match on type
					paramName = matchingTypes.FormatParameterInfos();
					int lineNumber = stackFrame.GetFileLineNumber();
					if (lineNumber > 0)
					{
						paramName = paramName
							+ ErrorMessages.ValidateArgument_TryGetParameterName_ValidateAtLine.InvariantFormat(lineNumber);
						int columnNumber = stackFrame.GetFileColumnNumber();
						if (columnNumber > 0)
						{
							paramName = paramName
								+ ErrorMessages.ValidateArgument_TryGetParameterName_Column.InvariantFormat(columnNumber);
						}
						return true;
					}
					return true;
			}

			paramName = "<failed: could not determine a parameter>";
			return false;
		}

		private static TArg ValidateArgumentIsNotNull<TArg>(this TArg arg, int stackOffset) where TArg : class
		{
			if (ReferenceEquals(null, arg))
			{
				string paramName;
				if (new StackTrace(true).TryGetParameterName<TArg>(out paramName, stackOffset) == false)
				{
					paramName = ErrorMessages.ValidateArgument_ValidateArgumentIsNotNull.InvariantFormat(typeof(TArg).Name);
				}
				throw new ArgumentNullException(paramName);
			}
			return arg;
		}
	}
}
