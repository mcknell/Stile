#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Specifications.Evaluations
{
    public interface IEvaluation {}

    public interface IEvaluation<out TSubject, out TResult> : IEvaluation
    {
        [NotNull]
        IWrappedResult<TSubject, TResult> Result { get; }
    }

    public class Evaluation<TSubject, TResult> : IEvaluation<TSubject, TResult>
    {
        public Evaluation([NotNull] IWrappedResult<TSubject, TResult> wrappedResult)
        {
            Result = wrappedResult.ValidateArgumentIsNotNull();
        }

        public IWrappedResult<TSubject, TResult> Result { get; private set; }
    }
}
