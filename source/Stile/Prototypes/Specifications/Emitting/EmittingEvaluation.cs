#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.Emitting
{
    public interface IEmittingEvaluation : IEvaluation {}

    public interface IEmittingEvaluation<out TEmit> : IEmittingEvaluation
    {
        TEmit Emitted { get; }
    }

    public interface IEmittingEvaluation<out TSubject, out TEmit> : IEmittingEvaluation<TEmit>,
        IEvaluation<TSubject> {}

    public interface IEmittingEvaluation<out TSubject, out TResult, out TEmit> : IEmittingEvaluation<TEmit>,
        IEvaluation<TSubject, TResult> {}

    public class EmittingEvaluation<TSubject, TResult, TEmit> : IEmittingEvaluation<TSubject, TResult, TEmit>
    {
        public EmittingEvaluation([NotNull] IWrappedResult<TSubject, TResult> wrappedResult, TEmit emitted)
        {
            Result = wrappedResult.ValidateArgumentIsNotNull();
            Emitted = emitted;
        }

        public TEmit Emitted { get; private set; }
        public IWrappedResult<TSubject, TResult> Result { get; private set; }
    }

    public class EmittingEvaluation<TSubject, TEmit> : IEmittingEvaluation<TSubject, TEmit>
    {
        public EmittingEvaluation([NotNull] IWrappedResult<TSubject> wrappedResult, TEmit emitted)
        {
            Result = wrappedResult.ValidateArgumentIsNotNull();
            Emitted = emitted;
        }

        public TEmit Emitted { get; private set; }
        public IWrappedResult<TSubject> Result { get; private set; }
    }
}
