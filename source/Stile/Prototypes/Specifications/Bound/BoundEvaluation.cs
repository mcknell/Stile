#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.Bound
{
    public interface IBoundEvaluation : IEvaluation {}

    public interface IBoundEvaluation<out TResult> : IBoundEvaluation,
        IEvaluation<TResult> { }

    public class BoundEvaluation<TResult> : Evaluation<TResult>,
        IBoundEvaluation<TResult>
    {
        public BoundEvaluation([NotNull] IWrappedResult<TResult> wrappedResult)
            : base(wrappedResult) {}
    }
}
