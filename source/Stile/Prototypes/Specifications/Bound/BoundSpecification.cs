#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.SemanticModel;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Sources;
#endregion

namespace Stile.Prototypes.Specifications.Bound
{
    public interface IBoundSpecification : ISpecification {}

    public interface IBoundSpecification<in TSubject, out TResult, out TEvaluation> : IBoundSpecification,
        ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IBoundEvaluation<TResult>
    {
        TEvaluation Evaluate();
    }

    public interface IBoundSpecificationState<TSubject, TResult, out TEvaluation> :
        ISpecificationState<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IBoundEvaluation<TResult> {}

    public abstract class BoundSpecification<TSubject, TResult, TEvaluation> :
        Specification<TSubject, TResult, TEvaluation>,
        IBoundSpecification<TSubject, TResult, TEvaluation>,
        IBoundSpecificationState<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IBoundEvaluation<TResult>
    {
        private readonly IPrintableSource<TSubject> _source;

        protected BoundSpecification([NotNull] IPrintableSource<TSubject> source,
            [NotNull] Lazy<Func<TSubject, TResult>> instrument,
            [NotNull] Predicate<TResult> accepter,
            Func<TResult, Exception, TEvaluation> exceptionFilter = null)
            : base(instrument, accepter, exceptionFilter)
        {
            _source = source.ValidateArgumentIsNotNull();
        }

        public TEvaluation Evaluate()
        {
            return base.Evaluate(_source.Get());
        }
    }
}
