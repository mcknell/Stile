#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using System.Text;
using Stile.Readability;
using Stile.Types.Reflection;
#endregion

namespace Stile.Types.Enumerables
{
    public static class ArrayExtensions
    {
        public static string ArrayToDebugString(object array)
        {
            Type type = array.GetType();
            var sb = new StringBuilder();
            sb.Append(type.GetElementType().ToDebugString());
            sb.Append("[");
            int arrayRank = type.GetArrayRank();
            var asArrayProper = ((Array) array);
            if (asArrayProper.Length == 0)
            {
                sb.Append("0]");
            }
            else
            {
                sb.Append(asArrayProper.GetLength(0));
                for (int rank = 1; rank < arrayRank; rank++)
                {
                    sb.AppendFormat(",{0}", asArrayProper.GetLength(rank));
                }
                sb.Append("] {");
                sb.Append(asArrayProper.GetValue(0).ToDebugString());
                for (int i = 1; i < asArrayProper.Length; i++)
                {
                    sb.Append(", ");
                    sb.Append(asArrayProper.GetValue(i).ToDebugString());
                }
                sb.Append("}");
            }
            return sb.ToString();
        }

        public static Lazy<string> ArrayToLazyDebugString(object array)
        {
            return new Lazy<string>(() => ArrayToDebugString(array));
        }

        public static Lazy<string> ToDebugString<TItem>(this TItem[] array)
        {
            return ArrayToLazyDebugString(array);
        }
    }
}
