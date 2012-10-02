#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Specifications.Evaluations
{
    public interface IEvaluation {}

    public interface IEvaluationBase<out TWrappedResult> : IEvaluation
        where TWrappedResult : class, IWrappedResult
    {
        [NotNull]
        TWrappedResult Result { get; }
    }

    public interface IEvaluationBase<out TSubject, out TWrappedResult> : IEvaluationBase<TWrappedResult>
        where TWrappedResult : class, IWrappedResult<TSubject> {}

    public interface IEvaluationBase<out TSubject, out TResult, out TWrappedResult> : IEvaluationBase<TWrappedResult>
        where TWrappedResult : class, IWrappedResult<TSubject, TResult> {}

    public interface IEvaluation<out TSubject> : IEvaluationBase<TSubject, IWrappedResult<TSubject>> {}

    public interface IEvaluation<out TSubject, out TResult> : IEvaluationBase<TResult, IWrappedResult<TSubject, TResult>> {}

    public class Evaluation<TSubject, TResult> : IEvaluation<TSubject, TResult>
    {
        public Evaluation([NotNull] IWrappedResult<TSubject, TResult> wrappedResult)
        {
            Result = wrappedResult.ValidateArgumentIsNotNull();
        }

        public IWrappedResult<TSubject, TResult> Result { get; private set; }
    }

    public class Evaluation<TSubject> : IEvaluation<TSubject>
    {
        public Evaluation([NotNull] IWrappedResult<TSubject> result)
        {
            Result = result.ValidateArgumentIsNotNull();
        }

        public IWrappedResult<TSubject> Result { get; private set; }
    }
}
