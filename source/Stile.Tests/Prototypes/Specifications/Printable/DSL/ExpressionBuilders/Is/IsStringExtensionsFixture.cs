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
    public class IsStringExtensionsFixture
    {
        private const string Seafood = "seafood";
        private INegatableIs<string, string, IIs<string, string>> _instrumentedNegatableIs;
        private INegatableIs<string, IIs<string>> _negatableIs;

        [SetUp]
        public void Init()
        {
            _instrumentedNegatableIs = Specify.For<string>().That(x => x.ToLowerInvariant()).Is;
            _negatableIs = Specify.ThatAny<string>().Is;
        }

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

        private void AssertInstrumentedStringFunction(Func<IIs<string, string>, IPrintableSpecification<string, string>> func,
            string s)
        {
            IsExtensionsFixture.AssertEquals(func.Invoke(_instrumentedNegatableIs.Not), Outcome.Failed, s);
            IsExtensionsFixture.AssertEquals(func.Invoke(_instrumentedNegatableIs), Outcome.Succeeded, s);
        }

        private void AssertStringFunction(Func<IIs<string>, IPrintableSpecification<string>> func, string s)
        {
            IsExtensionsFixture.AssertEquals(func.Invoke(_negatableIs.Not), Outcome.Failed, s);
            IsExtensionsFixture.AssertEquals(func.Invoke(_negatableIs), Outcome.Succeeded, s);
        }
    }
}
