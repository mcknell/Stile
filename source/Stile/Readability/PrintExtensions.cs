#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Stile.Types;
using Stile.Types.Enumerables;
using Stile.Types.Expressions;
using Stile.Types.Reflection;
#endregion

namespace Stile.Readability
{
	public static class PrintExtensions
	{
		public static readonly string ReadableNull = LocalizableStrings.PrintExtensions_ReadableNull;
		public static readonly string ReadableNullString = LocalizableStrings.PrintExtensions_ReadableNullString;

		public static string CurrentFormat(this string format, params object[] args)
		{
			return string.Format(CultureInfo.CurrentCulture, format, args);
		}

		public static string Pluralize(this int count, string singular, string plural = null)
		{
			if (count == 1)
			{
				return singular;
			}
			if (string.IsNullOrWhiteSpace(plural))
			{
				return LocalizableStrings.PrintExtensions_Pluralize.CurrentFormat(singular);
			}
			return plural;
		}

		public static string Pluralize(this long count, string singular, string plural = null)
		{
			return Pluralize((int) count, singular, plural);
		}

		public static string ToDebugString<TObject>(this TObject obj)
		{
			var s = obj as string;
			if (typeof(TObject) == typeof(string) || s != null)
			{
				if (s == null)
				{
					return ReadableNullString;
				}
				var sb = new StringBuilder(LocalizableStrings.PrintExtensions_ToDebugString_OpenQuote);
				sb.Append(s);
				sb.Replace("\n", LocalizableStrings.PrintExtensions_ToDebugString_ReplaceN);
				sb.Replace("\r", LocalizableStrings.PrintExtensions_ToDebugString_ReplaceR);
				sb.Replace("\t", LocalizableStrings.PrintExtensions_ToDebugString_ReplaceT);
				sb.Append(LocalizableStrings.PrintExtensions_ToDebugString_CloseQuote);
				return sb.ToString();
			}
			if (ReferenceEquals(null, obj))
			{
				return ReadableNull;
			}
			var asType = obj as Type;
			if (asType != null)
			{
				return TypeExtensions.ToDebugString(asType);
			}
			if (false.Equals(obj))
			{
				return LocalizableStrings.PrintExtensions_ToDebugString_False;
			}
			if (true.Equals(obj))
			{
				return LocalizableStrings.PrintExtensions_ToDebugString_True;
			}
			Type type = obj.GetType();
			if (type.IsArray)
			{
				return ArrayExtensions.ArrayToDebugString(obj);
			}
			Type implementer;
			if (type.Implements(typeof(IEnumerable<>), out implementer))
			{
				Type firstGenericArgument = implementer.GetGenericArguments().First();
				s = EnumerableExtensions.ToDebugString(obj, firstGenericArgument);
				return s;
			}
			var expression = obj as Expression;
			if (expression != null)
			{
				return ExpressionExtensions.ToDebugString(expression);
			}
			return obj.ToString();
		}

		public static Lazy<string> ToLazyDebugString(this object obj)
		{
			return new Lazy<string>(obj.ToDebugString);
		}

		public static Lazy<string> ToLazyDebugString<TItem>(this Lazy<TItem> lazy)
		{
			if (ReferenceEquals(null, lazy))
			{
				return ReadableNullString.ToLazy();
			}
			return new Lazy<string>(() => lazy.Value.ToDebugString());
		}
	}
}
