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
	/// Indicates a fragment that expands the right side of a <see cref="Production"/>.
	/// </summary>
	[AttributeUsage(
		AttributeTargets.Parameter | AttributeTargets.Method | AttributeTargets.Property
			| AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
	public class RuleFragmentAttribute : Attribute,
		IMetadataWithPrior
	{
		public RuleFragmentAttribute([NotNull] object prior, object token = null, object alias = null)
		{
			Token = (token == null) ? null : token.ToString();
			Alias = (alias == null) ? null : alias.ToString();
			Prior = prior.ToString();
			Terminal = false;
		}

		public string Alias { get; private set; }
		public bool Optional { get; set; }

		[NotNull]
		public string Prior { get; private set; }
		public bool Terminal { get; private set; }
		public string Token { get; private set; }
	}
}
