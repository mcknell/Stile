﻿#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using JetBrains.Annotations;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Evaluations
{
    public interface IEvaluable {}

   /* public interface IEvaluable<in TSubject, out TEvaluation> : IEvaluable
        where TEvaluation : class, IEvaluation<TSubject>
    {
        [NotNull]
        [System.Diagnostics.Contracts.Pure]
        TEvaluation Evaluate(TSubject subject);
    }*/

    public interface IEvaluable<in TSubject, out TResult, out TEvaluation> : IEvaluable
        where TEvaluation : class, IEvaluation<TResult>
    {
        [NotNull]
        [System.Diagnostics.Contracts.Pure]
        TEvaluation Evaluate(TSubject subject);
    }
}