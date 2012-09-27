#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Propter project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications
{
    public interface IEvaluable {}

    public interface IEvaluable<in TSubject, out TResult, out TEvaluation> : IEvaluable
        where TEvaluation : class, IEvaluation<TResult>
    {
        [NotNull]
        TEvaluation Evaluate(TSubject subject);
    }
}
