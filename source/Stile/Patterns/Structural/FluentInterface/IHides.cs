#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

namespace Stile.Patterns.Structural.FluentInterface
{
	public interface IHides<out TCollaborator>
	{
		/// <summary>
		/// Allows a mostly-fluent API to expose a non-fluent interface via a single property, e.g., for properties and state. 
		/// </summary>
		/// <remarks>Name starts with X so it tends to go to the botttom of the IntelliSense menu.</remarks>
		TCollaborator Xray { get; }
	}
}
