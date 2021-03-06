﻿#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Reflection;
using System.Runtime.CompilerServices;
#endregion

namespace Stile.Types.Reflection
{
	public static class MethodInfoExtensions
	{
		public static bool IsExtensionMethod(this MethodInfo methodInfo)
		{
			return methodInfo.IsStatic && methodInfo.IsDefined(typeof(ExtensionAttribute), true);
		}
	}
}
