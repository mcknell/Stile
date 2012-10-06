#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
#endregion

namespace Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations
{
    public interface IEvaluation {}

    public interface IEvaluation<out TResult> : IEvaluation
    {
        [NotNull]
        IWrappedResult<TResult> Result { get; }
    }

    public class Evaluation<TResult> : IEvaluation<TResult>
    {
        public Evaluation([NotNull] IWrappedResult<TResult> result)
        {
            Result = result.ValidateArgumentIsNotNull();
        }

        public IWrappedResult<TResult> Result { get; private set; }
    }
}
