#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Patterns.Behavioral.Validation;
using Stile.Types.Primitives;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public interface IProduction : IAcceptGrammarVisitors
	{
		NonterminalSymbol Left { get; }
		IChoice Right { get; }
	}

	public class Production : IProduction
	{
		public Production(NonterminalSymbol left, IChoice right)
		{
			Left = left.ValidateArgumentIsNotNull();
			Right = right.ValidateArgumentIsNotNull();
			Count = Right.Count + 1;
		}

		public int Count { get; private set; }
		public NonterminalSymbol Left { get; private set; }
		public IChoice Right { get; private set; }

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
			return "{0} {1} {2}".InvariantFormat(Left, TerminalSymbol.EBNFAssignment, Right);
		}
	}
}
