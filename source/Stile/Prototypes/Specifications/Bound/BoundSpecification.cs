#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.Bound
{
    public interface IBoundSpecification : ISpecification {}

    public interface IBoundSpecification<in TSubject, out TResult> : IBoundSpecification,
        ISpecification<TSubject, TResult, IBoundEvaluation<TResult>>
    {
        IBoundEvaluation<TResult> Evaluate();
    }

    public class BoundSpecification<TSubject, TResult> : Specification<TSubject, TResult, IBoundEvaluation<TResult>>,
        IBoundSpecification<TSubject, TResult>
    {
        private readonly ISource<TSubject> _source;

        public BoundSpecification([NotNull] ISource<TSubject> source,
            [NotNull] Func<TSubject, TResult> extractor,
            [NotNull] Predicate<TResult> accepter,
            Func<TResult, Exception, IBoundEvaluation<TResult>> exceptionFilter = null)
            : base(extractor, accepter, exceptionFilter)
        {
            _source = source.ValidateArgumentIsNotNull();
        }

        public IBoundEvaluation<TResult> Evaluate()
        {
            TSubject subject = _source.Get();
            return Evaluate(subject);
        }

        protected override IBoundEvaluation<TResult> EvaluationFactory(IWrappedResult<TResult> result)
        {
            return new BoundEvaluation<TResult>(result);
        }
    }
}
