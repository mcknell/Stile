#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Threading;
#endregion

namespace Stile.Tests.Prototypes.Specifications.SampleObjects
{
	public class Foo<TItem> : List<TItem>
	{
		public int Jump()
		{
			return GetHashCode();
		}

		public bool Sleep(TimeSpan timeSpan)
		{
			Thread.Sleep(timeSpan);
			return true;
		}
	}
}
