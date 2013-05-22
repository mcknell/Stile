#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Prototypes.Compilation.Grammars.ContextFree;
#endregion

namespace Stile.Prototypes.Specifications.Grammar.Metadata
{
	/// <summary>
	/// Indicates a fragment that expands the right side of a <see cref="IProductionRule"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = true,
		Inherited = true)]
	public class RuleCategoryAttribute : Attribute,
		IMetadataWithPrior
	{
		public RuleCategoryAttribute(object token = null)
		{
			Token = (token == null) ? null : token.ToString();
		}

		public string Alias { get; set; }
		public string Prior { get; set; }
		public string Token { get; private set; }
		public bool Terminal { get; set; }
	}
}
