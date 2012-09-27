#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Prototypes.Specifications.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output
{
    public interface IDescription<in TResult>
    {
        string ExplainActual(IWrappedResult<TResult> result);
        string ExplainExpected(IWrappedResult<TResult> result);
    }

    public class Description<TResult> : IDescription<TResult>
    {
        private readonly ActualClause<TResult> _actual;
        private readonly ExpectedClause<TResult> _expected;

        public Description([NotNull] IExpectationVerb expectationVerb,
            Func<TResult, string> expected = null,
            Func<TResult, string> actual = null)
        {
            _expected = new ExpectedClause<TResult>(expectationVerb.Expected, expected);
            _actual = new ActualClause<TResult>(expectationVerb.Actual, actual);
        }

        public string ExplainActual(IWrappedResult<TResult> result)
        {
            return _actual.Explain(result);
        }

        public string ExplainExpected(IWrappedResult<TResult> result)
        {
            return _expected.Explain(result);
        }
    }
}
