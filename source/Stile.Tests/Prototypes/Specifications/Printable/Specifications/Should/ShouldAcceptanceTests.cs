#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfInstruments;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Printable.Specifications.Should
{
	[TestFixture]
	public class ShouldAcceptanceTests
	{
		[Test]
		public void Bound()
		{
			int i = 9;
			IBoundSpecification<int, TypeCode, IFluentBoundExpectationBuilder<int, TypeCode>> specification =
				Specify.For(() => i).That(x => x.GetTypeCode()).Has.HashCode(0);
			Assert.That(specification.ToShould(), Is.EqualTo(@"i.GetTypeCode() should have hashcode 0"));
		}
	}
}
