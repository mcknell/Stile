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

namespace Stile.Prototypes.Specifications
{
    public interface ISpecification {}

    public interface ISpecification<in TSubject, out TResult, out TEvaluation> : ISpecification,
        IEvaluable<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TSubject, TResult> { }

    public abstract class Specification<TSubject, TResult, TEvaluation> : ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TSubject, TResult>
    {
        private readonly Predicate<TResult> _accepter;
        private readonly Func<TResult, Exception, TEvaluation> _exceptionFilter;
        private readonly bool _expectsException;
        private readonly Func<TSubject, TResult> _extractor;

        protected Specification([NotNull] Func<TSubject, TResult> extractor,
            [NotNull] Predicate<TResult> accepter,
            Func<TResult, Exception, TEvaluation> exceptionFilter = null)
        {
            _extractor = extractor.ValidateArgumentIsNotNull();
            _accepter = accepter.ValidateArgumentIsNotNull();
            _exceptionFilter = exceptionFilter;
            _expectsException = _exceptionFilter != null;
        }

        public virtual TEvaluation Evaluate(TSubject subject)
        {
            Outcome outcome;
            TResult result = default(TResult);
            TEvaluation evaluation;
            try
            {
                result = _extractor.Invoke(subject);
                bool accepted = _accepter.Invoke(result);
                outcome = accepted ? Outcome.Succeeded : Outcome.Failed;
            } catch (Exception e)
            {
                if (_expectsException)
                {
                    evaluation = _exceptionFilter.Invoke(result, e);
                    if (evaluation != null)
                    {
                        // only trap exception if exception filter handled it
                        return evaluation;
                    }
                }
                throw;
            }

            if (_expectsException)
            {
                // exception was expected but none was thrown
                return _exceptionFilter.Invoke(result, null);
            }

            var wrappedResult = new WrappedResult<TSubject, TResult>(subject, outcome, result);
            evaluation = EvaluationFactory(wrappedResult);
            return evaluation;
        }

        protected abstract TEvaluation EvaluationFactory(IWrappedResult<TSubject, TResult> result);
    }
}
