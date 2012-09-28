#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Stile.Patterns.Behavioral.Validation;
using Stile.Prototypes.Specifications.Evaluations;
using Stile.Readability;
using Stile.Types.Expressions;
#endregion

namespace Stile.Prototypes.Specifications.Printable.Output.Explainers
{
    public class ExplainHasAll<TSubject, TResult, TItem> : Explainer<TSubject, TResult>
        where TResult : class, IEnumerable<TItem>
    {
        private readonly Expression<Func<TItem, bool>> _expression;
        private readonly int _originalCount;
        /* explainer ::= 'have' ('1 item' | 'all' expected-count 'items') 'satisfying' predicate 
         *  conjunction 'had only' actual-count 'such' ('item' | 'items') '. These failed: '
         *  list-up-to-max-length
         */

        public ExplainHasAll(int originalCount, [NotNull] Expression<Func<TItem, bool>> expression)
            : base(ExpectationVerb.Have)
        {
            _originalCount = originalCount;
            _expression = expression.ValidateArgumentIsNotNull();
        }

        public ExplainHasAll([NotNull] ExpectedClause<TSubject, TResult> expected,
            [NotNull] ActualClause<TSubject, TResult> actual)
            : base(expected, actual) {}

        public override string ExplainActualSurprise(IWrappedResult<TSubject, TResult> result)
        {
            int actualCount = result.Value.Count();
            return string.Format("{0} only {1} such {2}. These failed: {3}",
                Verb.Actual,
                actualCount,
                actualCount.Pluralize("item"),
                Environment.NewLine);
        }

        public override string ExplainExpected(IWrappedResult<TSubject, TResult> result)
        {
            string expectedItems = _originalCount.Pluralize(_originalCount + " item", "all " + _originalCount + " items");
            string expressionExplained = ExpressionExtensions.ToDebugString(_expression);
            return string.Format("{0} {1} satisfying {2}", Verb.Actual, expectedItems, expressionExplained);
        }
    }
}
