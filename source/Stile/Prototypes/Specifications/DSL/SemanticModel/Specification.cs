#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.ExpressionBuilders;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.DSL.SemanticModel
{
    public interface ISpecification {}

    public interface ISpecification<in TSubject> : ISpecification {}

    public interface ISpecification<out TResult, out TEvaluation> : ISpecification
        where TEvaluation : class, IEvaluation<TResult> {}

    public interface ISpecification<in TSubject, out TResult, out TEvaluation> : ISpecification<TSubject>,
        ISpecification<TResult, TEvaluation>,
        IEvaluable<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult> {}

    public abstract class Specification<TSubject, TResult, TEvaluation> :
        ISpecification<TSubject, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult>
    {
        private readonly Predicate<TResult> _accepter;
        private readonly Func<TResult, Exception, TEvaluation> _exceptionFilter;
        private readonly bool _expectsException;
        private readonly Lazy<Func<TSubject, TResult>> _lazyExtractor;

        protected Specification([NotNull] Lazy<Func<TSubject, TResult>> lazyExtractor,
            [NotNull] Predicate<TResult> accepter,
            Func<TResult, Exception, TEvaluation> exceptionFilter = null)
        {
            _lazyExtractor = lazyExtractor.ValidateArgumentIsNotNull();
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
                result = _lazyExtractor.Value.Invoke(subject);
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

            var wrappedResult = new WrappedResult<TResult>(outcome, result);
            evaluation = EvaluationFactory(wrappedResult);
            return evaluation;
        }

        protected abstract TEvaluation EvaluationFactory(IWrappedResult<TResult> result);
    }

    public abstract class Specification<TResult, TEvaluation> : Specification<TResult, TResult, TEvaluation>
        where TEvaluation : class, IEvaluation<TResult>
    {
        protected Specification([NotNull] Predicate<TResult> accepter,
            Func<TResult, Exception, TEvaluation> exceptionFilter = null)
            : base(Instrument.Trivial<TResult>.Map, accepter, exceptionFilter) {}
    }
}
