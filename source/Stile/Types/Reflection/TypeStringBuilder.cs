#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq;
using System.Text;
using Stile.Readability;
using Nullable = System.Nullable;
#endregion

namespace Stile.Types.Reflection
{
	public class TypeStringBuilder
	{
		public const string GenericArgumentDelimiter = "`";
		private static readonly Lazy<string> LazyNull = new Lazy<string>(() => PrintExtensions.ReadableNullString);
		private readonly Lazy<string> _lazy;

		public TypeStringBuilder(Type type)
		{
			if (type == null)
			{
				_lazy = LazyNull;
			}
			else
			{
				_lazy = new Lazy<string>(() => Print(type));
			}
		}

		public override string ToString()
		{
			string value = _lazy.Value;
			return value;
		}

		private static void AppendCommas(StringBuilder sb, int length)
		{
			if (length > 0)
			{
				sb.Append(new string(',', length - 1));
			}
		}

		private static string GetName(Type type)
		{
			string name;
			if (!CSharp4Types.TypeAliases.TryGetValue(type, out name))
			{
				name = type.Name;
			}
			return name;
		}

		private static string Print(Type type)
		{
			var stringBuilder = new StringBuilder();
			Print(type, stringBuilder);
			return stringBuilder.ToString();
		}

		private static void Print(Type type, StringBuilder sb)
		{
			Type underlyingType = Nullable.GetUnderlyingType(type);
			if (underlyingType != null)
			{
				Print(underlyingType, sb);
				sb.Append("?");
				return;
			}

			if (type.IsArray)
			{
				Print(type.GetElementType(), sb);
				sb.Append("[");
				AppendCommas(sb, type.GetArrayRank());
				sb.Append("]");
				return;
			}

			if (type.IsGenericType)
			{
				int apostrophePosition = type.Name.IndexOf(GenericArgumentDelimiter, StringComparison.Ordinal);
				sb.Append(type.Name.Substring(0, apostrophePosition));
				Type[] genericArguments = type.GetGenericArguments();
				sb.Append("<");
				if (type.IsGenericTypeDefinition)
				{
					AppendCommas(sb, genericArguments.Length);
				}
				else
				{
					Type first = genericArguments.First();
					Print(first, sb);
					foreach (Type genericArgument in genericArguments.Skip(1))
					{
						sb.Append(", ");
						Print(genericArgument, sb);
					}
				}
				sb.Append(">");
			}
			else
			{
				string name = GetName(type);
				sb.Append(name);
			}
		}
	}
}
