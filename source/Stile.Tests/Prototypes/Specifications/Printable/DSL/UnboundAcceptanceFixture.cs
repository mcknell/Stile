#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Prototypes.Specifications.Evaluations;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.Printable.Construction;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Is;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Printable.DSL
{
    [TestFixture]
    public class UnboundAcceptanceFixture
    {
        [Test]
        public void EqualTo()
        {
            // arrange
            DateTime now = DateTime.Now;

            // act and assert
            AssertEquals(Specify.ThatAny<DateTime>().Is.EqualTo(DateTime.MinValue), Outcome.Failed, now);
            AssertEquals(Specify.ThatAny<DateTime>().Is.Not.EqualTo(DateTime.MinValue), Outcome.Succeeded, now);
        }

        [Test]
        public void GreaterThan()
        {
            // arrange
            DateTime now = DateTime.Now;

            // act and assert
            AssertEquals(Specify.For<DateTime>().That(x => x.Ticks).Is.EqualTo(0), Outcome.Failed, now);
            AssertEquals(Specify.For<DateTime>().That(x => x.Ticks).Is.Not.EqualTo(0), Outcome.Succeeded, now);
        }

        private void AssertEquals<T1, T2>(IPrintableSpecification<T1, T2> specification, Outcome outcome, T1 now)
        {
            IPrintableEvaluation<T1, T2> evaluation = specification.Evaluate(now);
            Assert.That(evaluation.Result.Outcome, Is.EqualTo(outcome));
            Assert.That(evaluation.Emitted.Retrieved.Value, Is.Not.Empty);
        }

        private static void AssertEquals<T>(IPrintableSpecification<T> negatedSpecification, Outcome outcome, T value)
        {
            IPrintableEvaluation<T> evaluation = negatedSpecification.Evaluate(value);
            Assert.That(evaluation.Result.Outcome, Is.EqualTo(outcome));
            Assert.That(evaluation.Emitted.Retrieved.Value, Is.Not.Empty);
        }
    }
}
