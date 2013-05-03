#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Builders.OfProcedures;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Testing;
using Stile.Tests.Prototypes.Specifications.SampleObjects;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Construction
{
	[TestFixture]
	public class CollectionSpecifyAcceptanceTests
	{
		[Test]
		public void BoundToExpression()
		{
			IBoundSpecification
				<Baz<int>, ICollection<int>, IFluentEnumerableBoundExpectationBuilder<Baz<int>, ICollection<int>, int>>
				specification =
					Specify.For(() => new Baz<int>())
						.That(x => x.CollectionIdentity())
						.CollectingItemsLike(0)
						.Has.All.ItemsSatisfying(x => x > 3);
			Assert.That(specification, Is.Not.Null);
			Assert.DoesNotThrow(() => specification.Evaluate());
		}
	}
}
