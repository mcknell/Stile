#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.Bound
{
    public interface IBoundEvaluation : IEvaluation {}

    public interface IBoundEvaluation<TSubject, out TResult> : IBoundEvaluation,
        IEvaluation<TSubject, TResult> { }

    public class BoundEvaluation<TSubject, TResult> : Evaluation<TSubject, TResult>,
        IBoundEvaluation<TSubject, TResult>
    {
        public BoundEvaluation([NotNull] IWrappedResult<TSubject, TResult> wrappedResult)
            : base(wrappedResult) {}
    }
}
