#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Patterns.SelfDescribingPredicates;
using Stile.Patterns.SelfDescribingPredicates.Evaluators;
#endregion

namespace Stile.Tests.Patterns.SelfDescribingPredicates
{
    [TestFixture]
    public class EvaluatorFixture
    {
        [Test]
        public void Evaluate()
        {
            const int actual = 1;
            const int expected = 3;
            Assume.That(actual, Is.Not.EqualTo(expected));

            // arrange
            var testSubject = new SimpleEvaluator<int>(expected, x => x == expected);

            // act
            IEvaluation<int> evaluation = testSubject.Evaluate(actual);

            // assert
            Assert.That(evaluation.Success, Is.False);
            Assert.That(evaluation.Reason, Is.EqualTo("would be 3" + Environment.NewLine + " but was 1"));
        }
    }
}
