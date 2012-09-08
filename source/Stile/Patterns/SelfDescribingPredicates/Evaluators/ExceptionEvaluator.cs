#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using Stile.Readability;
#endregion

namespace Stile.Patterns.SelfDescribingPredicates.Evaluators
{
    public class ExceptionEvaluator<TSubject, TException> : Evaluator<TSubject>
        where TException : Exception
    {
        public ExceptionEvaluator()
            : base(new NoOpExpectedClause<TSubject>(), new NoOpActualClause<TSubject>(), x => true) {}

        public override IEvaluation<TSubject> Evaluate(TSubject subject, Func<string, string> formattingCallback = null)
        {
            Exception capturedException = null;
            try
            {
                Invoke(subject);
            } catch (Exception e)
            {
                capturedException = e;
            }
            string but;
            if (capturedException == null)
            {
                but = "no exception was thrown.";
            }
            else
            {
                var expectedException = capturedException as TException;
                if (expectedException != null)
                {
                    return new Evaluation<TSubject>(subject);
                }
                but = string.Format("threw {0},{1}{2}",
                    capturedException.GetType().ToDebugString(),
                    Environment.NewLine,
                    capturedException.Message);
            }

            return
                new Evaluation<TSubject>(
                    string.Format("would throw exception of type {0}{1} but {2}",
                        typeof(TException).ToDebugString(),
                        Environment.NewLine,
                        but),
                    subject);
        }
    }
}
