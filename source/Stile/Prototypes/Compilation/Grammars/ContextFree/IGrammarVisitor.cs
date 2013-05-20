#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
#endregion

namespace Stile.Prototypes.Compilation.Grammars.ContextFree
{
	public interface IGrammarVisitor
	{
		void Visit(IGrammar target);
		void Visit(IProductionRule target);
		void Visit(IClause target);
		void Visit(Symbol target);
	}
}
