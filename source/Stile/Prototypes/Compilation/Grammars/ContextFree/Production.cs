#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public interface IProduction
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
		}

		public NonterminalSymbol Left { get; private set; }
		public IChoice Right { get; private set; }

		public override string ToString()
		{
			return string.Format("{0} {1} {2}", Left, TerminalSymbol.EBNFAssignment, Right);
		}
	}

	public interface IPrimary {}
}
