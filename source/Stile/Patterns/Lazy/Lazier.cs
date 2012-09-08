#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using System.Threading;
#endregion

namespace Stile.Patterns.Lazy
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
