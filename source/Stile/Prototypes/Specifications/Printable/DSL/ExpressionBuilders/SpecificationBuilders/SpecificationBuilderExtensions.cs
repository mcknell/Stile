#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders
{
    public static class SpecificationBuilderExtensions
    {
        [Pure]
        public static ISpecificationBuilder<TSubject, TResult> That<TSubject, TResult>(
            this ISpecificationBuilder<TSubject, TResult> builder, Expression<Func<TSubject, TResult>> expression)
        {
            return new SpecificationBuilder<TSubject, TResult>(expression.Compile);
        }
    }
}
