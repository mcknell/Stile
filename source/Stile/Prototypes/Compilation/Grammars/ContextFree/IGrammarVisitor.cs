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
		void Visit(IChoice target);
		void Visit(IGrammar target);
		void Visit(IItem target);
		void Visit(IProduction target);
		void Visit(ISequence target);
		void Visit(Symbol target);
	}

	public interface IGrammarVisitor<TData>
	{
		TData Visit(IChoice target, TData data);
		TData Visit(IGrammar target, TData data);
		TData Visit(IItem target, TData data);
		TData Visit(IProduction target, TData data);
		TData Visit(ISequence target, TData data);
		TData Visit(Symbol target, TData data);
	}
}
