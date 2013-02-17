#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using NUnit.Framework;
using Stile.Prototypes.Specifications.Builders.OfSpecifications;
using Stile.Prototypes.Specifications.SemanticModel;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
#endregion

namespace Stile.Tests.Prototypes.Specifications.SemanticModel
{
	[TestFixture]
	public class ThrowingSpecificationFactoryFixture
	{
		[Test]
		public void Foo()
		{
			var instrument = new ThrowingInstrument<string>(s => s.IndexOf(string.Empty));
			IThrowingSpecificationBuilder<IThrowingSpecification<string>, string, ArgumentOutOfRangeException> specification =
				ThrowingSpecificationFactory.Resolve
					<IThrowingSpecification<string>, string, ArgumentOutOfRangeException>(
						instrument);
			Assert.That(specification, Is.Not.Null);
		}
	}
}
