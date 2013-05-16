#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
#endregion

namespace Stile.Patterns.Structural.Hierarchy
{
	public interface ICanGetPredecessors<out TPredecessor>
		where TPredecessor : class
	{
		IEnumerable<TPredecessor> GetPredecessors(bool includeSelf = false);
	}
}
