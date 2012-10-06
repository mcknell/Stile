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
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs;
using Stile.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.SubjectBuilders;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Printable.DSL.ExpressionBuilders.ResultIs
{
    [TestFixture]
    public class IsStringExtensionsFixture
    {
        private const string Seafood = "seafood";

        [Test]
        public void NullOrEmpty()
        {
            AssertStringFunction(IsStringExtensions.NullOrEmpty, string.Empty);
            AssertInstrumentedStringFunction(IsStringExtensions.NullOrEmpty, string.Empty);
        }

        [Test]
        public void NullOrWhitespace()
        {
            AssertStringFunction(IsStringExtensions.NullOrWhitespace, "\t");
            AssertInstrumentedStringFunction(IsStringExtensions.NullOrWhitespace, "\t");
        }

        [Test]
        public void StringContaining()
        {
            AssertStringFunction(@is => @is.StringContaining("foo"), Seafood);
            AssertInstrumentedStringFunction(@is => @is.StringContaining("foo"), Seafood);
        }

        [Test]
        public void StringEndingWith()
        {
            AssertStringFunction(@is => @is.StringEndingWith("ood"), Seafood);
            AssertInstrumentedStringFunction(@is => @is.StringEndingWith("ood"), Seafood);
        }

        [Test]
        public void StringStartingWith()
        {
            AssertStringFunction(@is => @is.StringStartingWith("sea"), Seafood);
            AssertInstrumentedStringFunction(@is => @is.StringStartingWith("sea"), Seafood);
        }

        private void AssertInstrumentedStringFunction(
            Func<IPrintableIs<string, string>, IPrintableSpecification<string, string>> func, string s)
        {
            IPrintableNegatableIs<string, string, IPrintableIs<string, string>> instrumentedNegatableIs =
                Specify.ForAny<string>().That(x => x.ToLowerInvariant()).Is;
            IsExtensionsFixture.AssertEquals(func.Invoke(instrumentedNegatableIs.Not), Outcome.Failed, s);
            IsExtensionsFixture.AssertEquals(func.Invoke(instrumentedNegatableIs), Outcome.Succeeded, s);
        }

        private void AssertStringFunction(Func<IPrintableIs<string, string>, IPrintableSpecification<string, string>> func, string s)
        {
            IPrintableNegatableIs<string, string, IPrintableIs<string, string>> negatableIs = Specify.ThatAny<string>().Is;
            IsExtensionsFixture.AssertEquals(func.Invoke(negatableIs.Not), Outcome.Failed, s);
            IsExtensionsFixture.AssertEquals(func.Invoke(negatableIs), Outcome.Succeeded, s);
        }
    }
}
