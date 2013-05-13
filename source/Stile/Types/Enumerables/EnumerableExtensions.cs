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

		[NotNull]
		public static IEnumerable<TItem> Append<TItem>([NotNull] this IEnumerable<TItem> items,
			TItem item,
			params TItem[] others)
		{
			return items.Concat(others.Unshift(item));
		}

		[NotNull]
		public static IEnumerable<IList<TItem>> ForAll<TItem>([NotNull] this IEnumerable<TItem> first,
			[NotNull] IEnumerable<TItem> second,
			params IEnumerable<TItem>[] others)
		{
			List<IEnumerable<TItem>> sequences =
				others.Unshift(first.ValidateArgumentIsNotNull(), second.ValidateArgumentIsNotNull()).ToList();
			List<IEnumerator<TItem>> enumerators = sequences.Select(x => x.GetEnumerator()).ToList();
			while (enumerators.All(x => x.MoveNext()))
			{
				yield return enumerators.Select(x => x.Current).ToList();
			}
		}

		[NotNull]
		public static IEnumerable<IList<TItem>> ForAll<TItem>(
			[NotNull] this IEnumerable<IEnumerable<TItem>> sequences)
		{
			List<IEnumerator<TItem>> enumerators = sequences.Select(x => x.GetEnumerator()).ToList();
			while (enumerators.All(x => x.MoveNext()))
			{
				yield return enumerators.Select(x => x.Current).ToList();
			}
		}

		[NotNull]
		public static IEnumerable<TItem> Interlace<TItem>([NotNull] this IEnumerable<TItem> items, TItem item)
		{
			IEnumerator<TItem> enumerator = items.GetEnumerator();
			if (enumerator.MoveNext())
			{
				yield return enumerator.Current;
				while (enumerator.MoveNext())
				{
					yield return item;
					yield return enumerator.Current;
				}
			}
		}

		public static bool None<TItem>(this IEnumerable<TItem> items)
		{
			return items.Any() == false;
		}

		public static bool None<TItem>(this IEnumerable<TItem> items, Func<TItem, bool> predicate)
		{
			return items.Any(predicate) == false;
		}

		[System.Diagnostics.Contracts.Pure]
		public static int SequenceEquals<TItem>([NotNull] this IEnumerable<TItem> left,
			[NotNull] IEnumerable<TItem> right,
			params IEnumerable<TItem>[] others)
		{
			return SequenceEquals(left, right, null, others);
		}

		[System.Diagnostics.Contracts.Pure]
		public static int SequenceEquals<TItem>([NotNull] this IEnumerable<TItem> left,
			[NotNull] IEnumerable<TItem> right,
			[CanBeNull] IEqualityComparer<TItem> comparer,
			params IEnumerable<TItem>[] others)
		{
			comparer = comparer ?? EqualityComparer<TItem>.Default;
			IEnumerable<IEnumerable<TItem>> cohort = others.Unshift(left.ValidateArgumentIsNotNull(),
				right.ValidateArgumentIsNotNull());
			List<IEnumerator<TItem>> enumerators = cohort.Select(x => x.GetEnumerator()).ToList();
			int cohortSize = enumerators.Count;
			int firstDifference = 0;
			int canMove = enumerators.Count(x => x.MoveNext());
			if (canMove == 0)
			{
				return -1;
			}
			if (canMove < cohortSize)
			{
				return 0;
			}
			while (canMove == cohortSize)
			{
				TItem item = enumerators.First().Current;
				if (enumerators.Skip(1).Any(x => comparer.Equals(x.Current, item) == false))
				{
					break;
				}
				firstDifference++;
				canMove = enumerators.Count(x => x.MoveNext());
			}
			if (canMove == 0)
			{
				return -1;
			}
			return firstDifference;
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
				sb.Append(LocalizableStrings.PrintExtensions_ReadableEmpty);
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

		[NotNull]
		public static HashSet<TItem> ToHashSet<TItem>([NotNull] this IEnumerable<TItem> items,
			IEqualityComparer<TItem> comparer = null)
		{
			return new HashSet<TItem>(items.ValidateArgumentIsNotNull(), comparer);
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

		public static class Default
		{
			public const int LengthLimit = 400;
		}

		private delegate string EnumerableLazyPrintMethod<in TItem>(IEnumerable<TItem> enumerable, int lengthlimit);
	}
}
