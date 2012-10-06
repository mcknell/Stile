#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Construction
{
    public static class Specify
    {
        [Pure]
        public static IBoundSubjectBuilder<TSubject> For<TSubject>(Expression<Func<TSubject>> expression)
        {
            return new BoundSubjectBuilder<TSubject>(new Source<TSubject>(expression));
        }

        [Pure]
        public static IBoundSubjectBuilder<TSubject> For<TSubject>(TSubject subject) where TSubject : class
        {
            return new BoundSubjectBuilder<TSubject>(new Source<TSubject>(subject));
        }

        [Pure]
        public static IBoundEnumerableSubjectBuilder<TSubject, TItem> For<TSubject, TItem>(TSubject subject)
            where TSubject : class, IEnumerable<TItem>
        {
            return new BoundEnumerableSubjectBuilder<TSubject, TItem>(new Source<TSubject>(subject));
        }

        [Pure]
        public static ISubjectBuilder<TSubject> ForAny<TSubject>()
        {
            return new SubjectBuilder<TSubject>();
        }

        [Pure]
        public static IEnumerableSubjectBuilder<TSubject, TItem> ForAny<TSubject, TItem>()
            where TSubject : class, IEnumerable<TItem>
        {
            return new EnumerableSubjectBuilder<TSubject, TItem>();
        }

        [Pure]
        public static IFluentSpecificationBuilder<TSubject, TSubject> ThatAny<TSubject>()
        {
            return new PrintableSpecificationBuilder<TSubject>();
        }
    }
}
