#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Patterns.SelfDescribingPredicates;
using Stile.Prototypes.Specifications.Evaluations;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.Explainers
{
    public interface IExplainer<in TSubject, in TResult>
    {
        string ExplainActualSurprise(IWrappedResult<TSubject, TResult> result);
        string ExplainExpected(IWrappedResult<TSubject, TResult> result);
    }

    public abstract class Explainer{
        protected static ExpectationVerb ChooseVerb(Negated negated)
        {
            return negated ? ExpectationVerb.NotBe : ExpectationVerb.Be;
        }
    }
    public abstract class Explainer<TSubject, TResult> : Explainer, IExplainer<TSubject, TResult>
    {
        protected Explainer([NotNull] IExpectationVerb expectationVerb,
            string adjective = null,
            Func<TResult, string> expected = null,
            Func<TResult, string> actual = null)
            : this(
                new ExpectedClause<TSubject, TResult>(expectationVerb.Expected, adjective, expected),
                new ActualClause<TSubject, TResult>(expectationVerb.Actual, adjective, actual))
        {
            Verb = expectationVerb;
        }

        protected Explainer([NotNull] ExpectedClause<TSubject, TResult> expected, [NotNull] ActualClause<TSubject, TResult> actual)
        {
// ReSharper disable DoNotCallOverridableMethodsInConstructor
            Expected = expected.ValidateArgumentIsNotNull();
            Actual = actual.ValidateArgumentIsNotNull();
// ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        public virtual string ExplainActualSurprise(IWrappedResult<TSubject, TResult> result)
        {
            return Actual.Explain(result);
        }

        public virtual string ExplainExpected(IWrappedResult<TSubject, TResult> result)
        {
            return Expected.Explain(result);
        }

        [NotNull]
        protected virtual ActualClause<TSubject, TResult> Actual { get; set; }
        [NotNull]
        protected virtual ExpectedClause<TSubject, TResult> Expected { get; set; }
        [NotNull]
        protected IExpectationVerb Verb { get; private set; }
    }
}
