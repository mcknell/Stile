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
	/// A symbol on the right of a production rule in the grammar for constructing <see cref="Specification"/> objects.
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public class SymbolAttribute : Attribute,
		IMetadata
	{
		public SymbolAttribute(object token = null, object alias = null)
		{
			Token = token == null ? null : token.ToString();
			Alias = alias == null ? null : alias.ToString();
// ReSharper disable DoNotCallOverridableMethodsInConstructor
			Terminal = true;
// ReSharper restore DoNotCallOverridableMethodsInConstructor
		}

		public string Alias { get; private set; }
		public string Token { get; private set; }
		public virtual bool Terminal { get; set; }
	}
}
