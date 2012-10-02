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

namespace Stile.Tests.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.Is
{
    [TestFixture]
    public class IsExtensionsFixture
    {
        [Test]
        public void ComparablyEquivalentTo()
        {
            // arrange
            DateTime now = DateTime.Now;

            // act and assert
            AssertEquals(Specify.ThatAny<DateTime>().Is.ComparablyEquivalentTo(DateTime.MinValue), Outcome.Failed, now);
            AssertEquals(Specify.ThatAny<DateTime>().Is.Not.ComparablyEquivalentTo(DateTime.MinValue), Outcome.Succeeded, now);

            AssertEquals(Specify.For<DateTime>().That(x => x.Ticks).Is.ComparablyEquivalentTo(0), Outcome.Failed, now);
            AssertEquals(Specify.For<DateTime>().That(x => x.Ticks).Is.Not.ComparablyEquivalentTo(0), Outcome.Succeeded, now);
        }

        [Test]
        public void EqualTo()
        {
            // arrange
            DateTime now = DateTime.Now;

            // act and assert
            AssertEquals(Specify.ThatAny<DateTime>().Is.EqualTo(DateTime.MinValue), Outcome.Failed, now);
            AssertEquals(Specify.ThatAny<DateTime>().Is.Not.EqualTo(DateTime.MinValue), Outcome.Succeeded, now);

            AssertEquals(Specify.For<DateTime>().That(x => x.Ticks).Is.EqualTo(0), Outcome.Failed, now);
            AssertEquals(Specify.For<DateTime>().That(x => x.Ticks).Is.Not.EqualTo(0), Outcome.Succeeded, now);
        }

        [Test]
        public void GreaterThan()
        {
            // arrange
            DateTime now = DateTime.Now;
            const int one = 1;

            // act and assert
            AssertEquals(Specify.For<DateTime>().That(x => x.Ticks).Is.Not.GreaterThan(0), Outcome.Failed, now);
            AssertEquals(Specify.For<DateTime>().That(x => x.Ticks).Is.GreaterThan(0), Outcome.Succeeded, now);
            AssertEquals(Specify.ThatAny<int>().Is.Not.GreaterThan(0), Outcome.Failed, one);
            AssertEquals(Specify.ThatAny<int>().Is.GreaterThan(0), Outcome.Succeeded, one);
        }

        [Test]
        public void GreaterThanOrEqualTo()
        {
            // arrange
            DateTime now = DateTime.Now;
            const int one = 1;

            // act and assert
            AssertEquals(Specify.For<DateTime>().That(x => x.Ticks).Is.Not.GreaterThanOrEqualTo(0), Outcome.Failed, now);
            AssertEquals(Specify.For<DateTime>().That(x => x.Ticks).Is.GreaterThanOrEqualTo(0), Outcome.Succeeded, now);
            AssertEquals(Specify.ThatAny<int>().Is.Not.GreaterThanOrEqualTo(0), Outcome.Failed, one);
            AssertEquals(Specify.ThatAny<int>().Is.GreaterThanOrEqualTo(0), Outcome.Succeeded, one);
        }

        [Test]
        public void LessThan()
        {
            // arrange
            DateTime now = DateTime.Now;
            const int one = 1;

            // act and assert
            AssertEquals(Specify.For<DateTime>().That(x => x.Ticks).Is.LessThan(0), Outcome.Failed, now);
            AssertEquals(Specify.For<DateTime>().That(x => x.Ticks).Is.Not.LessThan(0), Outcome.Succeeded, now);
            AssertEquals(Specify.ThatAny<int>().Is.LessThan(0), Outcome.Failed, one);
            AssertEquals(Specify.ThatAny<int>().Is.Not.LessThan(0), Outcome.Succeeded, one);
        }

        [Test]
        public void LessThanOrEqualTo()
        {
            // arrange
            DateTime now = DateTime.Now;
            const int one = 1;

            // act and assert
            AssertEquals(Specify.For<DateTime>().That(x => x.Ticks).Is.LessThanOrEqualTo(0), Outcome.Failed, now);
            AssertEquals(Specify.For<DateTime>().That(x => x.Ticks).Is.Not.LessThanOrEqualTo(0), Outcome.Succeeded, now);
            AssertEquals(Specify.ThatAny<int>().Is.LessThanOrEqualTo(0), Outcome.Failed, one);
            AssertEquals(Specify.ThatAny<int>().Is.Not.LessThanOrEqualTo(0), Outcome.Succeeded, one);
        }

        private void AssertEquals<T1, T2>(IPrintableSpecification<T1, T2> specification, Outcome outcome, T1 value)
        {
            IPrintableEvaluation<T1, T2> evaluation = specification.Evaluate(value);
            Assert.That(evaluation.Result.Outcome, NUnit.Framework.Is.EqualTo(outcome));
            Assert.That(evaluation.Emitted.Retrieved.Value, NUnit.Framework.Is.Not.Empty);
        }

        private static void AssertEquals<T>(IPrintableSpecification<T> specification, Outcome outcome, T value)
        {
            IPrintableEvaluation<T> evaluation = specification.Evaluate(value);
            Assert.That(evaluation.Result.Outcome, NUnit.Framework.Is.EqualTo(outcome));
            Assert.That(evaluation.Emitted.Retrieved.Value, NUnit.Framework.Is.Not.Empty);
        }
    }
}
