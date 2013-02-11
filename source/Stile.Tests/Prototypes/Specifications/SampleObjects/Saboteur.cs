#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
#endregion

namespace Stile.Tests.Prototypes.Specifications.SampleObjects
{
	public class Saboteur
	{
		public Lazy<Exception> LazyThrower { get; private set; }

		public void Load<TException>(Func<TException> thrower) where TException : Exception
		{
			LazyThrower = new Lazy<Exception>(thrower.Invoke);
		}

		public void Throw()
		{
			throw LazyThrower.Value;
		}
	}
}
