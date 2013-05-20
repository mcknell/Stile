#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Threading;
#endregion

namespace Stile.Patterns.Lifetimes.Lazy
{
	public class Lazier<TArg> : Lazy<TArg>
		where TArg : new()
	{
		public Lazier()
			: base(Factory) {}

		public Lazier(bool isThreadSafe)
			: base(Factory, isThreadSafe) {}

		public Lazier(LazyThreadSafetyMode mode)
			: base(Factory, mode) {}

		private static TArg Factory()
		{
			return new TArg();
		}
	}
}
