#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
using System.Globalization;
using NUnit.Framework;
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExceptionFilters;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Has;
using Stile.Prototypes.Specifications.Builders.OfExpectations.Is;
using Stile.Prototypes.Specifications.Builders.OfProcedures;
using Stile.Prototypes.Specifications.Printable;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Prototypes.Specifications.SemanticModel.Specifications;
using Stile.Tests.Prototypes.Specifications.SampleObjects;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Printable.Should
{
	[TestFixture]
	public class ShouldAcceptanceTests
	{
		[Test]
		public void Bound()
		{
			int i = 9;
			IBoundSpecification<int, TypeCode, IFluentBoundExpectationBuilder<int, TypeCode>> specification =
				Specify.For(() => i).That(x => x.GetTypeCode()).Has.HashCode(0).Because("I said so");
			Assert.That(specification.ToShould(),
				Is.EqualTo(@"i.GetTypeCode() should have hashcode 0, because I said so"));
		}

		[Test]
		public void Chained()
		{
			ISpecification<Foo<int>, Foo<int>, IFluentEnumerableExpectationBuilder<Foo<int>, Foo<int>, int>>
				specification = Specify.ThatAny<Foo<int>, int>().Has.All.ItemsSatisfying(x => x >= 0).AndThen.Is.Not.Empty.Because("otherwise it ain't right");
			Assert.That(specification.ToShould(), Is.EqualTo(@"Any Foo<int> should have all items >= 0 initially,
then when measured again, should not be empty, because otherwise it ain't right"));
		}

		[Test]
		public void ExpectationFilter()
		{
			int i = 8;
			IEvaluation<int, string> evaluation =
				Specify.For(() => i)
					.That(x => x.ToString(CultureInfo.InvariantCulture))
					.Throws<ArgumentOutOfRangeException>() //
					.Evaluate();
			Assert.That(evaluation.ToPastTense(),
				Is.EqualTo(@"i.ToString(CultureInfo.InvariantCulture) should throw ArgumentOutOfRangeException
but was ""8"" and no exception was thrown"));

			IFaultSpecification<Foo<int>, IFluentExceptionFilterBuilder<Foo<int>>> specification =
				Specify.ForAny<Foo<int>>().That(x => x.Clear()).Throws<ArgumentOutOfRangeException>();
			Assert.That(specification.ToShould(),
				Is.EqualTo(@"Any Foo<int>.Clear() should throw ArgumentOutOfRangeException"));
		}

		[Test]
		public void Nullable()
		{
			var saboteur = new Saboteur();
			IEvaluation<Saboteur, TimeSpan?> evaluation =
				Specify.For(() => saboteur).That(x => x.Fuse).Is.Null().Evaluate();
			Assert.That(evaluation.ToPastTense(), Is.EqualTo(@"saboteur.Fuse should be null"));
		}

		[Test]
		public void Unbound()
		{
			ISpecification<Foo<int>, Foo<int>, IFluentEnumerableExpectationBuilder<Foo<int>, Foo<int>, int>>
				specification = Specify.ThatAny<Foo<int>, int>().Has.All.ItemsSatisfying(x => x >= 0);
			Assert.That(specification.ToShould(), Is.EqualTo(@"Any Foo<int> should have all items >= 0"));
		}
	}
}
