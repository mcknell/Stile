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
    public abstract class Evaluator
    {
        public static string FormatExplanation(string expected, string actual)
        {
            return string.Format("would {0}{1} but {2}", expected, Environment.NewLine, actual);
        }

        public static class Default
        {
            public static readonly Func<string, string> FormattingCallback = Identity.Map<string>();
        }
    }

    public class Evaluator<TSubject, TValue> : Evaluator,
        IEvaluator<TSubject>
    {
        private readonly ActualClause<TValue> _actualClause;
        private readonly ExpectedClause<TValue> _expectedClause;
        private readonly Func<TSubject, TValue, string> _explainer;
        private readonly Func<TValue, bool> _predicate;
        private readonly Func<TSubject, TValue> _valueExtractor;

        public Evaluator(ExpectedClause<TValue> expectedClause,
            ActualClause<TValue> actualClause,
            Func<TSubject, TValue> valueExtractor,
            Func<TValue, bool> predicate,
            Func<TSubject, TValue, string> explainer = null)
        {
            _expectedClause = expectedClause;
            _actualClause = actualClause;
            _valueExtractor = valueExtractor;
            _predicate = predicate;
            _explainer = explainer ?? Explain;
        }

        public virtual IEvaluation<TSubject> Evaluate(TSubject subject, Func<string, string> formattingCallback = null)
        {
            TValue actual;
            bool passed = Invoke(subject, out actual);

            if (passed)
            {
                return new Evaluation<TSubject>(subject);
            }
            return Fail(subject, actual, formattingCallback);
        }

        public bool Invoke(TSubject subject)
        {
            TValue actual;
            return Invoke(subject, out actual);
        }

        public bool Invoke(TSubject subject, out TValue actual)
        {
            actual = ExtractActual(subject);
            bool passed = ApplyTest(actual);
            return passed;
        }

        private bool ApplyTest(TValue actual)
        {
            return _predicate.Invoke(actual);
        }

        private string Explain(TSubject subject, TValue actual)
        {
            return FormatExplanation(_expectedClause.Explain(actual), _actualClause.Explain(actual));
        }

        private TValue ExtractActual(TSubject subject)
        {
            return _valueExtractor.Invoke(subject);
        }

        private Evaluation<TSubject> Fail(TSubject subject, TValue actual, Func<string, string> formattingCallback)
        {
            formattingCallback = formattingCallback ?? Default.FormattingCallback;
            var failureReason = new Lazy<string>(() => formattingCallback(_explainer.Invoke(subject, actual)));
            var evaluation = new Evaluation<TSubject>(failureReason, subject);
            return evaluation;
        }
    }

    public class Evaluator<TSubject> : Evaluator<TSubject, TSubject>
    {
        public Evaluator(ExpectedClause<TSubject> expectedClause,
            ActualClause<TSubject> actualClause,
            Func<TSubject, bool> valuePredicate)
            : base(expectedClause, actualClause, x => x, valuePredicate) {}
    }
}
