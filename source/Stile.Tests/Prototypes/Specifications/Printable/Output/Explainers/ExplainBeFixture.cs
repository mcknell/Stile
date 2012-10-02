#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Patterns.SelfDescribingPredicates;
using Stile.Prototypes.Specifications.Evaluations;
using Stile.Prototypes.Specifications.Printable.Output.Explainers;
using Stile.Prototypes.Specifications.Printable.Output.Explainers.Is;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Printable.Output.Explainers
{
    [TestFixture]
    public class ExplainBeFixture
    {
        [Test]
        public void Fail()
        {
            AssertExplain(Negated.False, Outcome.Failed);
        }

        [Test]
        public void FailNegated()
        {
            AssertExplain(Negated.True, Outcome.Failed);
        }

        [Test]
        public void Pass()
        {
            AssertExplain(Negated.False, Outcome.Succeeded);
        }

        [Test]
        public void PassNegated()
        {
            AssertExplain(Negated.True, Outcome.Succeeded);
        }

        private static void AssertExplain(Negated negated, Outcome outcome)
        {
            var testSubject = new ExplainIs<int, string>(negated, "one");
            var result = new WrappedResult<int, string>(1, outcome, "two");

            // act
            string expected = testSubject.ExplainExpected(result);
            string actual = testSubject.ExplainActualSurprise(result);

            Assert.That(expected, Is.EqualTo(negated + "be \"one\""));
            Assert.That(actual, Is.EqualTo("was \"two\""));
        }
    }
}
