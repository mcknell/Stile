#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.GrammarMetadata
{
	/// <summary>
	/// A symbol on the right of a production rule in the grammar for describing <see cref="ILazyReadableText"/> objects.
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public class SymbolAttribute : Attribute
	{
		public SymbolAttribute(object name = null)
		{
			Name = name;
		}

		public object Name { get; set; }
	}
}
