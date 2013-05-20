#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Diagnostics.Contracts;
#endregion

namespace Stile.Patterns.Structural.FluentInterface
{
	public interface INegatable {}

	public interface INegatable<out TReturn> : INegatable
	{
		[Pure]
		TReturn Not { get; }
	}
}
