#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Prototypes.Compilation.Grammars;
#endregion

namespace Stile.Prototypes.Specifications.Grammar.Metadata
{
	/// <summary>
	/// Indicates a fragment that expands the right side of a <see cref="IProductionRule"/>.
	/// </summary>
	[AttributeUsage(
		AttributeTargets.Parameter | AttributeTargets.Method | AttributeTargets.Property
			| AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
	public class RuleExpansionAttribute : Attribute
	{
		public RuleExpansionAttribute([NotNull] object prior, object name = null)
		{
			SymbolToken = (name == null) ? null : name.ToString();
			Prior = prior.ToString();
		}

		[CanBeNull]
		public string SymbolToken { get; private set; }
		[NotNull]
		public string Prior { get; private set; }
	}
}
