﻿#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Linq.Expressions;
#endregion

namespace Stile.Types.Expressions
{
	public static class MemberExpressionExtensions
	{
		public static bool MemberIsStatic(this MemberExpression expression)
		{
			return expression.Expression == null;
		}
	}
}
