#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Linq;
using Stile.Patterns.Behavioral.Validation;
using Stile.Types.Enumerables;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public interface ISequence : IAcceptGrammarVisitors
	{
		IReadOnlyList<IItem> Items { get; }
	}

	public class Sequence : ISequence
	{
		public Sequence(IItem item, params IItem[] items)
			: this(items.Unshift(item)) {}

		public Sequence(IEnumerable<IItem> items)
		{
			items = items.Validate().EnumerableOf<IItem>().IsNotNullOrEmpty();
			Items = items.ToArray();
		}

		public IReadOnlyList<IItem> Items { get; private set; }

		public void Accept(IGrammarVisitor visitor)
		{
			visitor.Visit(this);
		}

		public TData Accept<TData>(IGrammarVisitor<TData> visitor, TData data)
		{
			return visitor.Visit(this, data);
		}

		public override string ToString()
		{
			return string.Join(" ", Items);
		}
	}
}
