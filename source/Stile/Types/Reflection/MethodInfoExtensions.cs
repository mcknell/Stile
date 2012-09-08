#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
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
