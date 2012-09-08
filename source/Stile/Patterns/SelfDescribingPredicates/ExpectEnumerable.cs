#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Stile.Patterns.SelfDescribingPredicates.Evaluators;
using Stile.Readability;
using Stile.Types.Equality;
#endregion

namespace Stile.Patterns.SelfDescribingPredicates
{
    public static class ExpectEnumerable<TSubject, TItem>
        where TSubject : class, IEnumerable<TItem>
    {
        public static Evaluator<TSubject, int> CountOf(int count)
        {
            VerbTensePair wouldHaveButHad = VerbTensePair.WouldHaveButHad;
            var evaluator =
                new Evaluator<TSubject, int>(new ExpectedClause<int>(wouldHaveButHad, x => string.Format("count of {0}", count)),
                    new ActualClause<int>(wouldHaveButHad, x => string.Format("count of {0}", x)),
                    x => x.Count(),
                    x => x == count);

            return evaluator;
        }

        public static Evaluator<TSubject, int> ItemsSatisfying(Expression<Func<TItem, bool>> predicateExpression,
            int count,
            Func<int, bool> countPredicate,
            string qualifier)
        {
            Func<TItem, bool> compiled = predicateExpression.Compile();
            VerbTensePair wouldHaveButHad = VerbTensePair.WouldHaveButHad;
            var evaluator =
                new Evaluator<TSubject, int>(
                    new ExpectedClause<int>(wouldHaveButHad,
                        x =>
                        string.Format("{0} {1} satisfying {2}",
                            qualifier,
                            count.Pluralize("item"),
                            predicateExpression.ToDebugString())),
                    new ActualClause<int>(wouldHaveButHad, x => string.Format("{0} such {1}", x, x.Pluralize("item"))),
                    x => x.Count(compiled),
                    countPredicate);

            return evaluator;
        }

        public static Evaluator<TSubject, TItem> MinOf(TItem min)
        {
            VerbTensePair verbTensePair = VerbTensePair.WouldHaveButHad;
            Func<TItem, string> explainer = x => string.Format("min of {0}", min.ToDebugString());

            var evaluator = new Evaluator<TSubject, TItem>(new ExpectedClause<TItem>(verbTensePair, x => explainer.Invoke(min)),
                new ActualClause<TItem>(verbTensePair, x => "min of " + x),
                x => x.Min(),
                x => x.EqualsOrIsEquallyNull(min));
            return evaluator;
        }

        public static Evaluator<TSubject, bool> ToBeEmpty(Negated negated)
        {
            VerbTensePair wouldBeButWas = VerbTensePair.WouldBeButWas;
            var evaluator = new Evaluator<TSubject, bool>(new ExpectedClause<bool>(wouldBeButWas, x => "empty"),
                new ActualClause<bool>(wouldBeButWas, x => x.ToDebugString()),
                x => !x.Any(),
                x => x != negated);

            return evaluator;
        }

        public static Evaluator<TCollection, bool> ToBeReadOnly<TCollection>(Negated negated)
            where TCollection : class, ICollection<TItem>
        {
            VerbTensePair wouldBeButWas = VerbTensePair.WouldBeButWas;
            var evaluator =
                new Evaluator<TCollection, bool>(new ExpectedClause<bool>(wouldBeButWas, x => "a read only collection"),
                    new ActualClause<bool>(wouldBeButWas, x => "mutable"),
                    x => x.IsReadOnly,
                    x => x != negated);

            return evaluator;
        }

        public static Evaluator<TSubject> ToContain(TItem item, Negated negated)
        {
            VerbTensePair wouldContainButWas = VerbTensePair.WouldContainButWas;
            var evaluator = new Evaluator<TSubject>(new ExpectedClause<TSubject>(wouldContainButWas, x => item.ToDebugString()),
                new ActualClause<TSubject>(wouldContainButWas),
                x => x.Contains(item) != negated);
            return evaluator;
        }
    }
}
