#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfInstruments;
using Stile.Prototypes.Specifications.Builders.OfPredicates.Has;
using Stile.Prototypes.Specifications.Builders.OfPredicates.Is;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Tests.Prototypes.Specifications.SampleObjects;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Construction
{
	[TestFixture]
	public class SpecifyAcceptanceTests
	{
		[Test]
		public void BoundToExpression()
		{
			ISpecification<Foo<int>, int> specification =
				Specify.For(() => new Foo<int>()).That(x => x.Jump()).Is.ComparablyEquivalentTo(7);
			Assert.That(specification, Is.Not.Null);
		}

		[Test]
		public void BoundToInstance()
		{
			ISpecification<Foo<int>, int> specification = Specify.For(new Foo<int>()).That(x => x.Jump()).Is.Not.EqualTo(12);
			Assert.That(specification, Is.Not.Null);
		}

		[Test]
		public void Unbound()
		{
			ISpecification<Foo<int>, string> specification =
				Specify.ForAny<Foo<int>>().That(x => x.ToString()).Has.HashCode(45);
			Assert.That(specification, Is.Not.Null);
		}
	}
}
