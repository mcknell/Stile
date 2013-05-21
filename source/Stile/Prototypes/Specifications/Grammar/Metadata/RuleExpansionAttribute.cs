#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
#endregion

namespace Stile.Prototypes.Specifications.Grammar.Metadata
{
	/// <summary>
	/// Indicates a fragment that expands the right side of a <see cref="IProductionRule"/>.
	/// </summary>
	[AttributeUsage(
		AttributeTargets.Parameter | AttributeTargets.Method | AttributeTargets.Property
			| AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
	public class RuleExpansionAttribute : Attribute,
		IMetadata
	{
		public RuleExpansionAttribute([NotNull] object prior, object symbol = null, object alias = null)
		{
			Token = (symbol == null) ? null : symbol.ToString();
			Alias = (alias == null) ? null : alias.ToString();
			Prior = prior.ToString();
		}

		public string Alias { get; private set; }
		public bool Optional { get; set; }

		[NotNull]
		public string Prior { get; private set; }
		public string Token { get; private set; }
		public bool Terminal { get; set; }
	}
}
