#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
#endregion

namespace Stile.Prototypes.Specifications.Grammar.Metadata
{
	/// <summary>
	/// A production rule that indicates a class is a specialization of its superclass.
	/// </summary>
	[AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
	public class SpecializationAttribute : Attribute
	{
		public SpecializationAttribute(object symbol = null)
		{
			Symbol = symbol;
			CanBeInlined = true;
		}

		public bool CanBeInlined { get; set; }
		public object Symbol { get; private set; }
		public string SymbolToken
		{
			get { return Symbol == null ? null : Symbol.ToString(); }
		}
	}
}
