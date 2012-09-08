#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using System.Collections.Generic;
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
        public const string ReadableEmpty = "<empty>";
        public const string ReadableNull = "<null>";
        public const string ReadableNullString = @"""<null>""";

        public static string Pluralize(this int count, string singular, string plural = null)
        {
            if (count == 1)
            {
                return singular;
            }
            if (string.IsNullOrWhiteSpace(plural))
            {
                return string.Format("{0}s", singular);
            }
            return plural;
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
                var sb = new StringBuilder(s);
                sb.Replace("\n", "\\n");
                sb.Replace("\r", "\\r");
                sb.Replace("\t", "\\t");
                sb.Insert(0, "\"");
                sb.Append("\"");
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
                return "false";
            }
            if (true.Equals(obj))
            {
                return "true";
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
            return new Lazy<string>(() => obj.ToDebugString());
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
