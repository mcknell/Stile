#region License info...
// Stile for .NET, Copyright 2011-2013 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using Stile.Prototypes.Specifications;
using Stile.Prototypes.Specifications.Builders.OfExpectations;
using Stile.Prototypes.Specifications.SemanticModel.Evaluations;
using Stile.Tests.Prototypes.Specifications.SampleObjects;
using NUnitFramework = NUnit.Framework;
#endregion

namespace Stile.Tests.Prototypes.Specifications.Builders.OfExpectations.Is
{
	[NUnitFramework.TestFixtureAttribute]
	public class EnumerableIsTests
	{
		[NUnitFramework.TestAttribute]
		public void Empty()
		{
			var subject = new Foo<int>();

			IEvaluation<Foo<int>, Foo<int>> evaluationOfNew = Specify.ThatAny<Foo<int>>().OfItemsLike(0).Is.Empty //
				.AndThen.Is.Not.Empty.Evaluate(() => subject);

			NUnitFramework.Assert.That(evaluationOfNew.Outcome, NUnitFramework.Is.EqualTo(Outcome.Succeeded));

			subject.Add(1);
			IEvaluation<Foo<int>, Foo<int>> evaluationOfOne =
				Specify.ThatAny<Foo<int>>().OfItemsLike(0).Is.Not.Empty.Evaluate(() => subject);

			NUnitFramework.Assert.That(evaluationOfOne.Outcome, NUnitFramework.Is.EqualTo(Outcome.Succeeded));
		}
	}
}
