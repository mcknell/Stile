#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

namespace Stile.Patterns.SelfDescribingPredicates
{
    public interface IEvaluation
    {
        string Reason { get; }
        bool Success { get; }
    }

    public interface IEvaluation<out TSubject> : IEvaluation
    {
        TSubject Result { get; }
    }
}
