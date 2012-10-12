#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Printable.DSL.ExpressionBuilders
{
	public interface IFoo<out T> : IEnumerable<T>
	{
		int Jump();
	}

	internal class Foo<T> : List<T>,
		IFoo<T>
	{
		public int Jump()
		{
			return GetHashCode();
		}
	}
}
