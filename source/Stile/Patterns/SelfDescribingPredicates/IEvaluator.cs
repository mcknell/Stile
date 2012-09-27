#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Propter project on GitHub
#endregion

namespace Stile.Patterns.SelfDescribingPredicates
{
    public interface IEvaluator<TSubject> : ISelfDescribingPredicate<TSubject>
    {
        //IEvaluation<TSubject> Evaluate(TSubject subject, Func<string, string> formattingCallback = null);
    }
}
