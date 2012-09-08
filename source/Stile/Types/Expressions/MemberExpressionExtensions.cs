﻿#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
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
