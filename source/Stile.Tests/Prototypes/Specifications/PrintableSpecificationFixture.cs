#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Propter project on GitHub
#endregion

#region using...
using System;
using System.Globalization;
using NUnit.Framework;
using Stile.Prototypes.Specifications.Emitting;
using Stile.Prototypes.Specifications.Evaluations;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.Printable.Output;
#endregion

namespace Stile.Tests.Prototypes.Specifications
{
    [TestFixture]
    public class PrintableSpecificationFixture
    {
        [Test]
        public void EvaluateSimplest()
        {
            Func<int, string> extractor = i => i.ToString(CultureInfo.InvariantCulture);
            Predicate<string> accepter = s => true;
            var description = new Description<string>(ExpectationVerb.Be);
            var specification = new PrintableSpecification<int, string>(extractor, accepter, description);
            Assert.NotNull(specification);
            IEmittingEvaluation<string, ILazyReadableText> evaluation = specification.Evaluate(23);
            Assert.NotNull(evaluation);
            Assert.That(evaluation.Result.Value, Is.EqualTo("23"));
            Assert.That(evaluation.Result.Outcome, Is.EqualTo(Outcome.Succeeded));
            Assert.That(evaluation.Result.Errors, Is.Empty);
        }
    }
}
