#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.Emitting
{
    public interface IEmittingEvaluation : IEvaluation {}

    public interface IEmittingEvaluation<out TEmit> : IEmittingEvaluation
    {
        TEmit Emitted { get; }
    }

    public interface IEmittingEvaluation<out TResult, out TEmit> : IEmittingEvaluation<TEmit>,
        IEvaluation<TResult> {}

    public class EmittingEvaluation<TResult, TEmit> : IEmittingEvaluation<TResult, TEmit>
    {
        public EmittingEvaluation([NotNull] IWrappedResult<TResult> wrappedResult, TEmit emitted)
        {
            Result = wrappedResult.ValidateArgumentIsNotNull();
            Emitted = emitted;
        }

        public TEmit Emitted { get; private set; }
        public IWrappedResult<TResult> Result { get; private set; }
    }
}
