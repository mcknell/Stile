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
	/// A symbol on the left of a production rule in the grammar for describing <see cref="ISpecification"/> objects.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Property,
		AllowMultiple = false, Inherited = false)]
	public class RuleAttribute : Attribute,
		IMetadata
	{
		public RuleAttribute(object left, object alias = null, object token = null)
		{
			Left = left.ToString();
			Alias = alias == null ? null : alias.ToString();
			Token = token == null ? null : token.ToString();
			CanBeInlined = true;
		}

		public string Alias { get; private set; }
		public bool CanBeInlined { get; set; }
		public string Left { get; private set; }
		public bool NameIsSymbol { get; set; }
		public bool StartsGrammar { get; set; }
		public string Token { get; set; }
	}
}
