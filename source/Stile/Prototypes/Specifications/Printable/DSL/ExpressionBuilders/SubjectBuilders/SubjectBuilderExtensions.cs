#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SpecificationBuilders;
#endregion

namespace Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders
{
    public static class SubjectBuilderExtensions
    {
/*
        /// <summary>
        /// Syntactical gimmick for specifying the type of items in an <see cref="IEnumerable{T}"/>
        /// without having to re-specify the type of <typeparamref name="TSubject"/>.
        /// </summary>
        /// <typeparam name="TSubject"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="builder"></param>
        /// <param name="typeSpecifier"></param>
        /// <returns></returns>
        [Pure]
        public static IPrintableEnumerableSpecificationBuilder<TSubject, TItem> Containing<TSubject, TItem>(
            this IBoundSubjectBuilder<TSubject> builder, ITypeSpecifier<TItem> typeSpecifier)
            where TSubject : class, IEnumerable<TItem>
        {
            return new EnumerableSpecificationBuilder<TSubject, TItem>();
        }

        /// <summary>
        /// Syntactical gimmick for implicitly specifying the type of items an <see cref="IEnumerable{T}"/>,
        /// i.e., type inference will resolve the type without the coder having to hard-code an explicit 
        /// type parameter (which is much easier and more likely to be correct after refactorings).
        /// </summary>
        /// <typeparam name="TSubject"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="builder"></param>
        /// <param name="item">The value of this item is never used; only its type matters.</param>
        /// <returns></returns>
        [Pure]
        public static IEnumerableSpecificationBuilder<TSubject, TItem> OfItemsLike<TSubject, TItem>(
            this IBoundSubjectBuilder<TSubject> builder, TItem item) where TSubject : class, IEnumerable<TItem>
        {
            return new EnumerableSpecificationBuilder<TSubject, TItem>();
        }
*/

        [Pure]
        public static IFluentSpecificationBuilder<TSubject, TResult> That<TSubject, TResult>(
            this ISubjectBuilder<TSubject> builder, Expression<Func<TSubject, TResult>> expression)
        {
            return new PrintableSpecificationBuilder<TSubject, TResult>(expression);
        }
    }
}
