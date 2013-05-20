#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.CodeAnalysis;
#endregion

namespace Stile.Readability
{
	public static class Empty
	{
		public static readonly Action Action = () => {};

// ReSharper disable UnusedParameter.Global
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "item")]
		public static void ActionOn<TItem>(TItem item) {}

// ReSharper restore UnusedParameter.Global
	}
}
