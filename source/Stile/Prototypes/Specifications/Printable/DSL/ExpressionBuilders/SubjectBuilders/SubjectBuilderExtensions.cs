#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Linq.Expressions;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders
{
    public static class SubjectBuilderExtensions
    {
        public static ISpecificationBuilder<TSource, TSubject> That<TSource, TSubject>(this ISubjectBuilder<TSource> builder,
            Expression<Func<TSource, TSubject>> instrument)
        {
            return new SpecificationBuilder<TSource, TSubject>(instrument);
        }
    }
}
