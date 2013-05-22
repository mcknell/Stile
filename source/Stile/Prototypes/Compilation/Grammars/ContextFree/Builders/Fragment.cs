#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree.Builders
{
	public interface IFragment
	{
		Cardinality Cardinality { get; }
		string Left { get; }
		NonterminalSymbol Right { get; }
	}

	public class Fragment : IFragment
	{
		public Fragment(string left, NonterminalSymbol right, Cardinality cardinality = Cardinality.One)
		{
			Left = left.ValidateStringNotNullOrEmpty();
			Right = right.ValidateArgumentIsNotNull();
			Cardinality = cardinality;
		}

		public Cardinality Cardinality { get; private set; }

		public string Left { get; private set; }
		public NonterminalSymbol Right { get; private set; }
	}
}
