#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Builders.OfProcedures;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Tests.Prototypes.Specifications.SampleObjects;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Construction
{
	[TestFixture]
	public class EnumerableSpecifyAcceptanceTests
	{
		[Test]
		public void BoundToExpression()
		{
			IBoundSpecification
				<Foo<int>, IEnumerable<int>, IFluentEnumerableBoundExpectationBuilder<Foo<int>, IEnumerable<int>, int>>
				specification =
					Specify.For(() => new Foo<int>())
						.That(x => Enumerable.Reverse(x))
						.OfItemsLike(0)
						.Has.All.ItemsSatisfying(x => x > 3);
			Assert.That(specification, Is.Not.Null);
			Assert.DoesNotThrow(() => specification.Evaluate());
		}

/*
		[Test]
		public void BoundToInstance()
		{
			IBoundSpecification
				<Foo<int>, IEnumerable<int>, IFluentEnumerableBoundExpectationBuilder<Foo<int>, IEnumerable<int>, int>>
				specification =
					Specify.For(()=>new Foo<int>())
						.That(x => Enumerable.Reverse(x))
						.OfItemsLike(0)
						.Has.All.ItemsSatisfying(x => x > 3);
			Assert.That(specification, Is.Not.Null);
			Assert.DoesNotThrow(() => specification.Evaluate());
		}
*/

		[Test]
		public void Unbound()
		{
			ISpecification<Foo<int>, Foo<int>> specification =
				Specify.ThatAny<Foo<int>>().OfItemsLike(0).Has.All.ItemsSatisfying(x => x > 3);
			Assert.That(specification, Is.Not.Null);
			Assert.DoesNotThrow(() => specification.Evaluate(() => new Foo<int>()));
		}
	}
}
