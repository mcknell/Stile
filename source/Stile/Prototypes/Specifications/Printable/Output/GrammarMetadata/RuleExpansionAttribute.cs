#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using Stile.Prototypes.Compilation.Grammars;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata
{
	/// <summary>
	/// Indicates a fragment that expands the right side of a <see cref="ProductionRule"/>.
	/// </summary>
	[AttributeUsage(
		AttributeTargets.Parameter | AttributeTargets.Method | AttributeTargets.Property
			| AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
	public class RuleExpansionAttribute : Attribute
	{
		public RuleExpansionAttribute(object name = null)
		{
			Name = name;
		}

		public object Name { get; set; }
	}
}
