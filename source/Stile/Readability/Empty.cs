#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
#endregion

namespace Stile.Readability
{
	public static class Empty
	{
		public static readonly Action Action = () => {};

// ReSharper disable UnusedParameter.Global
		public static void ActionOn<TItem>(TItem item) {}
// ReSharper restore UnusedParameter.Global
	}
}
