#region License statement
// Stile
// Copyright (c) 2010-2012, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Readability;
using Stile.Types.Enums;
#endregion

namespace Stile.Patterns.Behavioral.Traversal
{
	public abstract class Traverser
	{
		#region Move enum
		public enum Move
		{
			Visit,
			Skip,
			Halt
		}
		#endregion
	}

	public class Traverser<TItem> : Traverser
	{
		private readonly Func<TItem, Move> _accepter;
		private readonly IEnumerable<TItem> _items;
		private readonly Func<IEnumerable<TItem>, IEnumerable<TItem>> _sequencer;

		public Traverser([NotNull] IEnumerable<TItem> items,
		                 [NotNull] Func<TItem, Move> accepter,
		                 Func<IEnumerable<TItem>, IEnumerable<TItem>> sequencer = null)
		{
			_items = ValidateArgument.IsNotDefault(() => items);
			_accepter = ValidateArgument.IsNotDefault(() => accepter);
			_sequencer = sequencer ?? Identity.Map<IEnumerable<TItem>>();
		}

		public bool Traverse()
		{
			IEnumerable<TItem> items = _sequencer.Invoke(_items);
			bool success = items.All(Accept);
			return success;
		}

		protected bool Accept(TItem item)
		{
			Move accepterResult = _accepter.Invoke(item);
			switch (accepterResult)
			{
				case Move.Halt:
					return false; // quit with failure
				case Move.Skip:
					return true; // continue for now
				case Move.Visit:
					return AfterAccept(item);
			}
			throw Enumeration.FailedToRecognize(() => accepterResult);
		}

		protected virtual bool AfterAccept(TItem item)
		{
			return true;
		}

		/* Having sequenced (or assuming that the enumerable is dynamically adapting),
		 * the loop is:
		 * quit with failure?
		 * else, quit with success?
		 * else, skip the next node?
		 * else, visit next node
		 *   possibly memorize it
		 *   
		 * is the initial sequence known (meaning, we can pass it to the sequencer and no more discovery needs to happen),
		 * or should we just consume whatever stream (enumerable) the sequencer produces and expose hooks for it to feed itself?
		 * --> if we're consuming, that feels more like an object (or at least an interface to such an one) than a function that returns an enumerable...
		 *	but we hope that's what the [enumerable -> enumerable] map does
		 */
	}
}
