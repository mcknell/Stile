#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Prototypes.Specifications.Grammar.Metadata
{
	/// <summary>
	/// A nonterminal symbol on the right of a production rule in the grammar for constructing <see cref="Specification"/> objects.
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public class NonterminalSymbolAttribute : SymbolAttribute
	{
		public NonterminalSymbolAttribute(object token = null, object alias = null)
			: base(token, alias) {}

		protected override bool ActualTerminal
		{
			get { return false; }
			set { }
		}
	}
}
