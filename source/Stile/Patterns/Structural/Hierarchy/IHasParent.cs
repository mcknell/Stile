#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

using JetBrains.Annotations;

namespace Stile.Patterns.Structural.Hierarchy
{
	public interface IHasParent<out TParent>
	{
		[CanBeNull]
		TParent Parent { get; }
	}
}
