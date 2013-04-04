#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Readability;
#endregion

namespace Stile.Types.Enumerables
{
	public static class EnumerableExtensions
	{
		private static readonly MethodInfo ToDebugStringGenericMethodDefinition =
			((EnumerableLazyPrintMethod<int>) ToDebugString).Method.GetGenericMethodDefinition();

		public static bool None<TItem>(this IEnumerable<TItem> items)
		{
			return items.Any() == false;
		}

		[NotNull]
		[System.Diagnostics.Contracts.Pure]
		public static IEnumerable<TItem> SkipWith<TItem>([NotNull] this IEnumerable<TItem> items,
			[NotNull] Action<TItem> action,
			int count = 1)
		{
			IEnumerator<TItem> enumerator = items.ValidateArgumentIsNotNull().GetEnumerator();
			Action<TItem> validAction = action.ValidateArgumentIsNotNull();
			for (int i = 0; i < count; i++)
			{
				if (enumerator.MoveNext())
				{
					validAction.Invoke(enumerator.Current);
				}
				else
				{
					break;
				}
			}
			while (enumerator.MoveNext())
			{
				yield return enumerator.Current;
			}
		}

		[NotNull]
		[System.Diagnostics.Contracts.Pure]
		public static IEnumerable<Tuple<TItem, TItem>> ToAdjacentPairs<TItem>(
			[NotNull] this IEnumerable<TItem> items, Action<TItem> actionOnFirstItem = null)
		{
			TItem prior = default(TItem);
			Action<TItem> wrapper = item =>
			{
				prior = item;
				if (actionOnFirstItem != null)
				{
					actionOnFirstItem.Invoke(item);
				}
			};
			foreach (TItem item in items.SkipWith(wrapper))
			{
				yield return Tuple.Create(prior, item);
				prior = item;
			}
		}

		public static string ToDebugString<TItem>(this IEnumerable<TItem> enumerable,
			int lengthLimit = Default.LengthLimit)
		{
			if (lengthLimit < 0)
			{
				throw new ArgumentOutOfRangeException("lengthLimit");
			}
			if (enumerable == null)
			{
				return PrintExtensions.ReadableNull;
			}
			var sb = new StringBuilder();
			sb.Append(enumerable.GetType().ToDebugString());
			sb.Append(" {");
			IEnumerator<TItem> enumerator = enumerable.GetEnumerator();
			if (enumerator.MoveNext())
			{
				sb.Append(enumerator.Current.ToDebugString());
				while (enumerator.MoveNext() && sb.Length < lengthLimit)
				{
					sb.AppendFormat(", {0}", enumerator.Current.ToDebugString());
				}
			}
			else
			{
				sb.Append(PrintExtensions.ReadableEmpty);
			}
			sb.Append("}");
			return sb.ToString();
		}

		public static string ToDebugString(object enumerable, Type itemType)
		{
			MethodInfo genericMethod = ToDebugStringGenericMethodDefinition.MakeGenericMethod(itemType);
			object invoked = genericMethod.Invoke(null, new[] {enumerable, Default.LengthLimit});
			return (string) invoked;
		}

		public static Lazy<string> ToLazyDebugString<TItem>(this IEnumerable<TItem> enumerable)
		{
			return new Lazy<string>(() => enumerable.ToDebugString());
		}

		public static Lazy<string> ToLazyDebugString(object enumerable, Type itemType)
		{
			return new Lazy<string>(() => ToDebugString(enumerable, itemType));
		}

		/// <summary>
		/// Returns a sequence of '<paramref name="first"/>' followed by optional '<paramref name="others"/>' 
		/// and finally the original sequence.
		/// </summary>
		/// <typeparam name="TItem"></typeparam>
		/// <param name="enumerable"></param>
		/// <param name="first"></param>
		/// <param name="others"></param>
		/// <returns></returns>
		public static IEnumerable<TItem> Unshift<TItem>(this IEnumerable<TItem> enumerable,
			TItem first,
			params TItem[] others)
		{
			yield return first;
			foreach (TItem other in others)
			{
				yield return other;
			}
			foreach (TItem item in enumerable)
			{
				yield return item;
			}
		}

		public class Default
		{
			public const int LengthLimit = 400;
		}

		private delegate string EnumerableLazyPrintMethod<in TItem>(IEnumerable<TItem> enumerable, int lengthlimit);
	}
}
