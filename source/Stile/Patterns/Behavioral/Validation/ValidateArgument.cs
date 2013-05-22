﻿#region License info...
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
		public static TArg IsNotNullOrEmpty<TArg, TItem>(this FluentEnumerableSource<TArg, TItem> builder)
			where TArg : class, IEnumerable<TItem>
		{
			TArg arg = builder.Arg;
			arg.ValidateArgumentIsNotNullOrEmpty<TArg, TItem>(builder.StackDepth + 1);
			return arg;
		}

		[ContractArgumentValidator]
		public static FluentValidationBuilder<TArg> Validate<TArg>(this TArg arg)
		{
			return new FluentValidationBuilder<TArg>(arg, 1);
		}

		[ContractArgumentValidator]
		[NotNull]
		[System.Diagnostics.Contracts.Pure]
		public static TArg ValidateArgumentIsNotNull<TArg>(this TArg arg) where TArg : class
		{
			return arg.ValidateArgumentIsNotNull(2);
		}

		[ContractArgumentValidator]
		public static string ValidateStringNotNullOrEmpty(this string arg)
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
		public static TArg ValidateStringNotNullOrEmpty<TArg>(this TArg arg) where TArg : class, IEnumerable
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

			paramName = "<failed>";
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

		private static TEnumerable ValidateArgumentIsNotNullOrEmpty<TEnumerable, TArg>(this TEnumerable arg,
			int stackDepth) where TEnumerable : class, IEnumerable<TArg>
		{
			arg.ValidateArgumentIsNotNull(stackDepth);
			if (arg.Any() == false)
			{
				string paramName;
				new StackTrace(true).TryGetParameterName<TEnumerable>(out paramName, stackDepth - 1);
				throw new ArgumentException(
					ErrorMessages.ValidateArgument_ValidateArgumentIsNotNullOrEmpty.InvariantFormat(paramName));
			}
			return arg;
		}

		public class FluentEnumerableSource<TArg, TItem>
		{
			public FluentEnumerableSource(TArg arg, int stackDepth)
			{
				Arg = arg;
				StackDepth = stackDepth;
				if (typeof(TItem) == typeof(string)) {}
			}

			public TArg Arg { get; private set; }
			public int StackDepth { get; private set; }
		}

		public class FluentValidationBuilder<TArg>
		{
			private readonly TArg _arg;
			private readonly int _stackDepth;

			public FluentValidationBuilder(TArg arg, int stackDepth)
			{
				_arg = arg;
				_stackDepth = stackDepth;
			}

			public FluentEnumerableSource<TArg, TItem> EnumerableOf<TItem>()
			{
				return new FluentEnumerableSource<TArg, TItem>(_arg, _stackDepth + 1);
			}
		}
	}
}
