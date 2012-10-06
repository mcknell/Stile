#region License info...
// Stile for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Prototypes.Specifications.DSL.SemanticModel.Evaluations;
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

            AssertEquals(Specify.ForAny<DateTime>().That(x => x.Ticks).Is.ComparablyEquivalentTo(0), Outcome.Failed, now);
            AssertEquals(Specify.ForAny<DateTime>().That(x => x.Ticks).Is.Not.ComparablyEquivalentTo(0), Outcome.Succeeded, now);
        }

        [Test]
        public void EqualTo()
        {
            // arrange
            DateTime now = DateTime.Now;

            // act and assert
            AssertEquals(Specify.ThatAny<DateTime>().Is.EqualTo(DateTime.MinValue), Outcome.Failed, now);
            AssertEquals(Specify.ThatAny<DateTime>().Is.Not.EqualTo(DateTime.MinValue), Outcome.Succeeded, now);

            AssertEquals(Specify.ForAny<DateTime>().That(x => x.Ticks).Is.EqualTo(0), Outcome.Failed, now);
            AssertEquals(Specify.ForAny<DateTime>().That(x => x.Ticks).Is.Not.EqualTo(0), Outcome.Succeeded, now);
        }

        [Test]
        public void False()
        {
            // arrange
            string empty = string.Empty;

            // act and assert
            AssertEquals(Specify.ForAny<string>().That(x => x.IsNormalized()).Is.False(), Outcome.Failed, empty);
            AssertEquals(Specify.ForAny<string>().That(x => x.IsNormalized()).Is.Not.False(), Outcome.Succeeded, empty);
            AssertEquals(Specify.ThatAny<bool>().Is.Not.False(), Outcome.Failed, false);
            AssertEquals(Specify.ThatAny<bool>().Is.False(), Outcome.Succeeded, false);
        }

        [Test]
        public void GreaterThan()
        {
            // arrange
            DateTime now = DateTime.Now;
            const int one = 1;

            // act and assert
            AssertEquals(Specify.ForAny<DateTime>().That(x => x.Ticks).Is.Not.GreaterThan(0), Outcome.Failed, now);
            AssertEquals(Specify.ForAny<DateTime>().That(x => x.Ticks).Is.GreaterThan(0), Outcome.Succeeded, now);
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
            AssertEquals(Specify.ForAny<DateTime>().That(x => x.Ticks).Is.Not.GreaterThanOrEqualTo(0), Outcome.Failed, now);
            AssertEquals(Specify.ForAny<DateTime>().That(x => x.Ticks).Is.GreaterThanOrEqualTo(0), Outcome.Succeeded, now);
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
            AssertEquals(Specify.ForAny<DateTime>().That(x => x.Ticks).Is.LessThan(0), Outcome.Failed, now);
            AssertEquals(Specify.ForAny<DateTime>().That(x => x.Ticks).Is.Not.LessThan(0), Outcome.Succeeded, now);
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
            AssertEquals(Specify.ForAny<DateTime>().That(x => x.Ticks).Is.LessThanOrEqualTo(0), Outcome.Failed, now);
            AssertEquals(Specify.ForAny<DateTime>().That(x => x.Ticks).Is.Not.LessThanOrEqualTo(0), Outcome.Succeeded, now);
            AssertEquals(Specify.ThatAny<int>().Is.LessThanOrEqualTo(0), Outcome.Failed, one);
            AssertEquals(Specify.ThatAny<int>().Is.Not.LessThanOrEqualTo(0), Outcome.Succeeded, one);
        }

        [Test]
        public void Null()
        {
            // arrange
            string empty = string.Empty;

            // act and assert
            AssertEquals(Specify.ForAny<string>().That(x => x.ToLowerInvariant()).Is.Null(), Outcome.Failed, empty);
            AssertEquals(Specify.ForAny<string>().That(x => x.ToLowerInvariant()).Is.Not.Null(), Outcome.Succeeded, empty);
            AssertEquals(Specify.ThatAny<string>().Is.Null(), Outcome.Failed, empty);
            AssertEquals(Specify.ThatAny<string>().Is.Not.Null(), Outcome.Succeeded, empty);
            const int one = 1;
            AssertEquals(Specify.ForAny<int?>().That(x => x).Is.Null(), Outcome.Failed, one);
            AssertEquals(Specify.ForAny<int?>().That(x => x).Is.Not.Null(), Outcome.Succeeded, one);
            AssertEquals(Specify.ThatAny<int?>().Is.Null(), Outcome.Failed, one);
            AssertEquals(Specify.ThatAny<int?>().Is.Not.Null(), Outcome.Succeeded, one);
        }

        [Test]
        public void True()
        {
            // arrange
            string empty = string.Empty;

            // act and assert
            AssertEquals(Specify.ForAny<string>().That(x => x.IsNormalized()).Is.Not.True(), Outcome.Failed, empty);
            AssertEquals(Specify.ForAny<string>().That(x => x.IsNormalized()).Is.True(), Outcome.Succeeded, empty);
            AssertEquals(Specify.ThatAny<bool>().Is.True(), Outcome.Failed, false);
            AssertEquals(Specify.ThatAny<bool>().Is.Not.True(), Outcome.Succeeded, false);
        }

        internal static void AssertEquals<T1, T2>(IPrintableSpecification<T1, T2> specification, Outcome outcome, T1 value)
        {
            IPrintableEvaluation<T2> evaluation = specification.Evaluate(value);
            Assert.That(evaluation.Result.Outcome, NUnit.Framework.Is.EqualTo(outcome));
            Assert.That(evaluation.Emitted.Retrieved.Value, NUnit.Framework.Is.Not.Empty);
        }
    }
}
