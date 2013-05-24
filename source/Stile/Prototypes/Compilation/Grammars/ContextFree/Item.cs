#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Compilation.Grammars.ContextFree.Builders;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public interface IItem : IAcceptGrammarVisitors,
		IEquatable<IItem>
	{
		Cardinality Cardinality { get; }
		IPrimary Primary { get; }

		IEnumerable<IItem> Flatten();
		IEnumerable<IFragment> Fragments();
		Symbol PrimaryAsSymbol();
	}

	public partial class Item : IItem
	{
		public Item(IPrimary primary, Cardinality cardinality = Cardinality.One)
		{
			Primary = primary.ValidateArgumentIsNotNull();
			Cardinality = cardinality;
			Count = Primary.Count;
		}

		public Cardinality Cardinality { get; private set; }
		public int Count { get; private set; }
		public IPrimary Primary { get; private set; }

		public void Accept(IGrammarVisitor visitor)
		{
			visitor.Visit(this);
		}

		public TData Accept<TData>(IGrammarVisitor<TData> visitor, TData data)
		{
			return visitor.Visit(this, data);
		}

		public IEnumerable<IItem> Flatten()
		{
			var choice = Primary as IChoice;
			if (choice != null && choice.Sequences.Count == 1)
			{
				return choice.Sequences[0].SelectMany(x => x.Flatten());
			}
			return new[] {this};
		}

		public IEnumerable<IFragment> Fragments()
		{
			var choice = Primary as IChoice;
			if (choice != null)
			{
				foreach (IFragment fragment in choice.Fragments())
				{
					yield return fragment;
				}
			}
		}

		public Symbol PrimaryAsSymbol()
		{
			if (Primary is IChoice)
			{
				return null;
			}
			return (Symbol) Primary;
		}

		public override string ToString()
		{
			return Primary + Cardinality.ToEbnfString();
		}
	}

	public partial class Item
	{
		public bool Equals(IItem other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return Cardinality == other.Cardinality && Primary.Equals(other.Primary);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			var other = obj as Item;
			return other != null && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((int) Cardinality * 397) ^ Primary.GetHashCode();
			}
		}

		public static bool operator ==(Item left, Item right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Item left, Item right)
		{
			return !Equals(left, right);
		}
	}
}
