#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public interface IItem: IAcceptGrammarVisitors
	{
		Cardinality Cardinality { get; }
		IPrimary Primary { get; }
	}

	public class Item : IItem
	{
		public Item(IPrimary primary, Cardinality cardinality = Cardinality.One)
		{
			Primary = primary.ValidateArgumentIsNotNull();
			Cardinality = cardinality;
		}

		public Cardinality Cardinality { get; private set; }
		public IPrimary Primary { get; private set; }

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
			return Primary + Cardinality.ToEbnfString();
		}
	}
}
