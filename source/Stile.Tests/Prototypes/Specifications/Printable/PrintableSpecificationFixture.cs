#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Globalization;
using NUnit.Framework;
using Stile.Patterns.SelfDescribingPredicates;
using Stile.Prototypes.Specifications.Emitting;
using Stile.Prototypes.Specifications.Evaluations;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.Printable.Output;
using Stile.Prototypes.Specifications.Printable.Output.Explainers;
using Stile.Types;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Printable
{
    [TestFixture]
    public class PrintableSpecificationFixture
    {
        [Test]
        public void EvaluateSimplest()
        {
            // arrange
            int input = 23;
            Func<int, string> extractor = i => i.ToString(CultureInfo.InvariantCulture);
            Predicate<string> accepter = s => s == "23";
            var description = new ExplainBe<int, string>(Negated.False, "23");
            var specification = new PrintableSpecification<int, string>(extractor.ToLazy(), accepter, description);
            Assert.NotNull(specification);

            // act
            IEmittingEvaluation<int, string, ILazyReadableText> evaluation = specification.Evaluate(input);

            // assert
            Assert.NotNull(evaluation);
            Assert.That(evaluation.Result.Value, Is.EqualTo(input.ToString(CultureInfo.InvariantCulture)));
            Assert.That(evaluation.Result.Outcome, Is.EqualTo(Outcome.Succeeded));
            Assert.That(evaluation.Result.Errors, Is.Empty);
            Assert.That(evaluation.Emitted.Retrieved.Value,
                Is.EqualTo(string.Format("be \"23\" {0} and was \"23\"", Environment.NewLine)));

            // arrange again
            input = 24;

            // act again
            evaluation = specification.Evaluate(input);

            // assert again
            Assert.That(evaluation.Result.Value, Is.EqualTo(input.ToString(CultureInfo.InvariantCulture)));
            Assert.That(evaluation.Result.Outcome, Is.EqualTo(Outcome.Failed));
            Assert.That(evaluation.Result.Errors, Is.Empty);
            Assert.That(evaluation.Emitted.Retrieved.Value,
                Is.EqualTo(string.Format("be \"23\" {0} but was \"24\"", Environment.NewLine)));
        }

        [Test]
        public void EvaluateSimplest_Negated()
        {
            // arrange
            int input = 23;
            Func<int, string> extractor = i => i.ToString(CultureInfo.InvariantCulture);
            Predicate<string> accepter = s => s == "23";
            var description = new ExplainBe<int, string>(Negated.True, "24");
            var specification = new PrintableSpecification<int, string>(extractor.ToLazy(), accepter, description);
            Assert.NotNull(specification);

            // act
            IEmittingEvaluation<int, string, ILazyReadableText> evaluation = specification.Evaluate(input);

            // assert
            Assert.NotNull(evaluation);
            Assert.That(evaluation.Result.Value, Is.EqualTo(input.ToString(CultureInfo.InvariantCulture)));
            Assert.That(evaluation.Result.Outcome, Is.EqualTo(Outcome.Succeeded));
            Assert.That(evaluation.Result.Errors, Is.Empty);
            Assert.That(evaluation.Emitted.Retrieved.Value,
                Is.EqualTo(string.Format("not be \"24\" {0} and was \"23\"", Environment.NewLine)));

            // arrange again
            input = 24;

            // act again
            evaluation = specification.Evaluate(input);

            // assert again
            Assert.That(evaluation.Result.Value, Is.EqualTo(input.ToString(CultureInfo.InvariantCulture)));
            Assert.That(evaluation.Result.Outcome, Is.EqualTo(Outcome.Failed));
            Assert.That(evaluation.Result.Errors, Is.Empty);
            Assert.That(evaluation.Emitted.Retrieved.Value,
                Is.EqualTo(string.Format("not be \"24\" {0} but was \"24\"", Environment.NewLine)));
        }

        [Test]
        public void PrintConjunction()
        {
            Assert.That(PrintableSpecification<int, string>.PrintConjunction(Outcome.Succeeded), Is.EqualTo("and"));
            Assert.That(PrintableSpecification<int, string>.PrintConjunction(Outcome.Failed), Is.EqualTo("but"));
        }
    }
}
